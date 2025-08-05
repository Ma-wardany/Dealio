using Dealio.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dealio.Services.Interfaces
{
    public interface IGeoLocationService
    {
        Task<(double lat, double lon)> GetCoordinatesAsync(Address address);
    }
}
