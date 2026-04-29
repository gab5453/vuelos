using System.Text.Json.Serialization;

namespace Microservicio.Vuelos.Business.DTOs.Cliente;

public class ClienteFiltroRequest
{
    [JsonPropertyName("numero_identificacion")]
    public string? NumeroIdentificacion { get; set; }

    [JsonPropertyName("correo")]
    public string? Correo { get; set; }

    [JsonPropertyName("estado")]
    public string? Estado { get; set; }

    [JsonPropertyName("page")]
    public int Page { get; set; } = 1;

    [JsonPropertyName("page_size")]
    public int PageSize { get; set; } = 20;
}
