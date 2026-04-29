using Microservicio.Vuelos.DataManagement.Interfaces;
using Microservicio.Vuelos.DataManagement.Mappers;
using Microservicio.Vuelos.DataManagement.Models.Aeropuerto;
using Microservicio.Vuelos.DataManagement.Models;

namespace Microservicio.Vuelos.DataManagement.Services;

public class AeropuertoDataService : IAeropuertoDataService
{
    private readonly IUnitOfWork _unitOfWork;

    public AeropuertoDataService(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<DataPagedResult<AeropuertoDataModel>> GetAeropuertosAsync(AeropuertoFiltroDataModel filtro, CancellationToken cancellationToken = default)
    {
        var pagedResult = await _unitOfWork.AeropuertoRepository.BuscarAeropuertosAsync(
            filtro.Estado,
            filtro.Page,
            filtro.PageSize,
            cancellationToken);

        return new DataPagedResult<AeropuertoDataModel>(
            pagedResult.Items.Select(e => e.ToDataModel()),
            pagedResult.TotalRegistros,
            pagedResult.PaginaActual,
            pagedResult.TamanoPagina);
    }

    public async Task<AeropuertoDataModel?> GetAeropuertoByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        var entity = await _unitOfWork.AeropuertoRepository.ObtenerPorIdAsync(id, cancellationToken);
        return entity?.ToDataModel();
    }
}
