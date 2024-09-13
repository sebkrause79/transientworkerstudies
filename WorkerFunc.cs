namespace TransientWorkerStudies
{
    internal class WorkerFunc : BackgroundService
    {
        private Func<IContext> _contextFunc;
        private Workertype _type;

        public WorkerFunc(Func<IContext> contextFunc, Workertype type)
        {
            _contextFunc = contextFunc;
            _type = type;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                await Task.Delay(_type.WaitStart, stoppingToken);
                Console.WriteLine($"\r\n{_type.Name}: 0. Worker running at: {DateTimeOffset.Now}");

                new Helper().Work(_contextFunc);

                await Task.Delay(3000 - _type.WaitStart, stoppingToken);
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
