using Microservicio.Vuelos.DataManagement.Models.Equipaje;

namespace Microservicio.Vuelos.DataManagement.Interfaces;

public interface IEquipajeDataService
{
    Task<EquipajeDataModel> RegistrarEquipajeAsync(EquipajeDataModel model, CancellationToken cancellationToken = default);
    Task<IEnumerable<EquipajeDataModel>> GetEquipajeByBoletoAsync(int idBoleto, CancellationToken cancellationToken = default);
}
