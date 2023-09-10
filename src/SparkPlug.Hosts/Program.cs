namespace SparkPlug.Hosts;

public static class Program
{
    public static void Main(string[] args)
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
            webBuilder.UseStartup<Startup>();
        })
        .Build()
        .Run();
    }
}
