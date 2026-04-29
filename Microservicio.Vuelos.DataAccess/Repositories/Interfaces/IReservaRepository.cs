namespace Microservicio.Vuelos.DataAccess.Repositories.Interfaces;

using Microservicio.Vuelos.DataAccess.Entities;
using Microservicio.Vuelos.DataAccess.Common;

public interface IReservaRepository
{
    Task<ReservaEntity?> ObtenerPorIdAsync(int id, CancellationToken cancellationToken = default);
    Task<IEnumerable<ReservaEntity>> ObtenerTodosAsync(CancellationToken cancellationToken = default);
    Task AgregarAsync(ReservaEntity entity, CancellationToken cancellationToken = default);
    void Actualizar(ReservaEntity entity);
    void Eliminar(ReservaEntity entity);

    // Consulta filtrada y paginada
    Task<PagedResult<ReservaEntity>> BuscarReservasAsync(
        int? idCliente,
        string? estado,
        int page,
        int pageSize,
        CancellationToken cancellationToken = default);
}
