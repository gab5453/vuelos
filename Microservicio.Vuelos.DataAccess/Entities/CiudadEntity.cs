namespace Microservicio.Vuelos.DataAccess.Entities;

public class CiudadEntity
{
    public int Id_ciudad { get; set; }
    public int Id_pais { get; set; }
    public string Nombre { get; set; } = string.Empty;
    public string Estado { get; set; } = "ACTIVO";
    public bool Eliminado { get; set; }

    public virtual PaisEntity Pais { get; set; } = null!;
    public virtual ICollection<AeropuertoEntity> Aeropuertos { get; set; } = new List<AeropuertoEntity>();
}
