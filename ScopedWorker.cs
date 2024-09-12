namespace TransientWorkerStudies
{
    internal class ScopedWorker : BackgroundService
    {
        private IServiceScopeFactory _scopeFactory;

        public ScopedWorker(IServiceScopeFactory scopeFactory)
        {
            _scopeFactory = scopeFactory;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {

                await Task.Delay(1000, stoppingToken);

                using (var s = _scopeFactory.CreateScope())
                {
                    Console.WriteLine("\r\n(3) Scope: 0. Worker running at: " + DateTimeOffset.Now);
                    var ctx = s.ServiceProvider.GetRequiredService<IContext>();
                    ctx.Increase();
                }                    

                await Task.Delay(2000, stoppingToken);
            }
        }
    }
}
