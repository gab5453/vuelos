namespace Microservicio.Vuelos.DataAccess.Repositories.Interfaces;

using Microservicio.Vuelos.DataAccess.Entities;
using Microservicio.Vuelos.DataAccess.Common;

public interface IVueloRepository
{
    Task<VueloEntity?> ObtenerPorIdAsync(int idVuelo, CancellationToken cancellationToken = default);
    Task<IEnumerable<VueloEntity>> ObtenerTodosAsync(CancellationToken cancellationToken = default);
    Task AgregarAsync(VueloEntity vuelo, CancellationToken cancellationToken = default);
    void Actualizar(VueloEntity vuelo);
    void Eliminar(VueloEntity vuelo);

    // Consulta filtrada y paginada
    Task<PagedResult<VueloEntity>> BuscarVuelosAsync(
        int idAeropuertoOrigen,
        int idAeropuertoDestino,
        DateTime fechaSalida,
        int page,
        int pageSize,
        CancellationToken cancellationToken = default);
}
