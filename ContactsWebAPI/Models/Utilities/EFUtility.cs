namespace ContactsWebAPI.Models.Utilities;

public class EFUtility
{
    public bool EnableSensitiveDataLogging { get; set; }
    public bool EnableDetailedErrors { get; set; }
    public bool EnableLogToConsole { get; set; }
    public int MaxReTryCount { get; set; }
    public int MaxReTryDelay { get; set; }
    public int CommandTimeout { get; set; }
}
