using MediatR;
using Microsoft.Extensions.Logging;
using Serilog.Context;

namespace Application;

public class LoggingBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse> where TRequest : IRequest<TRequest>
{
    private readonly ILogger<LoggingBehavior<TRequest, TResponse>> logger;

    public LoggingBehavior(ILogger<LoggingBehavior<TRequest, TResponse>> logger)
    {
        this.logger = logger;
    }

    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
    {
        var requestName = typeof(TRequest).Name;

        try
        {
            TResponse result = await next();
            logger.LogInformation("Completed request {RequestName}", requestName);
            return result;
        }
        catch (Exception ex)
        {
            using (LogContext.PushProperty("Error", ex, true))
            {
                logger.LogError(
                    "Completed request {RequestName} with error ",
                    requestName
                );
            }
            throw;
        }
    }
}
