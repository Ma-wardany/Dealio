using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dealio.Domain.Entities
{
    public class Commission
    {
        //[Key]
        public int Id { get; set; }

        public DateTime TransferDate { get; set; }
        public double CommissionPercentage { get; set; }
        public decimal CommissionAmount { get; set; }

        public int OrderId { get; set; }
        public Order Order { get; set; }
    }
}
