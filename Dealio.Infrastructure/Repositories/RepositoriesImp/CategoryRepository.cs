using Dealio.Domain.Entities;
using Dealio.Infrastructure.Bases;
using Dealio.Infrastructure.DbContexts;
using Dealio.Infrastructure.Repositories.Interfaces;


namespace Dealio.Infrastructure.Repositories.RepositoriesImp
{
    public class CategoryRepository : GenericRepository<Category>, ICategoryRepository
    {
        public CategoryRepository(ApplicationDbContext dbContext) : base(dbContext)
        {
        }
    }
}
