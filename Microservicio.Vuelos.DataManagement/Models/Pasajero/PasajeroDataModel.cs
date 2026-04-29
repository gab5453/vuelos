namespace Microservicio.Vuelos.DataManagement.Models.Pasajero;

public class PasajeroDataModel
{
    public int IdPasajero { get; set; }
    public string NombrePasajero { get; set; } = string.Empty;
    public string ApellidoPasajero { get; set; } = string.Empty;
    public string TipoDocumentoPasajero { get; set; } = string.Empty;
    public string NumeroDocumentoPasajero { get; set; } = string.Empty;
    public int? IdCliente { get; set; }
    public DateTime? FechaNacimientoPasajero { get; set; }
    public string? NacionalidadPasajero { get; set; }
    public string? EmailContactoPasajero { get; set; }
    public string? TelefonoContactoPasajero { get; set; }
    public string? GeneroPasajero { get; set; }
    public bool RequiereAsistencia { get; set; }
    public string? ObservacionesPasajero { get; set; }
}
