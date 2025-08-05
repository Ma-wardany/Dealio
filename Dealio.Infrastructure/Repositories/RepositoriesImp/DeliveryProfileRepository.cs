using Dealio.Domain.Entities;
using Dealio.Infrastructure.Bases;
using Dealio.Infrastructure.DbContexts;
using Dealio.Infrastructure.Repositories.Interfaces;


namespace Dealio.Infrastructure.Repositories.RepositoriesImp
{
    public class DeliveryProfileRepository : GenericRepository<DeliveryProfile>, IDeliveryProfileRepository
    {
        public DeliveryProfileRepository(ApplicationDbContext dbContext) : base(dbContext)
        {
        }
    }
}
