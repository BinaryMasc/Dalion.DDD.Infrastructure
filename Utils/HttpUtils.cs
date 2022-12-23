using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentValidation.Results;
using Microsoft.AspNetCore.Mvc;

namespace Dalion.DDD.Infrastructure.Utils
{
    public static class HttpUtils
    {
        public static class BuildDto
        {
            public static ResponseDto<T> ReturnData<T>(T data)
            {
                return new ResponseDto<T>
                {
                    Data = data,
                    IsSuccess = true,
                    Message = "",
                    Errors = null
                };
            }

            public static ResponseDto<object> ReturnValidationError(IEnumerable<ValidationFailure> errors)
            {
                return new ResponseDto<object>
                {
                    Data = null,
                    IsSuccess = false,
                    Message = errors.First()?.ErrorMessage ?? "Validation Error",
                    Errors = errors
                };
            }

            public static ResponseDto<object> ReturnInternalError(Exception ex)
            {
                return new ResponseDto<object>
                {
                    Data = null,
                    IsSuccess = false,
                    Message = ex.Message,
                    Errors = { }
                };
            }
        }

        public static OkObjectResult ReturnData<T>(T data)
        {
            return new OkObjectResult(BuildDto.ReturnData(data));
        }

        public static ConflictObjectResult ReturnValidationError<T>(T data)
        {
            return new ConflictObjectResult(BuildDto.ReturnData(data));
        }

        public static StatusCodeResult ReturnInternalServerError()
        {
            return new StatusCodeResult(500);
        }


    }
}
