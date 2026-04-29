namespace Microservicio.Vuelos.Business.DTOs.Vuelo;

public class VueloResponse
{
    public int IdVuelo { get; set; }
    public string NumeroVuelo { get; set; } = string.Empty;
    public int IdAeropuertoOrigen { get; set; }
    public int IdAeropuertoDestino { get; set; }
    public DateTime FechaHoraSalida { get; set; }
    public DateTime FechaHoraLlegada { get; set; }
    public int DuracionMin { get; set; }
    public decimal PrecioBase { get; set; }
    public int CapacidadTotal { get; set; }
    public string EstadoVuelo { get; set; } = string.Empty;
    public string Estado { get; set; } = string.Empty;

    // Se mantienen como info adicional si es necesario
    public AeropuertoInfoResponse? Origen { get; set; }
    public AeropuertoInfoResponse? Destino { get; set; }
}

public class AeropuertoInfoResponse
{
    public int IdAeropuerto { get; set; }
    public string CodigoIata { get; set; } = string.Empty;
    public string Nombre { get; set; } = string.Empty;
    public string NombreCiudad { get; set; } = string.Empty;
}
