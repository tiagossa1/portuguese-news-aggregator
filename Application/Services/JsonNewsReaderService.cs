using Application.Interfaces;
using Application.Models;
using Microsoft.Extensions.Logging;

namespace Application.Services;

public class JsonNewsReaderService : IJsonNewsReaderService
{
    private readonly ILogger<JsonNewsReaderService> _logger;
    private readonly IEnumerable<IJsonNewsReader> _jsonNewsReaders;

    public JsonNewsReaderService(
        IEnumerable<IJsonNewsReader> jsonNewsReaders,
        ILogger<JsonNewsReaderService> logger)
    {
        _jsonNewsReaders = jsonNewsReaders;
        _logger = logger;
    }

    public async Task<IList<NewsArticle>> Read(IList<NewsWebsite> newsWebsites)
    {
        var newsArticles = new List<NewsArticle>();
        foreach (var newsWebsite in newsWebsites)
        {
            var reader = _jsonNewsReaders.FirstOrDefault(reader =>
                newsWebsite.Code.Equals(reader.Name, StringComparison.InvariantCultureIgnoreCase));

            if (reader is null)
            {
                _logger.LogWarning("{serviceName}.{methodName}: there is no reader implemented or registered for {newsWebsiteCode}", nameof(JsonNewsReaderService), nameof(Read), newsWebsite.Code);
                continue;
            }

            var readerResult = await reader.Read(newsWebsite);
            if (readerResult is not null ||
                readerResult.Count > 0)
            {
                newsArticles.AddRange(readerResult);
            }
        }

        return newsArticles;
    }
}