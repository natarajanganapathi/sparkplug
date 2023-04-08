namespace SparkPlug.Api.Configuration;

public class WebApiOptions
{
    public const string ConfigPath = "SparkPlug:Api";
    public WebApiOptions()
    {
        ApplicationName = string.Empty;
        CacheDuration = new SlidingExpiration();
    }

    [Required]
    public string ApplicationName { get; set; }
    public bool IsMultiTenant { get; set; }
    public SlidingExpiration CacheDuration { get; set; }
}

public class SlidingExpiration
{
    public SlidingExpiration()
    {
        TenantCacheInfo = 10;
    }
    public int TenantCacheInfo { get; set; }
}
