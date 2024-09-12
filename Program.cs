using Microsoft.EntityFrameworkCore;

namespace Test04_Worker
{
    public class Program
    {
        public static void Main(string[] args)
        {
            IHost factoryHost = Host.CreateDefaultBuilder(args)
                .ConfigureServices(services =>
                {
                    services.AddSingleton(new Workertype { WorkerPrefix = "Factory", Id = 1 });
                    services.AddHostedService<FactoryWorker>();
                    services.AddDbContextFactory<Context>();
                })
                .Build();
            factoryHost.RunAsync();

            IHost funcHost = Host.CreateDefaultBuilder(args)
                .ConfigureServices(services =>
                {
                    services.AddSingleton(new Workertype { WorkerPrefix = "Function", Id = 2 });
                    services.AddHostedService<FuncWorker>();
                    services.AddTransient<IContext, Context>();
                    services.AddSingleton<Func<IContext>>(srv => () => srv.GetService<IContext>());
                })
                .Build();
            funcHost.RunAsync();

            IHost scopedHost = Host.CreateDefaultBuilder(args)
                .ConfigureServices(services =>
                {
                    services.AddSingleton(new Workertype { WorkerPrefix = "Scope", Id = 3 });
                    services.AddHostedService<ScopedWorker>();
                    services.AddScoped<IContext, Context>();
                })
                .Build();
            scopedHost.Run();
        }
    }

    public class Workertype
    {
        public string WorkerPrefix { get; set; }
        public int Id { get; set; }
    }
}