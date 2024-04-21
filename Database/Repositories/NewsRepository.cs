using Domain.Entities;
using Domain.Interfaces;
using LiteDB;
using Microsoft.Extensions.Logging;

namespace Database.Repositories;

public class NewsRepository : INewsRepository
{
    private const string _newsCollectionName = "News";
    private readonly string _databasePath;

    private readonly ILogger<NewsRepository> _logger;

    public NewsRepository(
        ILogger<NewsRepository> logger,
        string databasePath)
    {
        _logger = logger;
        _databasePath = databasePath;
    }

    public void CreateRangeIfNotExists(IList<News> newsArticlesList)
    {
        try
        {
            using var db = new LiteDatabase(_databasePath);

            var col = db.GetCollection<News>(_newsCollectionName);

            var requestNewsArticleSourceIds = newsArticlesList
                .Select(newsArticle => newsArticle.SourceId)
                .ToList();

            var newsIdsToIgnore = col
                .Query()
                .Where(news => !requestNewsArticleSourceIds
                    .Any(newsArticleSourceId => newsArticleSourceId
                        .Equals(news.SourceId, StringComparison.InvariantCultureIgnoreCase)))
                .Select(news => news.SourceId)
                .ToList() ?? new List<string>(0);

            var newsToInsert = newsArticlesList
                .Where(newsArticle => !newsIdsToIgnore.Contains(newsArticle.SourceId, StringComparer.InvariantCultureIgnoreCase));

            col.InsertBulk(newsToInsert);
            col.EnsureIndex(news => news.Id);
        }
        catch (Exception e)
        {
            _logger.LogError("{newsRepository}.{createRange}: there was an error upserting the document: {exception}", nameof(NewsCategory), nameof(CreateRangeIfNotExists), e.ToString());
        }
    }

    public IList<News> GetAll()
    {
        try
        {
            using var db = new LiteDatabase(_databasePath);
            var col = db.GetCollection<News>(_newsCollectionName);

            return col
                .FindAll()
                .ToList();
        }
        catch (Exception e)
        {
            _logger.LogError("{newsRepository}.{createRange}: there was an error getting all news: {exception}", nameof(NewsCategory), nameof(GetAll), e.ToString());

            return Array.Empty<News>();
        }
    }
}