using Microservicio.Vuelos.DataManagement.Interfaces;
using Microservicio.Vuelos.DataManagement.Models.Reserva;
using Microservicio.Vuelos.DataManagement.Mappers;
using Microservicio.Vuelos.DataAccess.Entities;
using Microservicio.Vuelos.DataManagement.Models;

namespace Microservicio.Vuelos.DataManagement.Services;

public class ReservaDataService : IReservaDataService
{
    private readonly IUnitOfWork _unitOfWork;

    public ReservaDataService(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<ReservaDataModel> CrearReservaAsync(int idCliente, int idVuelo, int idAsiento, CancellationToken cancellationToken = default)
    {
        // a. Genera el código de reserva
        var randomDigits = new Random().Next(1000, 10000);
        var codigoReserva = $"VU-{DateTime.UtcNow.Year}-{randomDigits}";

        // b. Crea la entidad ReservaEntity
        var reservaEntity = new ReservaEntity
        {
            Id_cliente = idCliente,
            Id_vuelo = idVuelo,
            Codigo_reserva = codigoReserva,
            Estado_reserva = "PEN",
            Fecha_reserva_utc = DateTime.UtcNow
        };

        // c. Usa _unitOfWork.ReservaRepository.AgregarAsync()
        await _unitOfWork.ReservaRepository.AgregarAsync(reservaEntity, cancellationToken);

        // d. Busca el asiento y cambia su propiedad Disponible = false
        var asiento = await _unitOfWork.AsientoRepository.ObtenerPorIdAsync(idAsiento, cancellationToken);
        if (asiento != null)
        {
            asiento.Disponible = false;
            _unitOfWork.AsientoRepository.Actualizar(asiento);
        }

        // e. Ejecuta el guardado transaccional
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return reservaEntity.ToDataModel();
    }

    public async Task<ReservaDataModel?> GetByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        var entity = await _unitOfWork.ReservaRepository.ObtenerPorIdAsync(id, cancellationToken);
        return entity?.ToDataModel();
    }

    public async Task<ReservaDataModel> ActualizarEstadoAsync(int id, string estado, string? motivo, CancellationToken cancellationToken = default)
    {
        var entity = await _unitOfWork.ReservaRepository.ObtenerPorIdAsync(id, cancellationToken);
        if (entity == null) throw new KeyNotFoundException("Reserva no encontrada");

        entity.Estado_reserva = estado;
        // NOTE: If we had a Motivo column, we would update it here.
        
        _unitOfWork.ReservaRepository.Actualizar(entity);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
        
        return entity.ToDataModel();
    }

    public async Task<DataPagedResult<ReservaDataModel>> BuscarReservasAsync(ReservaFiltroDataModel filtro, CancellationToken cancellationToken = default)
    {
        var pagedResult = await _unitOfWork.ReservaRepository.BuscarReservasAsync(
            filtro.IdCliente,
            filtro.Estado,
            filtro.Page,
            filtro.PageSize,
            cancellationToken);

        return new DataPagedResult<ReservaDataModel>(
            pagedResult.Items.Select(r => r.ToDataModel()),
            pagedResult.TotalRegistros,
            pagedResult.PaginaActual,
            pagedResult.TamanoPagina);
    }
}
