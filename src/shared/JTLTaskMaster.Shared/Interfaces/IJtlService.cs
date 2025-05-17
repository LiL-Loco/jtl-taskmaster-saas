using JTLTaskMaster.Shared.Models;

namespace JTLTaskMaster.Shared.Interfaces;

public interface IJtlService
{
    Task ImportVersanddaten(VersanddatenImportParameters parameters);
    Task ExecuteWorkflow(string workflowId, string parameters);
}