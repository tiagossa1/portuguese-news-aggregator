using System.Reflection;
using Application.Interfaces;
using Application.Jobs;
using Application.Models;
using Application.Readers;
using Application.Services;
using Database;
using Database.Repositories;
using Domain.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Quartz;

namespace Infrastructure;

public static class DependencyContainer
{
    public static IServiceCollection AddProjectDependencies(
        this IServiceCollection serviceCollection,
        IConfiguration config)
    {
        return serviceCollection
            .AddLogging(loggingBuilder =>
            {
                loggingBuilder.AddConsole();
                loggingBuilder.SetMinimumLevel(LogLevel.Information);
            })
            .AddScoped<IJsonNewsReader, ObservadorJsonNewsReader>()
            .AddScoped<IJsonNewsReader, PublicoJsonNewsReader>()
            .AddScoped<IRssNewsReader, NoticiasAoMinutoNewsReader>()
            .AddScoped<IJsonNewsReaderService, JsonNewsReaderService>()
            .AddScoped<IRssNewsReaderService, RssNewsReaderService>()
            .AddSingleton<INewsRepository, NewsRepository>(services =>
            {
                var path = Path.Combine(Path.GetDirectoryName(Assembly.GetEntryAssembly().Location), "newsDatabase.db");
                return new NewsRepository(services.GetService<ILogger<NewsRepository>>(), path);
            })
            .Configure<QuartzOptions>(config.GetSection("Quartz"))
            .AddScoped<NewsReaderJob>()
            .Configure<NewsWebsites>(options => config
                .GetSection("NewsWebsites")
                .Bind(options))
            .AddHttpClient();
    }
}