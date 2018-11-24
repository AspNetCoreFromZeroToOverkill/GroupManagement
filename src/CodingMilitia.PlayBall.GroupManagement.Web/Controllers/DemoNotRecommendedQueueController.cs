using System.Collections.Concurrent;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace CodingMilitia.PlayBall.GroupManagement.Web.Controllers
{
    [Route("queue")]
    public class DemoNotRecommendedQueueController : Controller
    {
        private static readonly ConcurrentQueue<TaskCompletionSource<int>> TaskCompletionSourceQueue 
            = new ConcurrentQueue<TaskCompletionSource<int>>();

        private static readonly ConcurrentQueue<CancellationTokenSource> CancellationTokenSourceQueue =
            new ConcurrentQueue<CancellationTokenSource>();


        [Route("ask")]
        public async Task<IActionResult> AskAsync()
        {
            var tcs = new TaskCompletionSource<int>(TaskCreationOptions.RunContinuationsAsynchronously);
            TaskCompletionSourceQueue.Enqueue(tcs);
            var result = await tcs.Task;
            return Content(result.ToString());
        }

        [Route("tell/{value}")]
        public IActionResult Tell(int value)
        {
            if (TaskCompletionSourceQueue.TryDequeue(out var tcs))
            {
                if (!tcs.TrySetResult(value))
                {
                    return StatusCode(500);        
                }
                
                return NoContent();
            }

            return NotFound();
        }
        
        [Route("cancel")]
        public IActionResult Cancel()
        {
            if (TaskCompletionSourceQueue.TryDequeue(out var tcs))
            {
                if (!tcs.TrySetCanceled())
                {
                    return StatusCode(500);        
                }
                
                return NoContent();
            }

            return NotFound();
        }

        [Route("delay/{value}")]
        public async Task<IActionResult> DelayAsync(int value)
        {
            using (var cts = new CancellationTokenSource())
            {
                CancellationTokenSourceQueue.Enqueue(cts);
                await Task.Delay(value, cts.Token);
                CancellationTokenSourceQueue.TryDequeue(out _);
                return Content("Done waiting");
            }
        }

        [Route("delay/cancel")]
        public IActionResult CancelDelay()
        {
            if (CancellationTokenSourceQueue.TryDequeue(out var cts))
            {
                cts.Cancel();
                return Content("Delay cancelled!");
            }

            return NotFound();
        }
    }
}