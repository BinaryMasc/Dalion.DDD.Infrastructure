
namespace Dalion.DDD.Infrastructure.Exceptions
{
    public abstract class ClientErrorException : System.Exception
    {
        public string Details { get; }
        public int StatusCode { get; }
        public string Code { get; }

        protected ClientErrorException(int statusCode)
        {
            StatusCode = statusCode;
        }

        /// <summary>
        /// Exception for 4XX Errors
        /// </summary>
        /// <param name="statusCode"></param>
        /// <param name="message"></param>
        /// <param name="details"></param>
        protected ClientErrorException(int statusCode, string message, string details = "") : base(message)
        {
            StatusCode = statusCode;
            Details = details;
        }

        protected ClientErrorException(int status, string message, string details = "", string code = "") : base(message)
        {
            StatusCode = status;
            Details = details;
            Code = code;
        }
    }
}
