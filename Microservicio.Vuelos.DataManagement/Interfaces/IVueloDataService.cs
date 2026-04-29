namespace Microservicio.Vuelos.DataManagement.Interfaces;

using Microservicio.Vuelos.DataManagement.Models.Vuelo;
using Microservicio.Vuelos.DataManagement.Models;

public interface IVueloDataService
{

    Task<DataPagedResult<VueloDataModel>> BuscarVuelosAsync(VueloFiltroDataModel filtro, CancellationToken cancellationToken = default);
    Task<VueloDataModel?> GetVueloByIdAsync(int id, CancellationToken cancellationToken = default);
    Task<IEnumerable<EscalaDataModel>> GetEscalasAsync(int idVuelo, CancellationToken cancellationToken = default);
    Task<IEnumerable<AsientoDataModel>> GetAsientosAsync(int idVuelo, bool? disponible, string? clase, CancellationToken cancellationToken = default);
}
