using Microservicio.Vuelos.DataAccess.Entities;
using Microservicio.Vuelos.DataManagement.Interfaces;
using Microservicio.Vuelos.DataManagement.Mappers;
using Microservicio.Vuelos.DataManagement.Models.Equipaje;

namespace Microservicio.Vuelos.DataManagement.Services;

public class EquipajeDataService : IEquipajeDataService
{
    private readonly IUnitOfWork _unitOfWork;

    public EquipajeDataService(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<EquipajeDataModel> RegistrarEquipajeAsync(EquipajeDataModel model, CancellationToken cancellationToken = default)
    {
        var entity = model.ToEntity();
        await _unitOfWork.EquipajeRepository.AgregarAsync(entity, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
        return entity.ToDataModel();
    }

    public async Task<IEnumerable<EquipajeDataModel>> GetEquipajeByBoletoAsync(int idBoleto, CancellationToken cancellationToken = default)
    {
        var entities = await _unitOfWork.EquipajeRepository.ObtenerTodosAsync(cancellationToken);
        return entities.Where(e => e.Id_boleto == idBoleto).Select(e => e.ToDataModel());
    }
}
