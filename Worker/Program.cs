using Worker;
using Infrastructure;

Host
    .CreateDefaultBuilder()
    .UseSystemd()
    .ConfigureServices((hostContext, services) =>
    {
        services.AddProjectDependencies(hostContext.Configuration);
        services.AddHostedService<JobWorker>();
    })
    .Build()
    .Run();