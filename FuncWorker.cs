
using Microsoft.EntityFrameworkCore.Internal;

namespace Test04_Worker
{
    internal class FuncWorker : BackgroundService
    {
        private Func<IContext> _contextFunc;

        public FuncWorker(Func<IContext> contextFunc)
        {
            _contextFunc = contextFunc;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                await Task.Delay(2000, stoppingToken);

                Console.WriteLine("Function: 0. Worker running at: " + DateTimeOffset.Now);
                var ctx = _contextFunc();
                ctx.Increase();

                await Task.Delay(1000, stoppingToken);
            }
        }
    }
}
