using Microservicio.Vuelos.DataManagement.Models.Pasajero;

namespace Microservicio.Vuelos.DataManagement.Interfaces;

public interface IPasajeroDataService
{
    Task<PasajeroDataModel> CreatePasajeroAsync(PasajeroDataModel model, CancellationToken cancellationToken = default);
    Task<PasajeroDataModel?> GetPasajeroByIdAsync(int id, CancellationToken cancellationToken = default);
}
