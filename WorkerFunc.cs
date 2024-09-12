namespace TransientWorkerStudies
{
    internal class WorkerFunc : BackgroundService
    {
        private Func<IContext> _contextFunc;
        private string _name;

        public WorkerFunc(Func<IContext> contextFunc, Workertype type)
        {
            _contextFunc = contextFunc;
            _name = type.Name;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                await Task.Delay(2000, stoppingToken);

                Console.WriteLine($"\r\n{_name}: 0. Worker running at: {DateTimeOffset.Now}");
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
