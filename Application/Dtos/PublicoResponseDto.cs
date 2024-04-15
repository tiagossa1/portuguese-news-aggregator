using System.Text.Json.Serialization;

namespace Application.Dtos;

public record PublicoResponseDto(
    [property: JsonPropertyName("tituloNoticia")]
    string Titulo,
    [property: JsonPropertyName("id")] int Id,
    [property: JsonPropertyName("url")] string Url,
    [property: JsonPropertyName("lead")] object Lead,
    [property: JsonPropertyName("data")] DateTime Data,
    [property: JsonPropertyName("multimediaPrincipal")] string ImagemUrl,
    [property: JsonPropertyName("rubrica")] string Rubrica);