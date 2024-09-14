namespace TransientWorkerStudies;

using System;

public class WorkerFactory : BackgroundService
{
    private readonly Func<IContext> _contextFactory;
    private readonly Workertype _type;
    private int _gc;
    private int _runs = 0;

    public WorkerFactory(Func<IContext> factory, Workertype type)
    {
        _contextFactory = factory;
        _type = type;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            await Task.Delay(_type.WaitStart, stoppingToken);
            Console.WriteLine($"\r\n{_type.Name}: 0. Run {++_runs}. Worker running at: {DateTimeOffset.Now}");

            new Helper().Work(_contextFactory);

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
        _gc = ++_gc % 9;
        if (_gc == 0)
        {
            Console.WriteLine($"\r\nGarbage Collection, called by {_type.Name}");
            GC.Collect();
        }
    }
}