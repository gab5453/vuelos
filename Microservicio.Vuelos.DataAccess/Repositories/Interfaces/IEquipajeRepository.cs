namespace Microservicio.Vuelos.DataAccess.Repositories.Interfaces;

using Microservicio.Vuelos.DataAccess.Entities;

public interface IEquipajeRepository
{
    Task<EquipajeEntity?> ObtenerPorIdAsync(int id, CancellationToken cancellationToken = default);
    Task<IEnumerable<EquipajeEntity>> ObtenerTodosAsync(CancellationToken cancellationToken = default);
    Task AgregarAsync(EquipajeEntity entity, CancellationToken cancellationToken = default);
    void Actualizar(EquipajeEntity entity);
    void Eliminar(EquipajeEntity entity);
}
