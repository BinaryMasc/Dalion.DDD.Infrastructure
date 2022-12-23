using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dalion.DDD.Infrastructure.Exceptions
{
    [Serializable]
    public sealed class InternalServerErrorException : ClientErrorException
    {
        /// <summary>
        /// Create 400 BadRequestException
        /// </summary>
        /// <param name="message"></param>
        /// <param name="details"></param>
        public InternalServerErrorException(string details = "") : base(StatusCodes.Status500InternalServerError, "InternalServerError",
            details)
        {
        }

        /// <summary>
        /// Create 400 BadRequestException
        /// </summary>
        /// <param name="code"></param>
        /// <param name="message"></param>
        /// <param name="details"></param>       
        public InternalServerErrorException(string code, string message, string details = "") : base(StatusCodes.Status500InternalServerError, message, details, code)
        {
        }
    }
}
