using Microsoft.Extensions.DependencyInjection;
using Quartz;
using Quartz.Spi;

namespace Application.Jobs;

public class NewsReaderJobFactory : IJobFactory
{
    private readonly IServiceProvider _serviceProvider;

    public NewsReaderJobFactory(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    public IJob NewJob(
        TriggerFiredBundle bundle,
        IScheduler scheduler)
    {
        return _serviceProvider.GetService<NewsReaderJob>()!;
    }

    public void ReturnJob(IJob job)
    {
        var disposable = job as IDisposable;
        disposable?.Dispose();
    }
}