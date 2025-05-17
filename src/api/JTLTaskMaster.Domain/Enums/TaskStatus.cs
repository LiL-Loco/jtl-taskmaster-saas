
using JTLTaskMaster.Domain.Common;   
// Add the following using if JobStatus is defined elsewhere, otherwise define the enum below
// using JTLTaskMaster.Domain.Enums;
using JTLTaskMaster.Domain.Entities;
namespace JTLTaskMaster.Domain.Enums;


public enum TaskStatus
{
    Pending,
    Running,
    Completed,
    Failed
}