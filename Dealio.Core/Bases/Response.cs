using System.Net;
using System.Text.Json.Serialization;


namespace Dealio.Core.Bases
{
    public class Response<T> where T : class
    {
        public HttpStatusCode StatusCode { get; set; }
        public string Message { get; set; }

        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public T Data { get; set; }


        public static Response<T> Success(T Data, string message = "success")
        {
            return new Response<T>
            {
                StatusCode = HttpStatusCode.OK,
                Message = message,
                Data = Data
            };
        }
        public static Response<T> Created(T Data, string message = "created")
        {
            return new Response<T>
            {
                StatusCode = HttpStatusCode.Created,
                Message = message,
                Data = Data
            };
        }
        public static Response<T> NotFound(string message = "not found")
        {
            return new Response<T>
            {
                StatusCode = HttpStatusCode.NotFound,
                Message = message,
                Data = null!
            };
        }
        public static Response<T> BadRequest(string message = "bad request")
        {
            return new Response<T>
            {
                StatusCode = HttpStatusCode.BadRequest,
                Message = message,
                Data = null!
            };
        }
        public static Response<T> Unauthorized(string message = "unauthorized")
        {
            return new Response<T>
            {
                StatusCode = HttpStatusCode.Unauthorized,
                Message = message,
                Data = null!
            };
        }
        public static Response<T> Forbidden(string message = "forbidden")
        {
            return new Response<T>
            {
                StatusCode = HttpStatusCode.Forbidden,
                Message = message,
                Data = null!
            };
        }
    }
}
