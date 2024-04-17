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

            var newsIdsToIgnore = col
                .Query()
                .Where(news => !newsArticlesList
                    .Select(newsArticle => newsArticle.SourceId)
                    .Any(newsArticleSourceId => newsArticleSourceId
                        .Equals(news.SourceId, StringComparison.InvariantCultureIgnoreCase)))
                .Select(news => news.Id)
                .ToList() ?? new List<ObjectId>(0);

            var newsToInsert = newsArticlesList
                .Where(newsArticle => !newsIdsToIgnore.Contains(newsArticle.Id));

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