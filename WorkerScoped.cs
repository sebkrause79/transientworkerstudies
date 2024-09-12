namespace TransientWorkerStudies
{
    internal class WorkerScoped : BackgroundService
    {
        private IServiceScopeFactory _scopeFactory;
        private string _name;

        public WorkerScoped(IServiceScopeFactory scopeFactory, Workertype type)
        {
            _scopeFactory = scopeFactory;
            _name = type.Name;
    }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {

                await Task.Delay(1000, stoppingToken);

                using (var scope = _scopeFactory.CreateScope())
                {
                    Console.WriteLine($"\r\n{_name}: 0. Worker running at: {DateTimeOffset.Now}");
                    var ctx = scope.ServiceProvider.GetRequiredService<IContext>();
                    ctx.Increase();
                }                    

                await Task.Delay(2000, stoppingToken);
            }
        }
    }
}
