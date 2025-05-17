using System;
using System.Threading.Tasks;

namespace JTLTaskMaster.Application.Common.Interfaces;

public interface IJobProcessor
{
    Task EnqueueJob(Guid jobId);
}