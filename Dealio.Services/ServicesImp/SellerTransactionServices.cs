using Dealio.Infrastructure.Repositories.Interfaces;
using Dealio.Services.Interfaces;


namespace Dealio.Services.ServicesImp
{
    public class SellerTransactionServices : ISellerTransactionServices
    {
        private readonly ISellerTransactionRepository sellerTransactionRepository;

        public SellerTransactionServices(ISellerTransactionRepository sellerTransactionRepository)
        {
            this.sellerTransactionRepository = sellerTransactionRepository;
        }
    }
}
