namespace Application.Models;

public class NewsArticle
{
    /// <summary>
    /// Generated ID for the news article
    /// </summary>
    public string Id { get; set; }

    /// <summary>
    /// Source ID of the news article. The news source sometimes provide their own ID for the article and if it does, it's a good way to know if this article is already in the database and we can avoid duplicated articles.
    /// </summary>
    public string SourceId { get; set; }

    /// <summary>
    /// Title of the news article
    /// </summary>
    public string Title { get; set; }

    /// <summary>
    /// News source
    /// </summary>
    public string Source { get; set; }

    /// <summary>
    /// When the news article was published
    /// </summary>
    public DateOnly? PublishDate { get; set; }

    /// <summary>
    /// List of categories that the articles fits in
    /// </summary>
    public List<NewsCategory> NewsCategories { get; set; }

    /// <summary>
    /// News article URL for an easier access.
    /// </summary>
    public string Url { get; set; }

    /// <summary>
    /// News article front image
    /// </summary>
    public string ImageUrl { get; set; }

    /// <summary>
    /// Date Time when the news article was inserted into the database
    /// </summary>
    public DateTime CreatedAt { get; set; }
}