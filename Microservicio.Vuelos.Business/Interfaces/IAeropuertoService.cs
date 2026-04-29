using Microservicio.Vuelos.Business.DTOs.Aeropuerto;
using Microservicio.Vuelos.DataManagement.Models;

namespace Microservicio.Vuelos.Business.Interfaces;

public interface IAeropuertoService
{

    Task<DataPagedResult<AeropuertoResponse>> GetAeropuertosAsync(AeropuertoFiltroRequest request, CancellationToken cancellationToken = default);
    Task<AeropuertoResponse?> GetAeropuertoByIdAsync(int id, CancellationToken cancellationToken = default);
}
