namespace SparkPlug.Api.Configuration;

public class WebApiOptions
{
    public const string ConfigPath = "SparkPlug:Api";
    public WebApiOptions()
    {
        ApplicationName = string.Empty;
        PathBase = "api";
    }

    [Required]
    public string ApplicationName { get; set; }
    public string PathBase { get; set; }
    public bool IsMultiTenant { get => PathBase.Contains($"{{{WebApiConstants.Tenant}}}", StringComparison.CurrentCultureIgnoreCase); }
}
