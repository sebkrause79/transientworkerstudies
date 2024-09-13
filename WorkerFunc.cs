namespace TransientWorkerStudies;

internal class WorkerFunc : BackgroundService
{
    private readonly Func<IContext> _contextFunc;
    private readonly Workertype _type;
    private int _gc;
    private int _runs = 0;

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
            Console.WriteLine($"\r\n{_type.Name}: 0. Run {++_runs}. Worker running at: {DateTimeOffset.Now}");

            new Helper().Work(_contextFunc);

            await Task.Delay(3000 - _type.WaitStart, stoppingToken);
            HandleGc();
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

    private void HandleGc()
    {
        _gc = ++_gc % 8;
        if (_gc == 0)
        {
            Console.WriteLine($"\r\nGarbage Collection, called by {_type.Name}");
            GC.Collect();
        }
    }
}