using Microsoft.EntityFrameworkCore;

namespace TransientWorkerStudies
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
                Console.WriteLine("\r\n(1) Factory: 0. Worker running at: " + DateTimeOffset.Now);
                new Helper().Work(_contextFactory);

                await Task.Delay(3000, stoppingToken);
            }
        }

        class Helper
        {
            public void Work(IDbContextFactory<Context> factory)
            {
                var ctx = factory.CreateDbContext();
                ctx.Increase();
            }
        }
    }

    
}
