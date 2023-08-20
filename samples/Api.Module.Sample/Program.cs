namespace Api.Module.Sample;

public static class Program
{
    public static void Main(string[] args)
    {
        // var modules = new object[]{
        //     new TenancyModule(),
        //     new TenantDetailsConfiguration(),
        //     new MenuModule(),
        //     new MenuItemConfiguration()
        // };
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
