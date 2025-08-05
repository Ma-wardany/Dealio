using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dealio.Domain.Results
{
    public class AuthenticationResult
    {
        public string UserId { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public string Role { get; set; }
        public bool IsConfirmed { get; set; }
        public string AccessToken { get; set; }
    }
}
