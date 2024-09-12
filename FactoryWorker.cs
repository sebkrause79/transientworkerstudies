using Microsoft.EntityFrameworkCore;

namespace Test04_Worker
{
    public class FactoryWorker : BackgroundService
    {
        private IDbContextFactory<Context> _contextFactory;

        public FactoryWorker(IDbContextFactory<Context> factory)
        {
            _contextFactory = factory;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                Console.WriteLine("Factory: 0. Worker running at: " + DateTimeOffset.Now);
                var ctx = _contextFactory.CreateDbContext();
                ctx.Increase();

                await Task.Delay(3000, stoppingToken);
            }
        }
    }
}
