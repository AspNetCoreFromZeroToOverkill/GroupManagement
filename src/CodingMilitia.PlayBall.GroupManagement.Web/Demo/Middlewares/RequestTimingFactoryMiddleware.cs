using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace CodingMilitia.PlayBall.GroupManagement.Web.Demo.Middlewares
{
    public class RequestTimingFactoryMiddleware : IMiddleware
    {
        private readonly ILogger<RequestTimingFactoryMiddleware> _logger;
        private int _requestCounter; 

        public RequestTimingFactoryMiddleware(ILogger<RequestTimingFactoryMiddleware> logger)
        {
            _logger = logger;
        }
        
        public async Task InvokeAsync(HttpContext context, RequestDelegate next)
        {
            var watch = Stopwatch.StartNew();
            await next(context);
            watch.Stop();
            Interlocked.Increment(ref _requestCounter);
            _logger.LogTrace("Request {requestNumber} took {requestTime}ms", _requestCounter, watch.ElapsedMilliseconds);
        }
    }
}