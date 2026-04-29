namespace Microservicio.Vuelos.DataManagement.Models.Vuelo;

public class VueloDataModel
{
    public int IdVuelo { get; set; }
    public string NumeroVuelo { get; set; } = string.Empty;
    public DateTime FechaHoraSalida { get; set; }
    public DateTime FechaHoraLlegada { get; set; }
    public int DuracionMin { get; set; }
    public decimal PrecioBase { get; set; }
    public int CapacidadTotal { get; set; }
    public string EstadoVuelo { get; set; } = string.Empty;
    public string Estado { get; set; } = string.Empty;

    public AeropuertoInfoDataModel Origen { get; set; } = null!;
    public AeropuertoInfoDataModel Destino { get; set; } = null!;
}

public class AeropuertoInfoDataModel
{
    public int IdAeropuerto { get; set; }
    public string CodigoIata { get; set; } = string.Empty;
    public string Nombre { get; set; } = string.Empty;
    public string NombreCiudad { get; set; } = string.Empty;
}
