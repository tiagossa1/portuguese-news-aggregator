using System.ServiceModel.Syndication;
using System.Xml;
using Application.Constants;
using Application.Interfaces;
using Application.Models;

namespace Application.Readers;

public class NoticiasAoMinutoNewsReader : IRssNewsReader
{
    public string Name => NewsCodeConstants.NoticiasAoMinuto;

    public IList<NewsArticle> Read(NewsWebsite newsWebsite)
    {
        using var reader = XmlReader.Create(newsWebsite.Url);
        var feed = SyndicationFeed.Load(reader);
        var articles = feed.Items;

        return articles
            ?.Select(news => new NewsArticle
            {
                Source = newsWebsite.Code,
                Title = news.Title.Text,
                CreatedAt = DateTime.Now,
                Url = news.Links.ElementAtOrDefault(0)?.Uri?.AbsoluteUri ?? string.Empty,
                PublishDate = DateOnly.FromDateTime(news.PublishDate.Date),
                SourceId = news.Links.ElementAtOrDefault(0)?.Uri?.AbsoluteUri ?? Guid.NewGuid().ToString(),
                NewsCategories = news.Categories
                ?.Select(category => new NewsCategory
                {
                    Code = category.Name.ToUpperInvariant(),
                    Name = category.Name
                })
                .ToList() ?? new List<NewsCategory>(0)
            })
            .ToList() ?? new List<NewsArticle>(0);
    }
}