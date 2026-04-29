using Microservicio.Vuelos.DataAccess.Entities;
using Microservicio.Vuelos.DataManagement.Interfaces;
using Microservicio.Vuelos.DataManagement.Mappers;
using Microservicio.Vuelos.DataManagement.Models.Boleto;

namespace Microservicio.Vuelos.DataManagement.Services;

public class BoletoDataService : IBoletoDataService
{
    private readonly IUnitOfWork _unitOfWork;

    public BoletoDataService(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<BoletoDataModel> EmitirBoletoAsync(int idReserva, CancellationToken cancellationToken = default)
    {
        var randomDigits = new Random().Next(100000, 999999);
        var numeroBoleto = $"ETKT-{randomDigits}";

        var entity = new BoletoEntity
        {
            Id_reserva = idReserva,
            Codigo_boleto = numeroBoleto,
            Fecha_emision = DateTime.UtcNow,
            Estado_boleto = "EMITIDO",
            Clase = "ECONOMY" // Default for now
        };

        await _unitOfWork.BoletoRepository.AgregarAsync(entity, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return entity.ToDataModel();
    }

    public async Task<BoletoDataModel?> GetByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        var entity = await _unitOfWork.BoletoRepository.ObtenerPorIdAsync(id, cancellationToken);
        return entity?.ToDataModel();
    }
}
