using Microservicio.Vuelos.Business.DTOs.Reserva;
using Microservicio.Vuelos.DataManagement.Models;

namespace Microservicio.Vuelos.Business.Interfaces;

public interface IReservaService
{
    Task<ReservaResponse> CrearReservaAsync(CrearReservaRequest request, CancellationToken cancellationToken = default);
    Task<ReservaResponse> ActualizarEstadoAsync(int id, ActualizarEstadoReservaRequest request, CancellationToken cancellationToken = default);

    Task<DataPagedResult<ReservaResponse>> BuscarReservasAsync(ReservaFiltroRequest request, CancellationToken cancellationToken = default);
}
