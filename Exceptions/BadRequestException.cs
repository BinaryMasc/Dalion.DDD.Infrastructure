using Microsoft.AspNetCore.Http;

namespace Dalion.DDD.Infrastructure.Exceptions
{
    [Serializable]
    public sealed class BadRequestException : ClientErrorException
    {
        /// <summary>
        /// Create 400 BadRequestException
        /// </summary>
        /// <param name="message"></param>
        /// <param name="details"></param>
        public BadRequestException(string details = "") : base(StatusCodes.Status400BadRequest, "Bad Request",
            details)
        {
        }

        /// <summary>
        /// Create 400 BadRequestException
        /// </summary>
        /// <param name="code"></param>
        /// <param name="message"></param>
        /// <param name="details"></param>       
        public BadRequestException(string code, string message, string details = "") : base(StatusCodes.Status400BadRequest, message, details, code)
        {
        }
    }
}
