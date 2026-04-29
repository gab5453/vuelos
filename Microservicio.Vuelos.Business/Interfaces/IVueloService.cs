namespace Microservicio.Vuelos.Business.Interfaces;

using Microservicio.Vuelos.Business.DTOs.Vuelo;
using Microservicio.Vuelos.DataManagement.Models;

public interface IVueloService
{

    Task<DataPagedResult<VueloResponse>> BuscarVuelosAsync(VueloFiltroRequest request, CancellationToken cancellationToken = default);
    Task<VueloResponse?> GetVueloByIdAsync(int id, CancellationToken cancellationToken = default);
    Task<IEnumerable<EscalaResponse>> GetEscalasAsync(int idVuelo, CancellationToken cancellationToken = default);
    Task<IEnumerable<AsientoResponse>> GetAsientosAsync(int idVuelo, bool? disponible, string? clase, CancellationToken cancellationToken = default);
}
