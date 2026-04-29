namespace Microservicio.Vuelos.DataAccess.Repositories.Interfaces;

using Microservicio.Vuelos.DataAccess.Entities;
using Microservicio.Vuelos.DataAccess.Common;

public interface IAeropuertoRepository
{
    Task<AeropuertoEntity?> ObtenerPorIdAsync(int idAeropuerto, CancellationToken cancellationToken = default);
    Task<IEnumerable<AeropuertoEntity>> ObtenerTodosAsync(CancellationToken cancellationToken = default);
    Task AgregarAsync(AeropuertoEntity aeropuerto, CancellationToken cancellationToken = default);
    void Actualizar(AeropuertoEntity aeropuerto);
    void Eliminar(AeropuertoEntity aeropuerto);

    // Consulta filtrada y paginada
    Task<PagedResult<AeropuertoEntity>> BuscarAeropuertosAsync(
        string? estado,
        int page,
        int pageSize,
        CancellationToken cancellationToken = default);
}
