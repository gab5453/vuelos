namespace Microservicio.Vuelos.DataAccess.Repositories.Interfaces;

using Microservicio.Vuelos.DataAccess.Entities;

public interface IAsientoRepository
{
    Task<AsientoEntity?> ObtenerPorIdAsync(int id, CancellationToken cancellationToken = default);
    Task<IEnumerable<AsientoEntity>> ObtenerTodosAsync(CancellationToken cancellationToken = default);
    Task AgregarAsync(AsientoEntity entity, CancellationToken cancellationToken = default);
    void Actualizar(AsientoEntity entity);
    void Eliminar(AsientoEntity entity);
}
