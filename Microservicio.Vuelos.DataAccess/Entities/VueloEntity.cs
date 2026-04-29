namespace Microservicio.Vuelos.DataAccess.Entities;

public class VueloEntity
{
    public int Id_vuelo { get; set; }
    public string Numero_vuelo { get; set; } = string.Empty;
    public int Id_aeropuerto_origen { get; set; }
    public int Id_aeropuerto_destino { get; set; }
    public DateTime Fecha_hora_salida { get; set; }
    public DateTime Fecha_hora_llegada { get; set; }
    public int Duracion_min { get; set; }
    public decimal Precio_base { get; set; }
    public int Capacidad_total { get; set; }
    public string Estado_vuelo { get; set; } = string.Empty;
    public string Estado { get; set; } = string.Empty;

    public virtual AeropuertoEntity AeropuertoOrigen { get; set; } = null!;
    public virtual AeropuertoEntity AeropuertoDestino { get; set; } = null!;
    public virtual ICollection<EscalaEntity> Escalas { get; set; } = new List<EscalaEntity>();
    public virtual ICollection<AsientoEntity> Asientos { get; set; } = new List<AsientoEntity>();
}
