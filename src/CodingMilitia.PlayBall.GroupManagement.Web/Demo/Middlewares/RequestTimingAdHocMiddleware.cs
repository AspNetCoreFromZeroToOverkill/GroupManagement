using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace CodingMilitia.PlayBall.GroupManagement.Web.Demo.Middlewares
{
    public class RequestTimingAdHocMiddleware
    {
        private readonly RequestDelegate _next;
        private int _requestCounter; 

        public RequestTimingAdHocMiddleware(RequestDelegate next)
        {
            _next = next;
        }
        
        public async Task InvokeAsync(HttpContext context, ILogger<RequestTimingAdHocMiddleware> logger)
        {
            var watch = Stopwatch.StartNew();
            await _next(context);
            watch.Stop();
            Interlocked.Increment(ref _requestCounter);
            logger.LogWarning("Request {requestNumber} took {requestTime}ms", _requestCounter, watch.ElapsedMilliseconds);
        }
    }
}