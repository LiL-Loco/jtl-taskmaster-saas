using JTLTaskMaster.Shared.Models;

namespace JTLTaskMaster.Agent.Services.Jtl;

public interface IJtlService
{
    Task ImportVersanddaten(VersanddatenImportParameters parameters);
    Task ExecuteWorkflow(string workflowId, string parameters);
}