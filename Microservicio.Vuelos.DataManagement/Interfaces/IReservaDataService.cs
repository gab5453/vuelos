using Microservicio.Vuelos.DataManagement.Models.Reserva;
using Microservicio.Vuelos.DataManagement.Models;

namespace Microservicio.Vuelos.DataManagement.Interfaces;

public interface IReservaDataService
{
    Task<ReservaDataModel> CrearReservaAsync(int idCliente, int idVuelo, int idAsiento, CancellationToken cancellationToken = default);
    Task<ReservaDataModel?> GetByIdAsync(int id, CancellationToken cancellationToken = default);
    Task<ReservaDataModel> ActualizarEstadoAsync(int id, string estado, string? motivo, CancellationToken cancellationToken = default);

    Task<DataPagedResult<ReservaDataModel>> BuscarReservasAsync(ReservaFiltroDataModel filtro, CancellationToken cancellationToken = default);
}
