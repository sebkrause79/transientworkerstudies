namespace TransientWorkerStudies
{
    public class Program
    {
        public static void Main(string[] args)
        {
            IHost funcHost = Host.CreateDefaultBuilder(args)
                .ConfigureServices(services =>
                {
                    services.AddSingleton(new Workertype 
                    { 
                        Id = 1,
                        WorkerPrefix = "Function",
                        WaitStart = 500,
                    });
                    services.AddHostedService<WorkerFunc>();

                    services.AddTransient<IContext, Context>();
                    services.AddSingleton<Func<IContext>>(srv => () => srv.GetService<IContext>());
                })
                .Build();
            funcHost.RunAsync();

            IHost factoryHost = Host.CreateDefaultBuilder(args)
                .ConfigureServices(services =>
                {
                    services.AddSingleton(new Workertype 
                    { 
                        Id = 2,
                        WorkerPrefix = "Factory",
                        WaitStart = 1500,
                    });
                    services.AddHostedService<WorkerFactory>();

                    services.AddDbContextFactory<Context>();
                })
                .Build();
            factoryHost.RunAsync();

            IHost scopedHost = Host.CreateDefaultBuilder(args)
                .ConfigureServices(services =>
                {
                    services.AddSingleton(new Workertype 
                    { 
                        Id = 3,
                        WorkerPrefix = "Scope",
                        WaitStart = 2500,
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
        public int WaitStart { get; set; }
    }
}