using System.Net.Http.Json;
using Application.Constants;
using Application.Dtos;
using Application.Interfaces;
using Application.Models;

namespace Application.Readers;

public class ObservadorJsonNewsReader : IJsonNewsReader
{
    private readonly IHttpClientFactory _httpClientFactory;

    public ObservadorJsonNewsReader(IHttpClientFactory httpClientFactory)
    {
        _httpClientFactory = httpClientFactory;
    }

    public string Name => NewsCodeConstants.Observador;

    public async Task<IList<NewsArticle>> Read(NewsWebsite newsWebsite)
    {
        var httpClient = _httpClientFactory.CreateClient();
        var response = await httpClient.GetFromJsonAsync<ObservadorResponseDto[]>(newsWebsite.Url);

        return response
            ?.Select(news => new NewsArticle
            {
                SourceId = news.Id.ToString(),
                Source = newsWebsite.Code,
                Title = news.Title,
                CreatedAt = DateTime.Now,
                Url = news.Url,
                ImageUrl = news.Image,
                PublishDate = news.PublishDate.HasValue
                    ? DateOnly.FromDateTime(news.PublishDate.Value)
                    : null,
                NewsCategories =
                    !string.IsNullOrWhiteSpace(news.Tag)
                        ?
                        [
                            new NewsCategory
                            {
                                Code = news.Tag.ToUpperInvariant(),
                                Name = news.Tag
                            }
                        ]
                        : []
            })
            .ToList() ?? [];
    }
}