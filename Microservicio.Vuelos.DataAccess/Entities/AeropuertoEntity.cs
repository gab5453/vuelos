namespace Microservicio.Vuelos.DataAccess.Entities;

public class AeropuertoEntity
{
    public int Id_aeropuerto { get; set; }
    public int Id_ciudad { get; set; }
    public string Codigo_iata { get; set; } = string.Empty;
    public string Nombre { get; set; } = string.Empty;
    public decimal Latitud { get; set; }
    public decimal Longitud { get; set; }
    public string Zona_horaria { get; set; } = string.Empty;
    public string Estado { get; set; } = "ACTIVO";
    public bool Eliminado { get; set; }

    public virtual CiudadEntity Ciudad { get; set; } = null!;
}
