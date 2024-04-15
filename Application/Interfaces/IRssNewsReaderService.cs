using Application.Models;

namespace Application.Interfaces;

public interface IRssNewsReaderService
{
    IList<NewsArticle> Read(IList<NewsWebsite> newsWebsites);
}