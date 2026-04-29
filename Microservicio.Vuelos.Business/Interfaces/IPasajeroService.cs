using Microservicio.Vuelos.Business.DTOs.Pasajero;

namespace Microservicio.Vuelos.Business.Interfaces;

public interface IPasajeroService
{
    Task<PasajeroResponse> RegistrarPasajeroAsync(PasajeroRequest request, CancellationToken cancellationToken = default);
    Task<PasajeroResponse?> GetPasajeroByIdAsync(int id, CancellationToken cancellationToken = default);
}
