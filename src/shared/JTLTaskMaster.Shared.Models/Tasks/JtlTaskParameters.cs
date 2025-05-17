namespace JTLTaskMaster.Shared.Models.Tasks;

public class JtlTaskParameters
{
    public string TaskType { get; set; } = string.Empty;
    public VersanddatenImportParameters? VersanddatenParameters { get; set; }
    public WorkflowParameters? WorkflowParameters { get; set; }
}

public class VersanddatenImportParameters
{
    public string Lieferscheinnummer { get; set; } = string.Empty;
    public string Trackingcode { get; set; } = string.Empty;
    public DateTime Versanddatum { get; set; }
    public string Versanddienstleister { get; set; } = string.Empty;
}

public class WorkflowParameters
{
    public string WorkflowId { get; set; } = string.Empty;
    public string Parameters { get; set; } = string.Empty;
}