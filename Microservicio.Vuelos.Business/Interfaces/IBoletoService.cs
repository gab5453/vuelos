using Microservicio.Vuelos.Business.DTOs.Boleto;

namespace Microservicio.Vuelos.Business.Interfaces;

public interface IBoletoService
{
    Task<BoletoResponse> EmitirBoletoAsync(EmitirBoletoRequest request, CancellationToken cancellationToken = default);
    Task<BoletoResponse?> GetBoletoByIdAsync(int id, CancellationToken cancellationToken = default);
}
