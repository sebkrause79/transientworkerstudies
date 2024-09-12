namespace TransientWorkerStudies
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

                Console.WriteLine("\r\n(2) Function: 0. Worker running at: " + DateTimeOffset.Now);
                new Helper().Work(_contextFunc);

                await Task.Delay(1000, stoppingToken);
            }
        }

        class Helper
        {
            public void Work(Func<IContext> factory)
            {
                var ctx = factory();
                ctx.Increase();
            }
        }
    }
}
