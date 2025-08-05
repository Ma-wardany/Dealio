using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Dealio.Services.Helpers
{
    public class ServiceResult<T>
    {
        public T Data { get; set; }
        public ServiceResultEnum ResultEnum { get; set; }

        public static ServiceResult<T> Success(T data, ServiceResultEnum status)
        {
            return new ServiceResult<T> 
            { 
                Data = data,
                ResultEnum = status
            };
        }

        public static ServiceResult<T> Failure(ServiceResultEnum status)
        {
            return new ServiceResult<T>
            {
                ResultEnum = status
            };
        }
    }
}
