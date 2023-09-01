namespace SparkPlug.Host;

public static class Program
{
    public static void Main(string[] args)
    {
        CreateHostBuilder(args)
        .ConfigureLogging((_, builder) => builder.AddSimpleConsole((options) =>
        {
            options.SingleLine = true;
            options.TimestampFormat = "hh:mm:ss ";
        }))
        .Build().Run();
    }

    public static IHostBuilder CreateHostBuilder(string[] args)
    {
        return Host.CreateDefaultBuilder(args)
            .ConfigureWebHostDefaults(webBuilder => _ = webBuilder.UseStartup<Startup>());
    }
}
