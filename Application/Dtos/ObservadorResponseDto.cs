using System.Text.Json.Serialization;

namespace Application.Dtos;

public record ObservadorResponseDto(
    [property: JsonPropertyName("id")] int Id,
    [property: JsonPropertyName("title")] string Title,
    [property: JsonPropertyName("publish_date")] DateTime? PublishDate,
    [property: JsonPropertyName("tag")] string Tag,
    [property: JsonPropertyName("image")] string Image,
    [property: JsonPropertyName("url")] string Url
    );