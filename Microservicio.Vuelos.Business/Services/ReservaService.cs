using Microservicio.Vuelos.Business.DTOs.Reserva;
using Microservicio.Vuelos.Business.Exceptions;
using Microservicio.Vuelos.Business.Interfaces;
using Microservicio.Vuelos.DataManagement.Interfaces;
using Microservicio.Vuelos.DataManagement.Models;
using Microservicio.Vuelos.DataManagement.Models.Reserva;

namespace Microservicio.Vuelos.Business.Services;

public class ReservaService : IReservaService
{
    private readonly IReservaDataService _reservaDataService;
    private readonly IUnitOfWork _unitOfWork;

    public ReservaService(IReservaDataService reservaDataService, IUnitOfWork unitOfWork)
    {
        _reservaDataService = reservaDataService;
        _unitOfWork = unitOfWork;
    }

    public async Task<ReservaResponse> CrearReservaAsync(CrearReservaRequest request, CancellationToken cancellationToken = default)
    {
        // a. Validar que el Vuelo exista y esté "PROGRAMADO"
        var vuelo = await _unitOfWork.VueloRepository.ObtenerPorIdAsync(request.IdVuelo, cancellationToken);
        if (vuelo == null)
        {
            throw new NotFoundException("El vuelo solicitado no existe.");
        }
        
        if (vuelo.Estado_vuelo != "PROGRAMADO")
        {
            throw new BusinessException("El vuelo no está disponible para reserva, su estado no es PROGRAMADO.", "ERR_VUELO_NO_PROGRAMADO");
        }

        // b. Validar que el Asiento pertenezca a ese Vuelo y esté Disponible
        var asiento = await _unitOfWork.AsientoRepository.ObtenerPorIdAsync(request.IdAsiento, cancellationToken);
        if (asiento == null || asiento.Id_vuelo != request.IdVuelo)
        {
            throw new BusinessException("El asiento solicitado no pertenece al vuelo especificado o no existe.", "ERR_ASIENTO_INVALIDO");
        }

        if (!asiento.Disponible)
        {
            throw new BusinessException("El asiento seleccionado ya está ocupado.", "ERR_ASIENTO_OCUPADO");
        }

        // Llamada a la capa de DataManagement para procesar la transacción
        var reservaDataModel = await _reservaDataService.CrearReservaAsync(request.IdCliente, request.IdVuelo, request.IdAsiento, cancellationToken);

        // Retornar el response mapeado
        return new ReservaResponse
        {
            IdReserva = reservaDataModel.IdReserva,
            CodigoReserva = reservaDataModel.CodigoReserva,
            IdCliente = reservaDataModel.IdCliente,
            IdVuelo = reservaDataModel.IdVuelo,
            EstadoReserva = reservaDataModel.EstadoReserva,
            FechaReservaUtc = reservaDataModel.FechaReservaUtc
        };
    }

    public async Task<ReservaResponse> ActualizarEstadoAsync(int id, ActualizarEstadoReservaRequest request, CancellationToken cancellationToken = default)
    {
        var result = await _reservaDataService.ActualizarEstadoAsync(id, request.EstadoReserva, request.MotivoCancelacion, cancellationToken);
        
        return new ReservaResponse
        {
            IdReserva = result.IdReserva,
            CodigoReserva = result.CodigoReserva,
            IdCliente = result.IdCliente,
            IdVuelo = result.IdVuelo,
            EstadoReserva = result.EstadoReserva,
            FechaReservaUtc = result.FechaReservaUtc
        };
    }

    public async Task<DataPagedResult<ReservaResponse>> BuscarReservasAsync(ReservaFiltroRequest request, CancellationToken cancellationToken = default)
    {
        var filtro = new ReservaFiltroDataModel
        {
            IdCliente = request.IdCliente,
            Estado = request.Estado,
            Page = request.Page,
            PageSize = request.PageSize
        };

        var pagedResult = await _reservaDataService.BuscarReservasAsync(filtro, cancellationToken);

        return new DataPagedResult<ReservaResponse>(
            pagedResult.Items.Select(r => new ReservaResponse
            {
                IdReserva = r.IdReserva,
                CodigoReserva = r.CodigoReserva,
                IdCliente = r.IdCliente,
                IdVuelo = r.IdVuelo,
                EstadoReserva = r.EstadoReserva,
                FechaReservaUtc = r.FechaReservaUtc
            }),
            pagedResult.TotalRegistros,
            pagedResult.PaginaActual,
            pagedResult.TamanoPagina);
    }
}
