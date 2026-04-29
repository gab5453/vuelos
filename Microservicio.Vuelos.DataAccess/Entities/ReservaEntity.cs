namespace Microservicio.Vuelos.DataAccess.Entities;

public class ReservaEntity
{
    public int Id_reserva { get; set; }
    public Guid Guid_reserva { get; set; }
    public string Codigo_reserva { get; set; } = string.Empty;
    public int Id_cliente { get; set; }
    public int Id_pasajero { get; set; }
    public int Id_vuelo { get; set; }
    public int Id_asiento { get; set; }
    public DateTime Fecha_reserva_utc { get; set; }
    public DateTime Fecha_inicio { get; set; }
    public DateTime Fecha_fin { get; set; }
    public decimal Subtotal_reserva { get; set; }
    public decimal Valor_iva { get; set; }
    public decimal Total_reserva { get; set; }
    public string Origen_canal_reserva { get; set; } = "BOOKING";
    public string Estado_reserva { get; set; } = string.Empty;
    public string? Contacto_email { get; set; }
    public string? Contacto_telefono { get; set; }
    public string? Observaciones { get; set; }

    public virtual ClienteEntity Cliente { get; set; } = null!;
    public virtual VueloEntity Vuelo { get; set; } = null!;
    public virtual ICollection<BoletoEntity> Boletos { get; set; } = new List<BoletoEntity>();
}
