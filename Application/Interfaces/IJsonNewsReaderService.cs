using Application.Models;

namespace Application.Interfaces;

public interface IJsonNewsReaderService
{
    Task<IList<NewsArticle>> Read(IList<NewsWebsite> newsWebsites);
}