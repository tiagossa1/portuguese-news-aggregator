using Application.Models;

namespace Application.Interfaces;

public interface IJsonNewsReader
{
    string Name { get; }
    
    Task<IList<NewsArticle>> Read(NewsWebsite newsWebsite);
}