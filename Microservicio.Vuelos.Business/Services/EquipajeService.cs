using Microservicio.Vuelos.Business.DTOs.Equipaje;
using Microservicio.Vuelos.Business.Interfaces;
using Microservicio.Vuelos.DataManagement.Interfaces;
using Microservicio.Vuelos.DataManagement.Models.Equipaje;

namespace Microservicio.Vuelos.Business.Services;

public class EquipajeService : IEquipajeService
{
    private readonly IEquipajeDataService _equipajeDataService;

    public EquipajeService(IEquipajeDataService equipajeDataService)
    {
        _equipajeDataService = equipajeDataService;
    }

    public async Task<EquipajeResponse> RegistrarEquipajeAsync(int idBoleto, RegistrarEquipajeRequest request, CancellationToken cancellationToken = default)
    {
        var model = new EquipajeDataModel
        {
            IdBoleto = idBoleto,
            Tipo = request.Tipo,
            PesoKg = request.PesoKg
        };

        var result = await _equipajeDataService.RegistrarEquipajeAsync(model, cancellationToken);

        return new EquipajeResponse
        {
            IdEquipaje = result.IdEquipaje,
            IdBoleto = result.IdBoleto,
            Tipo = result.Tipo,
            PesoKg = result.PesoKg
        };
    }

    public async Task<IEnumerable<EquipajeResponse>> GetEquipajeByBoletoAsync(int idBoleto, CancellationToken cancellationToken = default)
    {
        var results = await _equipajeDataService.GetEquipajeByBoletoAsync(idBoleto, cancellationToken);
        return results.Select(r => new EquipajeResponse
        {
            IdEquipaje = r.IdEquipaje,
            IdBoleto = r.IdBoleto,
            Tipo = r.Tipo,
            PesoKg = r.PesoKg
        });
    }
}
