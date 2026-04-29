using Microservicio.Vuelos.Business.DTOs.Equipaje;

namespace Microservicio.Vuelos.Business.Interfaces;

public interface IEquipajeService
{
    Task<EquipajeResponse> RegistrarEquipajeAsync(int idBoleto, RegistrarEquipajeRequest request, CancellationToken cancellationToken = default);
    Task<IEnumerable<EquipajeResponse>> GetEquipajeByBoletoAsync(int idBoleto, CancellationToken cancellationToken = default);
}
