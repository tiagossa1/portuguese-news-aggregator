using Application.Constants;
using Application.Interfaces;
using Application.Models;
using Domain.Entities;
using Domain.Interfaces;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Quartz;

namespace Application.Jobs;

public class NewsReaderJob : IJob
{
    private readonly ILogger<NewsReaderJob> _logger;
    private readonly List<NewsWebsite> _newsWebsites;
    private readonly IJsonNewsReaderService _jsonNewsReaderService;
    private readonly IRssNewsReaderService _rssNewsReaderService;
    private readonly INewsRepository _newsRepository;

    public NewsReaderJob(ILogger<NewsReaderJob> logger,
        INewsRepository newsRepository,
        IJsonNewsReaderService jsonNewsReaderService,
        IRssNewsReaderService rssNewsReaderService,
        IOptions<NewsWebsites> newsWebsites)
    {
        _jsonNewsReaderService = jsonNewsReaderService;
        _rssNewsReaderService = rssNewsReaderService;
        _newsRepository = newsRepository;
        _newsWebsites = newsWebsites.Value?.Sources ?? new List<NewsWebsite>(0);
        _logger = logger;
    }

    public async Task Execute(IJobExecutionContext context)
    {
        try
        {
            var jsonNewsResult = await _jsonNewsReaderService.Read(_newsWebsites
                ?.Where(newsWebsite => NewsTypeConstants.Json.Equals(newsWebsite.Type, StringComparison.InvariantCultureIgnoreCase))
                .ToArray() ?? Array.Empty<NewsWebsite>());

            var rssNewsResult = _rssNewsReaderService.Read(_newsWebsites
                ?.Where(newsWebsite => NewsTypeConstants.Rss.Equals(newsWebsite.Type, StringComparison.InvariantCultureIgnoreCase))
                .ToArray() ?? Array.Empty<NewsWebsite>());

            var allNewsFromSources = jsonNewsResult
                .Concat(rssNewsResult)
                .Select(newsArticle => new News
                {
                    Source = newsArticle.Source,
                    Title = newsArticle.Title,
                    CreatedAt = newsArticle.CreatedAt,
                    ImageUrl = newsArticle.ImageUrl,
                    PublishDate = newsArticle.PublishDate,
                    SourceId = newsArticle.SourceId,
                    NewsCategories = newsArticle.NewsCategories
                        ?.Select(newsCategory => new Domain.Entities.NewsCategory
                        {
                            Code = newsCategory.Code,
                            Name = newsCategory.Name
                        })
                        .ToList() ?? new List<Domain.Entities.NewsCategory>(0)
                })
                .ToList();

            _newsRepository.CreateRangeIfNotExists(allNewsFromSources);
        }
        catch (Exception e)
        {
            _logger.LogError("{exceptionMessage}", e.ToString());
            throw new JobExecutionException(e);
        }
    }
}