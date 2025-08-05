using Dealio.Domain.Entities;
using Dealio.Infrastructure.Repositories.Interfaces;
using Dealio.Services.Interfaces;
using System.Text.Json;
using System.Web;

namespace Dealio.Services.ServicesImp
{
    public class NominatimGeoLocationService : IGeoLocationService
    {
        private readonly HttpClient _httpClient;
        private readonly IAddressRepository _addressRepo;

        public NominatimGeoLocationService(HttpClient httpClient, IAddressRepository addressRepo)
        {
            _httpClient = httpClient;
            _addressRepo = addressRepo;
        }

        public async Task<(double lat, double lon)> GetCoordinatesAsync(Address address)
        {
            if (address.Latitude != 0 && address.Longitude != 0)
                return (address.Latitude, address.Longitude);

            string query = $"{address.Street}, {address.Region}, {address.City}, Egypt";
            string encodedQuery = HttpUtility.UrlEncode(query);
            string url = $"https://nominatim.openstreetmap.org/search?q={encodedQuery}&format=json&limit=1";

            var request = new HttpRequestMessage(HttpMethod.Get, url);
            request.Headers.Add("User-Agent", "DealioApp");

            var response = await _httpClient.SendAsync(request);
            response.EnsureSuccessStatusCode();

            var json = await response.Content.ReadAsStringAsync();
            var result = JsonSerializer.Deserialize<List<NominatimResult>>(json);

            if (result is { Count: > 0 })
            {
                double lat = double.Parse(result[0].Lat);
                double lon = double.Parse(result[0].Lon);

                // خزّن الإحداثيات في Address
                address.Latitude = lat;
                address.Longitude = lon;
                await _addressRepo.UpdateAsync(address);

                return (lat, lon);
            }

            throw new Exception("Coordinates not found.");
        }

        private class NominatimResult
        {
            public string Lat { get; set; }
            public string Lon { get; set; }
        }
    }
}

