namespace Microservicio.Vuelos.DataAccess.Repositories.Interfaces;

using Microservicio.Vuelos.DataAccess.Entities;

public interface IPaisRepository
{
    Task<PaisEntity?> ObtenerPorIdAsync(int idPais, CancellationToken cancellationToken = default);
    Task<IEnumerable<PaisEntity>> ObtenerTodosAsync(CancellationToken cancellationToken = default);
    Task AgregarAsync(PaisEntity pais, CancellationToken cancellationToken = default);
    void Actualizar(PaisEntity pais);
    void Eliminar(PaisEntity pais);
}
