using LiteDB;

namespace Domain.Entities;

public class News
{
    public ObjectId Id { get; set; }

    public string Title { get; set; }

    public string Source { get; set; }

    public string SourceId { get; set; }

    public DateOnly? PublishDate { get; set; }

    public List<NewsCategory> NewsCategories { get; set; }

    public string ImageUrl { get; set; }

    public DateTime CreatedAt { get; set; }
}