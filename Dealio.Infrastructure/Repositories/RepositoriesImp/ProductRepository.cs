using Dealio.Domain.Entities;
using Dealio.Infrastructure.Bases;
using Dealio.Infrastructure.DbContexts;
using Dealio.Infrastructure.Repositories.Interfaces;


namespace Dealio.Infrastructure.Repositories.RepositoriesImp
{
    public class ProductRepository : GenericRepository<Product>, IProductRepository
    {
        public ProductRepository(ApplicationDbContext dbContext) : base(dbContext)
        {
        }
    }
}
