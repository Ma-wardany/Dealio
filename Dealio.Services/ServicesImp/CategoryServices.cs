using Dealio.Infrastructure.Repositories.Interfaces;
using Dealio.Services.Interfaces;

namespace Dealio.Services.ServicesImp
{
    public class CategoryServices : ICategoryServices
    {
        private readonly ICategoryRepository categoryRepository;

        public CategoryServices(ICategoryRepository categoryRepository)
        {
            this.categoryRepository = categoryRepository;
        }
    }
}
