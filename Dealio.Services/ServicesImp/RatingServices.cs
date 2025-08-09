using Dealio.Domain.Entities;
using Dealio.Infrastructure.Repositories.Interfaces;
using Dealio.Services.Helpers;
using Dealio.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using Org.BouncyCastle.Utilities.Collections;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Dealio.Services.ServicesImp
{
    public class RatingServices : IRatingServices
    {
        private readonly IRatingRepository _ratingRepository;

        public RatingServices(IRatingRepository ratingRepository)
        {
            _ratingRepository = ratingRepository;
        }

        public async Task<ServiceResult<Rating>> AddRatingAsync(Rating rate)
        {
            var existRating = await _ratingRepository.GetTableNoTracking()
                .FirstOrDefaultAsync(r => r.UserId == rate.UserId);

            if (existRating != null)
                return ServiceResult<Rating>.Failure(ServiceResultEnum.Already_Exist);

            try
            {
                await _ratingRepository.AddAsync(rate);

                var newRating = await _ratingRepository.GetTableNoTracking()
                                                       .Include(r => r.User)
                                                       .ThenInclude(u => u.ApplicationUser)
                                                       .FirstOrDefaultAsync(r => r.Id == rate.Id);

                return ServiceResult<Rating>.Success(newRating, ServiceResultEnum.Created);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.InnerException + "-----------------------------");
                return ServiceResult<Rating>.Failure(ServiceResultEnum.Failed);
            }
        }

        public async Task<ServiceResultEnum> DeleteRatingAsync(string userId)
        {
            var existRating = await _ratingRepository.GetTableNoTracking()
                .FirstOrDefaultAsync(r => r.UserId == userId);

            if (existRating == null)
                return ServiceResultEnum.NotFound;

            try
            {
                await _ratingRepository.DeleteAsync(existRating);
                return ServiceResultEnum.Deleted;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.InnerException + "-----------------------------");
                return ServiceResultEnum.Failed;
            }
        }

        public async Task<ServiceResult<Rating>> GetUserRatingAsync(string userId)
        {
            var rating = await _ratingRepository.GetTableNoTracking()
                .Include(r => r.User)
                .ThenInclude(u => u.ApplicationUser)
                .FirstOrDefaultAsync(r => r.UserId == userId);

            if (rating == null)
                return ServiceResult<Rating>.Failure(ServiceResultEnum.NotFound);

            return ServiceResult<Rating>.Success(rating, ServiceResultEnum.Success);
        }

        public async Task<ServiceResult<Rating>> UpdateRatingAsync(Rating rating)
        {
            var existRating = await _ratingRepository.GetTableAsTracking()
                .FirstOrDefaultAsync(r => r.UserId == rating.UserId);

            if (existRating == null)
                return ServiceResult<Rating>.Failure(ServiceResultEnum.NotFound);

            try
            {
                existRating.RatingValue = rating.RatingValue;
                existRating.Comment = rating.Comment;
                existRating.UserId = rating.UserId;

                await _ratingRepository.SaveChangesAsync();

                var newRating = await _ratingRepository.GetTableNoTracking()
                                       .Include(r => r.User)
                                       .ThenInclude(u => u.ApplicationUser)
                                       .FirstOrDefaultAsync(r => r.Id == existRating.Id);
                return ServiceResult<Rating>.Success(existRating, ServiceResultEnum.Updated);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.InnerException + "-----------------------------");
                return ServiceResult<Rating>.Failure(ServiceResultEnum.Failed);
            }

        }

        public async Task<ServiceResult<IQueryable<Rating>>> GetAllRatingsAsync()
        {
            var ratings = _ratingRepository.GetTableNoTracking()
                .Include(r => r.User)
                .ThenInclude(u => u.ApplicationUser);

            return ratings.Any()
                ? ServiceResult<IQueryable<Rating>>.Success(ratings, ServiceResultEnum.Success)
                : ServiceResult<IQueryable<Rating>>.Failure(ServiceResultEnum.Empty);
        }
    }

}
