// See https://aka.ms/new-console-template for more information

public class AppConfig
{
    public string AppName { get; set; }
    public string AppVersion { get; set; }

    public override string ToString()
    {
        return $"{AppName} v{AppVersion}";
    }
}