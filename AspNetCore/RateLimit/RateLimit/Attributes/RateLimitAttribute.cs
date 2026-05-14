using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Collections.Concurrent;

namespace RateLimit.Attributes;

public class RateLimitAttribute(int maxConcurrentRequests) : ActionFilterAttribute
{
    private static readonly ConcurrentDictionary<string, SemaphoreSlim> _semaphores = new();

    public override async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
    {
        var key = $"{context.Controller.GetType().Name}_{context.ActionDescriptor.DisplayName}";
        var semaphore = _semaphores.GetOrAdd(key, _ => new SemaphoreSlim(maxConcurrentRequests));

        if (!semaphore.Wait(0))
        {
            context.Result = new StatusCodeResult(429);

            return;
        }

        try
        {
            await next();
        }
        finally
        {
            semaphore.Release();
        }
    }
}
