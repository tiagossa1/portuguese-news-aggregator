using Application;
using Application.Jobs;
using Infrastructure;
using Quartz;
using Quartz.Logging;

namespace Worker;

public class JobWorker : BackgroundService
{
    private readonly IServiceProvider _serviceProvider;
    private readonly ILogger<JobWorker> _logger;

    public JobWorker(
        ILogger<JobWorker> logger, IServiceProvider serviceProvider)
    {
        _logger = logger;
        _serviceProvider = serviceProvider;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        // TODO: This has to be moved into DI.
        LogProvider.SetCurrentLogProvider(new ConsoleLogProvider());

        var job = JobBuilder.Create<NewsReaderJob>()
            .WithIdentity(nameof(NewsReaderJob), "JobGroup")
            .Build();

        var trigger = TriggerBuilder.Create()
            .WithIdentity($"{nameof(NewsReaderJob)}Trigger", "TriggerGroup")
            .StartNow()
            .WithSimpleSchedule(simpleScheduleBuilder => simpleScheduleBuilder
                .WithIntervalInHours(1))
            .Build();

        var schedulerFactory = SchedulerBuilder.Create().Build();
        var scheduler = await schedulerFactory.GetScheduler(stoppingToken);

        scheduler.JobFactory = new NewsReaderJobFactory(_serviceProvider);

        await scheduler.ScheduleJob(job, trigger, stoppingToken);
        await scheduler.Start(stoppingToken);
    }
}
