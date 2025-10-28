using System.ComponentModel.DataAnnotations;
using Assecor.Backend.Domain.Exceptions;
using Newtonsoft.Json;

namespace Assecor.Backend.Api
{
    public class ExceptionHandler(RequestDelegate next, ILogger<ExceptionHandler> logger)
    {
        public async Task Invoke(HttpContext context)
        {
            try
            {
                await next(context);
            }
            catch (Exception ex)
            {
                await HandleExceptionAsync(context, ex);
            }
        }

        private async Task HandleExceptionAsync(HttpContext context, Exception ex)
        {
            var statusCode = StatusCodes.Status500InternalServerError;
            var title = "An unexpected error occurred.";

            switch (ex)
            {
                case CsvReaderException:
                    title = "Csv parsing failed.";
                    break;

                case KeyNotFoundException:
                    statusCode = StatusCodes.Status404NotFound;
                    title = "Resource not found.";
                    break;

                case FileNotFoundException:
                    title = "Could not find csv file";
                    break;
            }

            logger.LogError(ex, ex.Message);

            var problem = new
            {
                title,
                status = statusCode,
                detail = ex.Message,
                traceId = context.TraceIdentifier
            };

            context.Response.ContentType = "application/json";
            context.Response.StatusCode = statusCode;

            // Newtonsoft.Json SerializerSettings
            var jsonSettings = new JsonSerializerSettings
            {
                NullValueHandling = NullValueHandling.Ignore,
                Formatting = Formatting.Indented,
                ContractResolver = new Newtonsoft.Json.Serialization.CamelCasePropertyNamesContractResolver()
            };

            var json = JsonConvert.SerializeObject(problem, jsonSettings);
            await context.Response.WriteAsync(json);
        }
    }
}
