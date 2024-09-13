namespace TransientWorkerStudies
{
    public class Program
    {
        public static void Main(string[] args)
        {
            IHost factoryHost = Host.CreateDefaultBuilder(args)
                .ConfigureServices(services =>
                {
                    services.AddSingleton(new Workertype 
                    { 
                        Id = 1,
                        WorkerPrefix = "Factory", 
                    });
                    services.AddHostedService<WorkerFactory>();

                    services.AddDbContextFactory<Context>();
                })
                .Build();
            factoryHost.RunAsync();

            IHost funcHost = Host.CreateDefaultBuilder(args)
                .ConfigureServices(services =>
                {
                    services.AddSingleton(new Workertype 
                    { 
                        Id = 2,
                        WorkerPrefix = "Function", 
                    });
                    services.AddHostedService<WorkerFunc>();

                    services.AddTransient<IContext, Context>();
                    services.AddSingleton<Func<IContext>>(srv => () => srv.GetService<IContext>());
                })
                .Build();
            funcHost.RunAsync();

            IHost scopedHost = Host.CreateDefaultBuilder(args)
                .ConfigureServices(services =>
                {
                    services.AddSingleton(new Workertype 
                    { 
                        Id = 3,
                        WorkerPrefix = "Scope", 
                    });
                    services.AddHostedService<WorkerScoped>();

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
        public string Name => $"({Id}) {WorkerPrefix}";
    }
}