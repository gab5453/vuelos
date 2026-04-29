using Microservicio.Vuelos.Business.DTOs.Pais;

namespace Microservicio.Vuelos.Business.Interfaces;

public interface IPaisService
{
    Task<IEnumerable<PaisResponse>> GetPaisesAsync(CancellationToken cancellationToken = default);
}