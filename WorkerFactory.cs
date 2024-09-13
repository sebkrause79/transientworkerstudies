using Microsoft.EntityFrameworkCore;

namespace TransientWorkerStudies
{
    public class WorkerFactory : BackgroundService
    {
        private IDbContextFactory<Context> _contextFactory;
        private Workertype _type;

        public WorkerFactory(IDbContextFactory<Context> factory, Workertype type)
        {
            _contextFactory = factory;
            _type = type;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                await Task.Delay(_type.WaitStart, stoppingToken);
                Console.WriteLine($"\r\n{_type.Name}: 0. Worker running at: {DateTimeOffset.Now}");

                new Helper().Work(_contextFactory);

                await Task.Delay(3000 - _type.WaitStart, stoppingToken);
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
