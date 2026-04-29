namespace Microservicio.Vuelos.DataAccess.Entities;

public class EscalaEntity
{
    public int Id_escala { get; set; }
    public int Id_vuelo { get; set; }
    public int Id_aeropuerto_escala { get; set; }
    public int Orden { get; set; }

    public virtual VueloEntity Vuelo { get; set; } = null!;
    public virtual AeropuertoEntity AeropuertoEscala { get; set; } = null!;
}
