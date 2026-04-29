namespace Microservicio.Vuelos.DataManagement.Services;

using Microservicio.Vuelos.DataManagement.Models.Vuelo;
using Microservicio.Vuelos.DataManagement.Interfaces;
using Microservicio.Vuelos.DataManagement.Mappers;
using Microservicio.Vuelos.DataManagement.Models;

public class VueloDataService : IVueloDataService
{
    private readonly IUnitOfWork _unitOfWork;

    public VueloDataService(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<DataPagedResult<VueloDataModel>> BuscarVuelosAsync(VueloFiltroDataModel filtro, CancellationToken cancellationToken = default)
    {
        var pagedResult = await _unitOfWork.VueloRepository.BuscarVuelosAsync(
            filtro.IdAeropuertoOrigen,
            filtro.IdAeropuertoDestino,
            filtro.FechaSalida,
            filtro.Page,
            filtro.PageSize,
            cancellationToken);

        return new DataPagedResult<VueloDataModel>(
            pagedResult.Items.Select(v => v.ToDataModel()),
            pagedResult.TotalRegistros,
            pagedResult.PaginaActual,
            pagedResult.TamanoPagina);
    }

    public async Task<VueloDataModel?> GetVueloByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        var vuelo = await _unitOfWork.VueloRepository.ObtenerPorIdAsync(id, cancellationToken);
        return vuelo?.ToDataModel();
    }

    public async Task<IEnumerable<EscalaDataModel>> GetEscalasAsync(int idVuelo, CancellationToken cancellationToken = default)
    {
        var escalas = await _unitOfWork.EscalaRepository.ObtenerTodosAsync(cancellationToken);
        return escalas.Where(e => e.Id_vuelo == idVuelo).OrderBy(e => e.Orden).Select(e => e.ToDataModel());
    }

    public async Task<IEnumerable<AsientoDataModel>> GetAsientosAsync(int idVuelo, bool? disponible, string? clase, CancellationToken cancellationToken = default)
    {
        var asientos = await _unitOfWork.AsientoRepository.ObtenerTodosAsync(cancellationToken);
        var query = asientos.Where(a => a.Id_vuelo == idVuelo);
        
        if (disponible.HasValue)
            query = query.Where(a => a.Disponible == disponible.Value);
            
        if (!string.IsNullOrEmpty(clase))
            query = query.Where(a => a.Clase.Equals(clase, StringComparison.OrdinalIgnoreCase));

        return query.Select(a => a.ToDataModel());
    }
}
