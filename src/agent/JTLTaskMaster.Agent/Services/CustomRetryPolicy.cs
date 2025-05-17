using Microsoft.AspNetCore.SignalR.Client;

namespace JTLTaskMaster.Agent.Services;

public class CustomRetryPolicy : IRetryPolicy
{
    private readonly Random _random = new();

    public TimeSpan? NextRetryDelay(RetryContext retryContext)
    {
        // No retry if we've been retrying for more than 30 minutes
        if (retryContext.ElapsedTime.TotalMinutes > 30)
        {
            return null;
        }

        // Calculate retry interval with exponential backoff and jitter
        var secondsToWait = Math.Min(Math.Pow(2, retryContext.PreviousRetryCount), 300);
        secondsToWait = Math.Max(1, secondsToWait); // At least 1 second

        // Add some randomness (jitter) to avoid reconnection storms
        secondsToWait += _random.NextDouble() * 3;

        return TimeSpan.FromSeconds(secondsToWait);
    }
}