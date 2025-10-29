using Assecor.Backend.Domain.Maybe;
using Microsoft.Extensions.Logging;

namespace Assecor.Backend.Domain.Extensions
{
    public static class LoggerExtensions
    {
        public static void LogMaybeResult<T>(this ILogger logger, Maybe<T> maybe, object key)
        {
            if (maybe.HasValue)
            {
                return;
            }
            
            logger.LogInformation("No entity of type '{type}' with key '{Key}' found!", maybe.GetTypeName(), key);
        }
    }
}
