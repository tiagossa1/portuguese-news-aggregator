using System.Net.Http.Json;
using Application.Constants;
using Application.Dtos;
using Application.Interfaces;
using Application.Models;

namespace Application.Readers;

public class PublicoJsonNewsReader : IJsonNewsReader
{
    private readonly IHttpClientFactory _httpClientFactory;

    public PublicoJsonNewsReader(IHttpClientFactory httpClientFactory)
    {
        _httpClientFactory = httpClientFactory;
    }

    public string Name => NewsCodeConstants.Publico;

    public async Task<IList<NewsArticle>> Read(NewsWebsite newsWebsite)
    {
        var httpClient = _httpClientFactory.CreateClient();
        var response = await httpClient.GetFromJsonAsync<PublicoResponseDto[]>(newsWebsite.Url);

        return response
            ?.Select(news => new NewsArticle
            {
                SourceId = news.Id.ToString(),
                Source = newsWebsite.Code,
                Title = news.Titulo,
                CreatedAt = DateTime.Now,
                Url = news.Url,
                ImageUrl = news.ImagemUrl,
                PublishDate = DateOnly.FromDateTime(news.Data),
                NewsCategories =
                    !string.IsNullOrWhiteSpace(news.Rubrica)
                        ?
                        new List<NewsCategory> {
                            new()
                            {
                                Code = news.Rubrica.ToUpperInvariant(),
                                Name = news.Rubrica
                            }
                        }
                        : new List<NewsCategory>(0)
            })
            .ToList() ?? new List<NewsArticle>(0);
    }
}