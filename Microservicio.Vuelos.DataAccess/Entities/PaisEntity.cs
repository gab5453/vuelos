namespace Microservicio.Vuelos.DataAccess.Entities;

public class PaisEntity
{
    public int Id_pais { get; set; }
    public string Codigo_iso2 { get; set; } = string.Empty;
    public string Codigo_iso3 { get; set; } = string.Empty;
    public string Nombre { get; set; } = string.Empty;
    public string Continente { get; set; } = string.Empty;
    public string Estado { get; set; } = "ACTIVO";
    public bool Eliminado { get; set; }

    public virtual ICollection<CiudadEntity> Ciudades { get; set; } = new List<CiudadEntity>();
}
