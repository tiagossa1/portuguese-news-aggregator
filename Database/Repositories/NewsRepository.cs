using System.Text.Json;
using System.Text.Json.Serialization;
using Domain.Entities;
using Domain.Interfaces;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace Database.Repositories;

public class NewsRepository : INewsRepository
{
    private const string CollectionName = "news";

    private readonly ILogger<NewsRepository> _logger;
    private readonly IMongoDatabase _mongoDatabase;
    private readonly IMongoCollection<News> _collection;

    public NewsRepository(IOptions<MongoConnectionOptions> options, ILogger<NewsRepository> logger)
    {
        _logger = logger;

        _logger.LogInformation(JsonSerializer.Serialize(options.Value));

        var mongoClient = new MongoClient(options.Value.ConnectionString);
        _mongoDatabase = mongoClient.GetDatabase(options.Value.DatabaseName);
        _collection = _mongoDatabase.GetCollection<News>(CollectionName);
    }

    public async Task CreateRange(IList<News> newsList)
    {
        try
        {
            var dbNews = await _collection.Find(_ => true).ToListAsync();

            var newsToInsert = new List<News>();

            foreach (var article in newsList)
            {
                var exists = dbNews.Any(news => news.SourceId.Equals(article.SourceId, StringComparison.InvariantCultureIgnoreCase));
                if (!exists)
                {
                    _logger.LogInformation("{newsRepository}.{createRange}: upserting news '{articleTitle}'", nameof(NewsCategory), nameof(CreateRange), article.Title);

                    newsToInsert.Add(article);
                }
            }

            if (newsToInsert.Count > 0)
            {
                await _collection.InsertManyAsync(newsToInsert);
            }
        }
        catch (Exception e)
        {
            _logger.LogError("{newsRepository}.{createRange}: there was an error upserting the document: {exception}", nameof(NewsCategory), nameof(CreateRange), e.ToString());
        }
    }

    public async Task<IList<News>> GetAll()
    {
        return await _collection
            .Find(f => true)
            .ToListAsync();
    }
}