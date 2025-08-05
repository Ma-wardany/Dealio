using Dealio.Domain.Entities;
using Dealio.Infrastructure.Bases;
using Dealio.Infrastructure.DbContexts;
using Dealio.Infrastructure.Repositories.Interfaces;


namespace Dealio.Infrastructure.Repositories.RepositoriesImp
{
    public class SellerTransactionRepository : GenericRepository<SellerTransaction>, ISellerTransactionRepository
    {
        public SellerTransactionRepository(ApplicationDbContext dbContext) : base(dbContext)
        {
        }
    }
}
