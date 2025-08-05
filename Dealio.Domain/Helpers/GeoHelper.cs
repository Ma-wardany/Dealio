using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dealio.Domain.Helpers
{
    public static class GeoHelper
    {
        public static double CalculateDistance((double lat, double lon) a, (double lat, double lon) b)
        {
            var R = 6371; // Earth radius in km
            var dLat = DegreeToRad(b.lat - a.lat);
            var dLon = DegreeToRad(b.lon - a.lon);

            var lat1 = DegreeToRad(a.lat);
            var lat2 = DegreeToRad(b.lat);

            var a1 = Math.Sin(dLat / 2) * Math.Sin(dLat / 2) +
                     Math.Sin(dLon / 2) * Math.Sin(dLon / 2) * Math.Cos(lat1) * Math.Cos(lat2);
            var c = 2 * Math.Atan2(Math.Sqrt(a1), Math.Sqrt(1 - a1));
            return R * c;
        }

        private static double DegreeToRad(double deg) => deg * (Math.PI / 180);
    }
}