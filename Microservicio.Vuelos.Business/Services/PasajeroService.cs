using Microservicio.Vuelos.Business.DTOs.Pasajero;
using Microservicio.Vuelos.Business.Interfaces;
using Microservicio.Vuelos.Business.Mappers;
using Microservicio.Vuelos.DataManagement.Interfaces;

namespace Microservicio.Vuelos.Business.Services;

public class PasajeroService : IPasajeroService
{
    private readonly IPasajeroDataService _pasajeroDataService;

    public PasajeroService(IPasajeroDataService pasajeroDataService)
    {
        _pasajeroDataService = pasajeroDataService;
    }

    public async Task<PasajeroResponse> RegistrarPasajeroAsync(PasajeroRequest request, CancellationToken cancellationToken = default)
    {
        var dataModel = request.ToDataModel();
        var result = await _pasajeroDataService.CreatePasajeroAsync(dataModel, cancellationToken);
        return result.ToResponse();
    }

    public async Task<PasajeroResponse?> GetPasajeroByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        var result = await _pasajeroDataService.GetPasajeroByIdAsync(id, cancellationToken);
        return result?.ToResponse();
    }
}
