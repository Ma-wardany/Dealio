using Dealio.Infrastructure.Repositories.Interfaces;
using Dealio.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dealio.Services.ServicesImp
{
    public class CommissionServices : ICommissionServices
    {
        private readonly ICommissionRepository commissionRepository;

        public CommissionServices(ICommissionRepository commissionRepository)
        {
            this.commissionRepository = commissionRepository;
        }
    }
}
