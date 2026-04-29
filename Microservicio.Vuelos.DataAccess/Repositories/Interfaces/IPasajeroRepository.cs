namespace Microservicio.Vuelos.DataAccess.Repositories.Interfaces;

using Microservicio.Vuelos.DataAccess.Entities;

public interface IPasajeroRepository
{
    Task<PasajeroEntity?> ObtenerPorIdAsync(int id, CancellationToken cancellationToken = default);
    Task<IEnumerable<PasajeroEntity>> ObtenerTodosAsync(CancellationToken cancellationToken = default);
    Task AgregarAsync(PasajeroEntity entity, CancellationToken cancellationToken = default);
    void Actualizar(PasajeroEntity entity);
    void Eliminar(PasajeroEntity entity);
}
