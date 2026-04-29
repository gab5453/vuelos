using Microservicio.Vuelos.DataManagement.Models.Boleto;

namespace Microservicio.Vuelos.DataManagement.Interfaces;

public interface IBoletoDataService
{
    Task<BoletoDataModel> EmitirBoletoAsync(int idReserva, CancellationToken cancellationToken = default);
    Task<BoletoDataModel?> GetByIdAsync(int id, CancellationToken cancellationToken = default);
}
