namespace Microservicio.Vuelos.DataAccess.Repositories.Interfaces;

using Microservicio.Vuelos.DataAccess.Entities;

public interface IBoletoRepository
{
    Task<BoletoEntity?> ObtenerPorIdAsync(int id, CancellationToken cancellationToken = default);
    Task<IEnumerable<BoletoEntity>> ObtenerTodosAsync(CancellationToken cancellationToken = default);
    Task AgregarAsync(BoletoEntity entity, CancellationToken cancellationToken = default);
    void Actualizar(BoletoEntity entity);
    void Eliminar(BoletoEntity entity);
}
