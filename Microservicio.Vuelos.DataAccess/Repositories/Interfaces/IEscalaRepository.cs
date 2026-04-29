namespace Microservicio.Vuelos.DataAccess.Repositories.Interfaces;

using Microservicio.Vuelos.DataAccess.Entities;

public interface IEscalaRepository
{
    Task<EscalaEntity?> ObtenerPorIdAsync(int id, CancellationToken cancellationToken = default);
    Task<IEnumerable<EscalaEntity>> ObtenerTodosAsync(CancellationToken cancellationToken = default);
    Task AgregarAsync(EscalaEntity entity, CancellationToken cancellationToken = default);
    void Actualizar(EscalaEntity entity);
    void Eliminar(EscalaEntity entity);
}
