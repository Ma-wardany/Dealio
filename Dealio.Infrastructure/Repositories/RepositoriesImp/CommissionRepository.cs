using Dealio.Domain.Entities;
using Dealio.Infrastructure.Bases;
using Dealio.Infrastructure.DbContexts;
using Dealio.Infrastructure.Repositories.Interfaces;


namespace Dealio.Infrastructure.Repositories.RepositoriesImp
{
    public class CommissionRepository : GenericRepository<Commission>, ICommissionRepository
    {
        public CommissionRepository(ApplicationDbContext dbContext) : base(dbContext)
        {
        }
    }
}
