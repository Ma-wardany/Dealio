using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dealio.Domain.Entities
{
    public class SellerTransaction
    {
        public int Id { get; set; }

        public string SellerId { get; set; }
        public UserProfile Seller { get; set; } 


        public string TransferStatus { get; set; }
        public decimal AmountReceived { get; set; }

        public int OrderId { get; set; }
        public Order Order { get; set; }
    }
}
