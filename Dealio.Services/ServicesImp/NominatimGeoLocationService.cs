using Dealio.Domain.Entities;
using Dealio.Infrastructure.Repositories.Interfaces;
using Dealio.Services.Interfaces;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Web;
using System.Globalization;

namespace Dealio.Services.ServicesImp
{
    public class NominatimGeoLocationService : IGeoLocationService
    {
        private readonly HttpClient _httpClient;
        private readonly IAddressRepository _addressRepo;
        private readonly SemaphoreSlim _rateLimitSemaphore;

        public NominatimGeoLocationService(HttpClient httpClient, IAddressRepository addressRepo)
        {
            _httpClient = httpClient;
            _addressRepo = addressRepo;
            // Nominatim rate limiting: 1 request per second
            _rateLimitSemaphore = new SemaphoreSlim(1, 1);

            // Configure HttpClient
            _httpClient.DefaultRequestHeaders.Add("User-Agent", "DealioApp/1.0 (contact@dealio.com)");
            _httpClient.Timeout = TimeSpan.FromSeconds(30);
        }

        public async Task<(double lat, double lon)> GetCoordinatesAsync(Address address)
        {
            try
            {
                // Return cached coordinates if available
                if (address.Latitude != 0 && address.Longitude != 0)
                    return (address.Latitude, address.Longitude);

                // Validate address input
                if (!IsValidAddress(address))
                {
                    Console.WriteLine($"Invalid address provided: {FormatAddressForLogging(address)}");
                    throw new ArgumentException("Address must contain at least street or city information");
                }

                var coordinates = await FetchCoordinatesFromNominatim(address);

                if (coordinates.HasValue)
                {
                    // Cache coordinates in database
                    await UpdateAddressCoordinates(address, coordinates.Value.lat, coordinates.Value.lon);
                    return coordinates.Value;
                }

                throw new Exception($"Coordinates not found for address: {FormatAddressForLogging(address)}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error getting coordinates for address {FormatAddressForLogging(address)}: {ex.Message}");
                throw;
            }
        }

        private async Task<(double lat, double lon)?> FetchCoordinatesFromNominatim(Address address)
        {
            await _rateLimitSemaphore.WaitAsync();

            try
            {
                // Build query with fallback strategy
                var queries = BuildSearchQueries(address);

                foreach (var query in queries)
                {
                    if (string.IsNullOrWhiteSpace(query))
                        continue;

                    Console.WriteLine($"Searching coordinates for: {query}");

                    var result = await ExecuteNominatimQuery(query);
                    if (result.HasValue)
                        return result;

                    // Rate limiting - wait 1 second between requests
                    await Task.Delay(1000);
                }

                return null;
            }
            finally
            {
                _rateLimitSemaphore.Release();
            }
        }

        private async Task<(double lat, double lon)?> ExecuteNominatimQuery(string query)
        {
            try
            {
                string encodedQuery = HttpUtility.UrlEncode(query);
                string url = $"https://nominatim.openstreetmap.org/search?q={encodedQuery}&format=json&limit=1&countrycodes=eg";

                var response = await _httpClient.GetAsync(url);

                if (!response.IsSuccessStatusCode)
                {
                    Console.WriteLine($"Nominatim API returned status: {response.StatusCode}");
                    return null;
                }

                var json = await response.Content.ReadAsStringAsync();

                if (string.IsNullOrWhiteSpace(json) || json == "[]")
                {
                    Console.WriteLine($"No results found for query: {query}");
                    return null;
                }

                var results = JsonSerializer.Deserialize<List<NominatimResult>>(json, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });

                if (results == null || results.Count == 0)
                {
                    Console.WriteLine($"Empty results for query: {query}");
                    return null;
                }

                var result = results[0];

                // Validate and parse coordinates
                if (string.IsNullOrWhiteSpace(result.Lat) || string.IsNullOrWhiteSpace(result.Lon))
                {
                    Console.WriteLine($"Invalid coordinates returned - Lat: '{result.Lat}', Lon: '{result.Lon}'");
                    return null;
                }

                // Use invariant culture for parsing to handle different decimal separators
                if (!double.TryParse(result.Lat, NumberStyles.Float, CultureInfo.InvariantCulture, out double latitude))
                {
                    Console.WriteLine($"Failed to parse latitude: '{result.Lat}'");
                    return null;
                }

                if (!double.TryParse(result.Lon, NumberStyles.Float, CultureInfo.InvariantCulture, out double longitude))
                {
                    Console.WriteLine($"Failed to parse longitude: '{result.Lon}'");
                    return null;
                }

                // Validate coordinate ranges
                if (!IsValidCoordinate(latitude, longitude))
                {
                    Console.WriteLine($"Invalid coordinate values - Lat: {latitude}, Lon: {longitude}");
                    return null;
                }

                Console.WriteLine($"Successfully found coordinates: {latitude}, {longitude}");
                return (latitude, longitude);
            }
            catch (HttpRequestException ex)
            {
                Console.WriteLine($"Network error calling Nominatim API: {ex.Message}");
                return null;
            }
            catch (TaskCanceledException ex)
            {
                Console.WriteLine($"Nominatim API request timeout: {ex.Message}");
                return null;
            }
            catch (JsonException ex)
            {
                Console.WriteLine($"Error parsing Nominatim response: {ex.Message}");
                return null;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Unexpected error in Nominatim query: {ex.Message}");
                return null;
            }
        }

        private List<string> BuildSearchQueries(Address address)
        {
            var queries = new List<string>();

            // Full address query
            var parts = new List<string>();
            if (!string.IsNullOrWhiteSpace(address.Street))
                parts.Add(address.Street.Trim());
            if (!string.IsNullOrWhiteSpace(address.Region))
                parts.Add(address.Region.Trim());
            if (!string.IsNullOrWhiteSpace(address.City))
                parts.Add(address.City.Trim());
            parts.Add("Egypt"); // Always include Egypt

            if (parts.Count > 1)
                queries.Add(string.Join(", ", parts));

            // Fallback queries with less specificity
            if (!string.IsNullOrWhiteSpace(address.City))
            {
                queries.Add($"{address.City}, Egypt");

                if (!string.IsNullOrWhiteSpace(address.Region))
                    queries.Add($"{address.City}, {address.Region}, Egypt");
            }

            if (!string.IsNullOrWhiteSpace(address.Region))
                queries.Add($"{address.Region}, Egypt");

            return queries.Where(q => !string.IsNullOrWhiteSpace(q)).Distinct().ToList();
        }

        private bool IsValidAddress(Address address)
        {
            return !string.IsNullOrWhiteSpace(address.Street) ||
                   !string.IsNullOrWhiteSpace(address.City) ||
                   !string.IsNullOrWhiteSpace(address.Region);
        }

        private bool IsValidCoordinate(double latitude, double longitude)
        {
            // Egypt coordinate bounds (approximate)
            // Latitude: 22°N to 31.5°N
            // Longitude: 25°E to 35°E
            return latitude >= 22.0 && latitude <= 31.5 &&
                   longitude >= 25.0 && longitude <= 35.0;
        }

        private string FormatAddressForLogging(Address address)
        {
            var parts = new List<string>();
            if (!string.IsNullOrWhiteSpace(address.Street)) parts.Add($"Street: {address.Street}");
            if (!string.IsNullOrWhiteSpace(address.City)) parts.Add($"City: {address.City}");
            if (!string.IsNullOrWhiteSpace(address.Region)) parts.Add($"Region: {address.Region}");

            return parts.Any() ? string.Join(", ", parts) : "Empty Address";
        }

        private async Task UpdateAddressCoordinates(Address address, double latitude, double longitude)
        {
            try
            {
                address.Latitude = latitude;
                address.Longitude = longitude;
                await _addressRepo.UpdateAsync(address);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error updating address coordinates: {ex.Message}");
                // Don't throw - coordinates can still be used even if not cached
            }
        }

        private class NominatimResult
        {
            [JsonPropertyName("lat")]
            public string Lat { get; set; } = string.Empty;

            [JsonPropertyName("lon")]
            public string Lon { get; set; } = string.Empty;

            [JsonPropertyName("display_name")]
            public string DisplayName { get; set; } = string.Empty;

            [JsonPropertyName("importance")]
            public double? Importance { get; set; }
        }
    }
}