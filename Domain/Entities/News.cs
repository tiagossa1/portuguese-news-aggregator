using System.ComponentModel.DataAnnotations;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Domain.Entities;

public class News
{
    [BsonRepresentation(BsonType.ObjectId)]
    [Required]
    public string Id { get; set; }

    [Required]
    public string Title { get; set; }

    [Required]
    public string Source { get; set; }
    
    [Required]
    public string SourceId { get; set; }

    [Required]
    public DateOnly? PublishDate { get; set; }

    [Required]
    public List<NewsCategory> NewsCategories { get; set; }

    [Required]
    public string ImageUrl { get; set; }

    [Required]
    public DateTime CreatedAt { get; set; }
}