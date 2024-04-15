using Application.Interfaces;
using Application.Models;
using Microsoft.Extensions.Logging;

namespace Application.Services;

public class RssNewsReaderService : IRssNewsReaderService
{
    private readonly ILogger<JsonNewsReaderService> _logger;
    private readonly IEnumerable<IRssNewsReader> _rssNewsReaders;

    public RssNewsReaderService(
        ILogger<JsonNewsReaderService> logger,
        IEnumerable<IRssNewsReader> rssNewsReaders)
    {
        _logger = logger;
        _rssNewsReaders = rssNewsReaders;
    }

    public IList<NewsArticle> Read(IList<NewsWebsite> newsWebsites)
    {
        var newsArticles = new List<NewsArticle>();
        foreach (var newsWebsite in newsWebsites)
        {
            var reader = _rssNewsReaders.FirstOrDefault(reader =>
                newsWebsite.Code.Equals(reader.Name, StringComparison.InvariantCultureIgnoreCase));

            if (reader is null)
            {
                _logger.LogWarning("{serviceName}.{methodName}: there is no reader implemented or registered for {newsWebsiteCode}", nameof(RssNewsReaderService), nameof(Read), newsWebsite.Code);
                continue;
            }

            var readerResult = reader.Read(newsWebsite);
            if (readerResult is not null ||
                readerResult.Count > 0)
            {
                newsArticles.AddRange(readerResult);
            }
        }

        return newsArticles;
    }
}