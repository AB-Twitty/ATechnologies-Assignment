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

        protected virtual BaseResponse NotFound(string message = "Resource not found.")
        {
            return new BaseResponse
            {
                StatusCode = HttpStatusCode.NotFound,
                Message = message
            };
        }

        protected virtual BaseResponse Created(string message = "Resource created.")
        {
            return new BaseResponse
            {
                StatusCode = HttpStatusCode.Created,
                Message = message
            };
        }

        protected virtual BaseResponse NoContent(string message = "No content.")
        {
            return new BaseResponse
            {
                StatusCode = HttpStatusCode.NoContent,
                Message = message
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

        protected virtual DataResponse<TData> Created<TData>(TData data, string message = "Resource created.")
        {
            return new DataResponse<TData>
            {
                StatusCode = HttpStatusCode.Created,
                Message = message,
                Data = data
            };
        }
    }
}