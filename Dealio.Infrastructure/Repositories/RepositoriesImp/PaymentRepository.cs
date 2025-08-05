using Dealio.Domain.Entities;
using Dealio.Infrastructure.Bases;
using Dealio.Infrastructure.DbContexts;
using Dealio.Infrastructure.Repositories.Interfaces;


namespace Dealio.Infrastructure.Repositories.RepositoriesImp
{
    public class PaymentRepository : GenericRepository<Payment>, IPaymentRepository
    {
        public PaymentRepository(ApplicationDbContext dbContext) : base(dbContext)
        {
        }
    }
}
