using System.Text.Json.Serialization;

namespace Microservicio.Vuelos.Business.DTOs.Cliente;

public class ActualizarClienteRequest
{
    [JsonIgnore]
    public Guid GuidCliente { get; set; }

    [JsonPropertyName("nombres")]
    public string? Nombres { get; set; }

    [JsonPropertyName("apellidos")]
    public string? Apellidos { get; set; }

    [JsonPropertyName("razon_social")]
    public string? RazonSocial { get; set; }

    [JsonPropertyName("tipo_identificacion")]
    public string TipoIdentificacion { get; set; } = null!;

    [JsonPropertyName("numero_identificacion")]
    public string NumeroIdentificacion { get; set; } = null!;

    [JsonPropertyName("correo")]
    public string Correo { get; set; } = null!;

    [JsonPropertyName("telefono")]
    public string? Telefono { get; set; }

    [JsonPropertyName("direccion")]
    public string? Direccion { get; set; }

    [JsonPropertyName("id_ciudad_residencia")]
    public int IdCiudadResidencia { get; set; }

    [JsonPropertyName("id_pais_nacionalidad")]
    public int IdPaisNacionalidad { get; set; }

    [JsonPropertyName("fecha_nacimiento")]
    public DateTime? FechaNacimiento { get; set; }

    [JsonPropertyName("nacionalidad")]
    public string? Nacionalidad { get; set; }

    [JsonPropertyName("genero")]
    public string? Genero { get; set; }

    [JsonPropertyName("estado")]
    public string Estado { get; set; } = null!;

    [JsonIgnore]
    public string? ModificadoPorUsuario { get; set; }

    [JsonIgnore]
    public string? ModificacionIp { get; set; }

    [JsonIgnore]
    public string? ServicioOrigen { get; set; }
}
