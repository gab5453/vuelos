namespace Microservicio.Vuelos.DataAccess.Repositories.Interfaces;

using Microservicio.Vuelos.DataAccess.Entities;

public interface ICiudadRepository
{
    Task<CiudadEntity?> ObtenerPorIdAsync(int idCiudad, CancellationToken cancellationToken = default);
    Task<IEnumerable<CiudadEntity>> ObtenerTodosAsync(CancellationToken cancellationToken = default);
    Task AgregarAsync(CiudadEntity ciudad, CancellationToken cancellationToken = default);
    void Actualizar(CiudadEntity ciudad);
    void Eliminar(CiudadEntity ciudad);
}
