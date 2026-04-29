using Microservicio.Vuelos.DataManagement.Interfaces;
using Microservicio.Vuelos.DataManagement.Mappers;
using Microservicio.Vuelos.DataManagement.Models.Pasajero;

namespace Microservicio.Vuelos.DataManagement.Services;

public class PasajeroDataService : IPasajeroDataService
{
    private readonly IUnitOfWork _unitOfWork;

    public PasajeroDataService(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<PasajeroDataModel> CreatePasajeroAsync(PasajeroDataModel model, CancellationToken cancellationToken = default)
    {
        var entity = model.ToEntity();
        await _unitOfWork.PasajeroRepository.AgregarAsync(entity, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
        return entity.ToDataModel();
    }

    public async Task<PasajeroDataModel?> GetPasajeroByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        var entity = await _unitOfWork.PasajeroRepository.ObtenerPorIdAsync(id, cancellationToken);
        return entity?.ToDataModel();
    }
}
