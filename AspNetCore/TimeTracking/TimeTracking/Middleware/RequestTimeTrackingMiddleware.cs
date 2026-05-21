using System.Diagnostics;

namespace TimeTracking.Middleware;

public class RequestTimeTrackingMiddleware(RequestDelegate next, ILogger<RequestTimeTrackingMiddleware> logger)
{
    public async Task InvokeAsync(HttpContext context)
    {
        var stopwatch = Stopwatch.StartNew();
        var requestTime = DateTime.UtcNow;

        logger.LogInformation("Request started at {RequestTime}: {Method} {Path}",
            requestTime, context.Request.Method, context.Request.Path);

        await next(context);

        stopwatch.Stop();
        var elapsedMs = stopwatch.ElapsedMilliseconds;

        logger.LogInformation("Request finished at {FinishTime} - Elapsed {ElapsedMs}ms - Status {StatusCode}",
            DateTime.UtcNow, elapsedMs, context.Response.StatusCode);
    }
}
