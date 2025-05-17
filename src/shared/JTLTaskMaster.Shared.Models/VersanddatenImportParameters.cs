namespace JTLTaskMaster.Shared.Models;

public class VersanddatenImportParameters
{
    public string Lieferscheinnummer { get; set; } = string.Empty;
    public string Trackingcode { get; set; } = string.Empty;
    public DateTime Versanddatum { get; set; }
    public string Versanddienstleister { get; set; } = string.Empty;
}