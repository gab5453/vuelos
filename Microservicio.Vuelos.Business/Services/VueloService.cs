namespace Microservicio.Vuelos.Business.Services;

using Microservicio.Vuelos.Business.DTOs.Vuelo;
using Microservicio.Vuelos.Business.Exceptions;
using Microservicio.Vuelos.Business.Interfaces;
using Microservicio.Vuelos.Business.Mappers;
using Microservicio.Vuelos.DataManagement.Interfaces;
using Microservicio.Vuelos.DataManagement.Models.Vuelo;
using Microservicio.Vuelos.DataManagement.Models;

public class VueloService : IVueloService
{
    private readonly IVueloDataService _vueloDataService;

    public VueloService(IVueloDataService vueloDataService)
    {
        _vueloDataService = vueloDataService;
    }

    public async Task<DataPagedResult<VueloResponse>> BuscarVuelosAsync(VueloFiltroRequest request, CancellationToken cancellationToken = default)
    {
        // Validaciones de negocio
        if (request.IdAeropuertoOrigen > 0 && request.IdAeropuertoDestino > 0)
        {
            if (request.IdAeropuertoOrigen == request.IdAeropuertoDestino)
            {
                throw new BusinessException("El aeropuerto de origen y destino no pueden ser el mismo.", "ERR_VUELO_MISMO_ORIGEN_DESTINO");
            }
        }

        if (request.FechaSalida.Date < DateTime.UtcNow.Date)
        {
            throw new BusinessException("La fecha de salida no puede estar en el pasado.", "ERR_VUELO_FECHA_PASADA");
        }

        // Convertir Request a DataModel
        var filtroDataModel = new VueloFiltroDataModel
        {
            IdAeropuertoOrigen = request.IdAeropuertoOrigen,
            IdAeropuertoDestino = request.IdAeropuertoDestino,
            FechaSalida = request.FechaSalida,
            EstadoVuelo = request.EstadoVuelo,
            Page = request.Page,
            PageSize = request.PageSize
        };

        // Llamada a la capa de DataManagement
        var pagedResult = await _vueloDataService.BuscarVuelosAsync(filtroDataModel, cancellationToken);

        return new DataPagedResult<VueloResponse>(
            pagedResult.Items.Select(v => v.ToResponse()),
            pagedResult.TotalRegistros,
            pagedResult.PaginaActual,
            pagedResult.TamanoPagina);
    }

    public async Task<VueloResponse?> GetVueloByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        var dataModel = await _vueloDataService.GetVueloByIdAsync(id, cancellationToken);
        return dataModel?.ToResponse();
    }

    public async Task<IEnumerable<EscalaResponse>> GetEscalasAsync(int idVuelo, CancellationToken cancellationToken = default)
    {
        var dataModels = await _vueloDataService.GetEscalasAsync(idVuelo, cancellationToken);
        return dataModels.Select(e => e.ToResponse());
    }

    public async Task<IEnumerable<AsientoResponse>> GetAsientosAsync(int idVuelo, bool? disponible, string? clase, CancellationToken cancellationToken = default)
    {
        var dataModels = await _vueloDataService.GetAsientosAsync(idVuelo, disponible, clase, cancellationToken);
        return dataModels.Select(a => a.ToResponse());
    }
}
