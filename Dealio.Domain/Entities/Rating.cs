using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dealio.Domain.Entities
{
    public class Rating
    {
        [Key]
        public int Id { get; set; }

        public string Comment { get; set; }
        public int RatingValue { get; set; }

        public string UserId { get; set; }
        public UserProfile User { get; set; }
    }
}
