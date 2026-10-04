using System.Collections.Concurrent;
using Microsoft.Extensions.DependencyInjection;
using Quartz;
using Quartz.Spi;

namespace Application.Jobs;

public class NewsReaderJobFactory : IJobFactory
{
    private readonly IServiceProvider _serviceProvider;
    private readonly ConcurrentDictionary<IJob, IServiceScope> _scopes = new();

    public NewsReaderJobFactory(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    public IJob NewJob(
        TriggerFiredBundle bundle,
        IScheduler scheduler)
    {
        var scope = _serviceProvider.CreateScope();
        var job = scope.ServiceProvider.GetRequiredService<NewsReaderJob>();
        _scopes[job] = scope;

        return job;
    }

    public void ReturnJob(IJob job)
    {
        if (_scopes.TryRemove(job, out var scope))
        {
            scope.Dispose();
        }
    }
}