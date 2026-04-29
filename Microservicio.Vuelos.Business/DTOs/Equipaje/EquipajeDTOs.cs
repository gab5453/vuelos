namespace Microservicio.Vuelos.Business.DTOs.Equipaje;

public class RegistrarEquipajeRequest
{
    public string Tipo { get; set; } = "BODEGA";
    public decimal PesoKg { get; set; }
    public string? Descripcion { get; set; }
}

public class EquipajeResponse
{
    public int IdEquipaje { get; set; }
    public int IdBoleto { get; set; }
    public string Tipo { get; set; } = string.Empty;
    public decimal PesoKg { get; set; }
}
