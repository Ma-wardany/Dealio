using Dealio.Domain.Entities;
using Dealio.Services.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dealio.Services.Interfaces
{
    public interface IRatingServices
    {
        public Task<ServiceResult<Rating>> AddRatingAsync(Rating rate);

        public Task<ServiceResultEnum> DeleteRatingAsync(string userId);


        public Task<ServiceResult<Rating>> UpdateRatingAsync(Rating rating);

        public Task<ServiceResult<Rating>> GetUserRatingAsync(string userId);

        public Task<ServiceResult<IQueryable<Rating>>> GetAllRatingsAsync();

    }
}
