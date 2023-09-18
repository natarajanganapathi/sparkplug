namespace SparkPlug.Hosts;

public static class Program
{
    public static void Main(string[] args)
    {
        Run<Startup>(args);
    }
    public static void Run<TStartup>(string[] args) where TStartup : class
    {
        Host
            .CreateDefaultBuilder(args)
            .ConfigureLogging((context, builder) =>
            {
                builder.AddSimpleConsole(options =>
                {
                    options.SingleLine = true;
                    options.TimestampFormat = "hh:mm:ss ";
                });
            })
            .ConfigureWebHostDefaults(webBuilder =>
            {
                webBuilder.UseStartup<TStartup>();
            })
            .Build()
            .Run();
    }
}
