namespace Banhcafe.Microservices.DigitalArchiveRequest.Core.Common;

public static class DatabaseSettingsInstances
{
    public static string SQL = "DatabaseSettings";
}

public sealed class DatabaseSettings
{
    public string SchemeName { get; set; }
    public string DatabaseName { get; set; }
    public string DatabaseApiUrl { get; set; }
}
