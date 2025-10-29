using System.Net;
using Assecor.Backend.Api.Responses;
using Assecor.Backend.Domain;
using Microsoft.AspNetCore.Mvc;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Assecor.Backend.Api.Controllers
{
    public class RestServerControllerBase : ControllerBase
    {
        private readonly string _notFoundMessage = "Resource not found.";
        private readonly string _badRequestMessage = "Request was ill-formatted";
        private static TObjectResult RestServerReturn<TObjectResult>(object data, Func<object, TObjectResult> handler) => handler(data);
        
        private TObjectResult RestServerError<TObjectResult>(
            string title, 
            Func<object, TObjectResult> handler, 
            int statusCode,
            object? details = null, 
            string? traceId = null) => handler(new RestServerErrorResponse()
        {
            Title = title,
            Details = details,
            TraceId = traceId,
            StatusCode = statusCode
        });

        protected OkObjectResult RestServerOk(object data) => RestServerReturn(data, Ok);
        private NotFoundObjectResult RestServerNotFound(object? details = null) 
            => RestServerError(_notFoundMessage, NotFound, StatusCodes.Status404NotFound, details, HttpContext.TraceIdentifier);
        protected BadRequestObjectResult RestServerBadRequest(object? details = null) 
            => RestServerError(_badRequestMessage, BadRequest, StatusCodes.Status400BadRequest, details, HttpContext.TraceIdentifier);

        protected IActionResult MapToResult<T>(Maybe<T> maybe, string message) =>
            maybe.HasValue && maybe.Value != null
                ? RestServerOk(maybe.Value)
                : RestServerNotFound(message);
    }
}
