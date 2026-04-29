using Microservicio.Vuelos.Business.DTOs.Pais;
using Microservicio.Vuelos.Business.Interfaces;
using Microservicio.Vuelos.DataAccess.Repositories.Interfaces;

namespace Microservicio.Vuelos.Business.Services;

public class PaisService : IPaisService
{
    private readonly IPaisRepository _paisRepository;

    public PaisService(IPaisRepository paisRepository)
    {
        _paisRepository = paisRepository;
    }

    public async Task<IEnumerable<PaisResponse>> GetPaisesAsync(CancellationToken cancellationToken = default)
    {
        var entities = await _paisRepository.ObtenerTodosAsync(cancellationToken);

        return entities.Select(p => new PaisResponse
        {
            IdPais = p.Id_pais,
            Nombre = p.Nombre
        });
    }
}