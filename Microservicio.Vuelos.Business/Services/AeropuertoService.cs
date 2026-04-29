using Microservicio.Vuelos.Business.DTOs.Aeropuerto;
using Microservicio.Vuelos.Business.Interfaces;
using Microservicio.Vuelos.Business.Mappers;
using Microservicio.Vuelos.DataManagement.Interfaces;
using Microservicio.Vuelos.DataManagement.Models;
using Microservicio.Vuelos.DataManagement.Models.Aeropuerto;

namespace Microservicio.Vuelos.Business.Services;

public class AeropuertoService : IAeropuertoService
{
    private readonly IAeropuertoDataService _aeropuertoDataService;

    public AeropuertoService(IAeropuertoDataService aeropuertoDataService)
    {
        _aeropuertoDataService = aeropuertoDataService;
    }

    public async Task<DataPagedResult<AeropuertoResponse>> GetAeropuertosAsync(AeropuertoFiltroRequest request, CancellationToken cancellationToken = default)
    {
        var filtroDataModel = new AeropuertoFiltroDataModel
        {
            Estado = request.Estado,
            Page = request.Page,
            PageSize = request.PageSize
        };

        var pagedResult = await _aeropuertoDataService.GetAeropuertosAsync(filtroDataModel, cancellationToken);
        
        return new DataPagedResult<AeropuertoResponse>(
            pagedResult.Items.Select(m => m.ToResponse()),
            pagedResult.TotalRegistros,
            pagedResult.PaginaActual,
            pagedResult.TamanoPagina);
    }

    public async Task<AeropuertoResponse?> GetAeropuertoByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        var model = await _aeropuertoDataService.GetAeropuertoByIdAsync(id, cancellationToken);
        return model?.ToResponse();
    }
}
