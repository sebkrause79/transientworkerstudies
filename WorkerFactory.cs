using Microsoft.EntityFrameworkCore;
using System.Xml.Linq;

namespace TransientWorkerStudies
{
    public class WorkerFactory : BackgroundService
    {
        private IDbContextFactory<Context> _contextFactory;
        private string _name;

        public WorkerFactory(IDbContextFactory<Context> factory, Workertype type)
        {
            _contextFactory = factory;
            _name = type.Name;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                Console.WriteLine($"\r\n{_name}: 0. Worker running at: {DateTimeOffset.Now}");
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
