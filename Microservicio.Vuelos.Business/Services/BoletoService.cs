using Microservicio.Vuelos.Business.DTOs.Boleto;
using Microservicio.Vuelos.Business.Interfaces;
using Microservicio.Vuelos.Business.Mappers;
using Microservicio.Vuelos.DataManagement.Interfaces;

namespace Microservicio.Vuelos.Business.Services;

public class BoletoService : IBoletoService
{
    private readonly IBoletoDataService _boletoDataService;

    public BoletoService(IBoletoDataService boletoDataService)
    {
        _boletoDataService = boletoDataService;
    }

    public async Task<BoletoResponse> EmitirBoletoAsync(EmitirBoletoRequest request, CancellationToken cancellationToken = default)
    {
        // En una implementación real, aquí se pasarían todos los campos del request al DataService
        var dataModel = await _boletoDataService.EmitirBoletoAsync(request.IdReserva, cancellationToken);
        
        // Mapear campos adicionales que vienen en el request pero tal vez no se procesaron en el DataService aún
        dataModel.IdVuelo = request.IdVuelo;
        dataModel.IdAsiento = request.IdAsiento;
        dataModel.IdFactura = request.IdFactura;
        dataModel.Clase = request.Clase;
        dataModel.PrecioVueloBase = request.PrecioVueloBase;
        dataModel.PrecioAsientoExtra = request.PrecioAsientoExtra;
        dataModel.ImpuestosBoleto = request.ImpuestosBoleto;
        dataModel.CargoEquipaje = request.CargoEquipaje;
        dataModel.PrecioFinal = request.PrecioFinal;

        return dataModel.ToResponse();
    }

    public async Task<BoletoResponse?> GetBoletoByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        var dataModel = await _boletoDataService.GetByIdAsync(id, cancellationToken);
        return dataModel?.ToResponse();
    }
}
