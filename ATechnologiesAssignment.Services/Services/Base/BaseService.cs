using ATechnologiesAssignment.App.Models;
using System.Net;

namespace ATechnologiesAssignment.Services.Services.Base
{
    public abstract class BaseService
    {
        private Dictionary<string, string[]> ParseErrors(IDictionary<string, string> errors)
        {
            return errors.Select(x => new KeyValuePair<string, string[]>(x.Key, new[] { x.Value })).ToDictionary(x => x.Key, x => x.Value);
        }

        protected virtual BaseResponse ValidationError(Dictionary<string, string> errors)
        {
            return new BaseResponse
            {
                StatusCode = HttpStatusCode.UnprocessableEntity,
                Message = "One or more validation errors occurred.",
                Errors = ParseErrors(errors)
            };
        }

        protected virtual BaseResponse Success(string message)
        {
            return new BaseResponse
            {
                StatusCode = HttpStatusCode.OK,
                Message = message
            };
        }

        protected virtual BaseResponse Error(string message, HttpStatusCode statusCode = HttpStatusCode.BadRequest, IDictionary<string, string>? errors = null)
        {
            return new BaseResponse
            {
                StatusCode = statusCode,
                Message = message,
                Errors = errors == null ? null : ParseErrors(errors)
            };
        }



        protected virtual DataResponse<TData> Success<TData>(TData data, string message = "Request was successful.")
        {
            return new DataResponse<TData>
            {
                StatusCode = HttpStatusCode.OK,
                Message = message,
                Data = data
            };
        }

        protected virtual DataResponse<TData> Error<TData>(string message, HttpStatusCode statusCode = HttpStatusCode.BadRequest, IDictionary<string, string>? errors = null)
        {
            return new DataResponse<TData>
            {
                StatusCode = statusCode,
                Message = message,
                Errors = errors == null ? null : ParseErrors(errors)
            };
        }
    }
}