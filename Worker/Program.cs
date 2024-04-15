using Worker;
using Infrastructure;

Host
    .CreateDefaultBuilder()
    .ConfigureServices((hostContext, services) =>
    {
        services.AddProjectDependencies(hostContext.Configuration);
        services.AddHostedService<JobWorker>();
    })
    .Build()
    .Run();