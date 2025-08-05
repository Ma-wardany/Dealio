using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dealio.Domain.Entities
{
    public class Payment
    {
        public int Id { get; set; }

        public string PaymentStatus { get; set; }
        public string PaymentMethod { get; set; }
        public string CardInfo { get; set; }

        public string BuyerId { get; set; }
        public UserProfile Buyer { get; set; }  

        public int OrderId { get; set; }
        public Order Order { get; set; }

    }
}

