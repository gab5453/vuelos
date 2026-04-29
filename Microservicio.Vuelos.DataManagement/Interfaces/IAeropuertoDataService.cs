using Microservicio.Vuelos.DataManagement.Models.Aeropuerto;
using Microservicio.Vuelos.DataManagement.Models;

namespace Microservicio.Vuelos.DataManagement.Interfaces;

public interface IAeropuertoDataService
{

    Task<DataPagedResult<AeropuertoDataModel>> GetAeropuertosAsync(AeropuertoFiltroDataModel filtro, CancellationToken cancellationToken = default);
    Task<AeropuertoDataModel?> GetAeropuertoByIdAsync(int id, CancellationToken cancellationToken = default);
}
