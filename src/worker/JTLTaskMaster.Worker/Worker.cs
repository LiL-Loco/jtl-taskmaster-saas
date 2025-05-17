using Microsoft.Extensions.Hosting;
using System.Threading;
using System.Threading.Tasks;

namespace JTLTaskMaster.Worker
{
    public class Worker : BackgroundService
    {
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                // Job processing logic here
                await Task.Delay(1000, stoppingToken);
            }
        }
    }
}