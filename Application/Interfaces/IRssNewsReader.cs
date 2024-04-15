using Application.Models;

namespace Application.Interfaces;

public interface IRssNewsReader
{
    string Name { get; }

    IList<NewsArticle> Read(NewsWebsite newsWebsite);
}