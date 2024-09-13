namespace TransientWorkerStudies
{
    internal class WorkerScoped : BackgroundService
    {
        private IServiceScopeFactory _scopeFactory;
        private Workertype _type;

        public WorkerScoped(IServiceScopeFactory scopeFactory, Workertype type)
        {
            _scopeFactory = scopeFactory;
            _type = type;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                await Task.Delay(_type.WaitStart, stoppingToken);
                Console.WriteLine($"\r\n{_type.Name}: 0. Worker running at: {DateTimeOffset.Now}");

                using var scope = _scopeFactory.CreateScope();
                var ctx = scope.ServiceProvider.GetRequiredService<IContext>();
                ctx.Increase();

                await Task.Delay(3000 - _type.WaitStart, stoppingToken);
            }
        }
    }
}
