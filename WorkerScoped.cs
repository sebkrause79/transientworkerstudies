using System;
using System.Text;

namespace TransientWorkerStudies;

internal class WorkerScoped : BackgroundService
{
    private readonly IServiceScopeFactory _scopeFactory;
    private readonly Workertype _type;
    private int _gc;
    private int _runs = 0;

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
            Console.WriteLine($"\r\n{_type.Name}: 0. Run {++_runs}. Worker running at: {DateTimeOffset.Now}");

            using var scope = _scopeFactory.CreateScope();
            var ctx = scope.ServiceProvider.GetRequiredService<IContext>();
            ctx.Increase();

            await Task.Delay(3000 - _type.WaitStart, stoppingToken);
            HandleGc();
        }
    }

    private void HandleGc()
    {
        _gc = ++_gc % 10;
        if (_gc == 0)
        {
            Console.WriteLine($"\r\nGarbage Collection, called by {_type.Name}");
            GC.Collect();
        }
    }
}