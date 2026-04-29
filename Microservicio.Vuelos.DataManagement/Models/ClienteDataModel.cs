namespace Microservicio.Vuelos.DataManagement.Models;

public class ClienteDataModel
{
    public int IdCliente { get; set; }
    public Guid GuidCliente { get; set; }
    public int IdUsuario { get; set; }
    public string? Nombres { get; set; }
    public string? Apellidos { get; set; }
    public string? RazonSocial { get; set; }
    public string TipoIdentificacion { get; set; } = null!;
    public string NumeroIdentificacion { get; set; } = null!;
    public string Correo { get; set; } = null!;
    public string? Telefono { get; set; }
    public string? Direccion { get; set; }
    public int IdCiudadResidencia { get; set; }
    public int IdPaisNacionalidad { get; set; }
    public DateTime? FechaNacimiento { get; set; }
    public string? Nacionalidad { get; set; }
    public string? Genero { get; set; }
    public string Estado { get; set; } = "ACT";
    public bool EsEliminado { get; set; }
    public string? CreadoPorUsuario { get; set; }
    public DateTimeOffset FechaRegistroUtc { get; set; }
    public string? ModificadoPorUsuario { get; set; }
    public DateTimeOffset? FechaModificacionUtc { get; set; }
    public string? ModificacionIp { get; set; }
    public string? ServicioOrigen { get; set; }
}
