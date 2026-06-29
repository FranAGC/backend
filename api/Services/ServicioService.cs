using api.Data.Repositories;
using api.Models.Domain;
using api.Models.DTOs;
using api.Services.Interfaces;

namespace api.Services;

public class ServicioService : IServicioService
{
    private readonly IServicioRepository _repo;
    private readonly ILogger<ServicioService> _logger;

    public ServicioService(IServicioRepository repo, ILogger<ServicioService> logger)
    {
        _repo = repo;
        _logger = logger;
    }

    // ── Servicios ─────────────────────────────────────────────────────────────

    public async Task<IEnumerable<ServicioResponse>> GetAllAsync()
    {
        _logger.LogInformation("Obteniendo todos los servicios");
        var servicios = await _repo.GetAllWithSubtiposAsync();
        return servicios.Select(MapServicio);
    }

    public async Task<ServicioResponse?> GetByIdAsync(int id)
    {
        _logger.LogInformation("Obteniendo servicio ID: {Id}", id);
        var s = await _repo.GetByIdWithSubtipoAsync(id);
        return s is null ? null : MapServicio(s);
    }

    public async Task<IEnumerable<ServicioResponse>> GetByClienteIdAsync(int clienteId)
    {
        _logger.LogInformation("Obteniendo servicios del cliente ID: {Id}", clienteId);
        var servicios = await _repo.GetByClienteIdAsync(clienteId);
        return servicios.Select(MapServicio);
    }

    public async Task<ServicioResponse> CreateAsync(ServicioCreateRequest request)
    {
        _logger.LogInformation("Creando servicio: {Codigo}", request.CodigoServicio);

        if (await _repo.GetByCodigoAsync(request.CodigoServicio) is not null)
            throw new InvalidOperationException($"Ya existe un servicio con el código '{request.CodigoServicio}'");

        var servicio = new Servicio
        {
            CodigoServicio = request.CodigoServicio,
            IdCliente = request.IdCliente,
            TipoServicio = request.TipoServicio,
            CostoMensualBase = request.CostoMensualBase,
            LugarInstalacion = request.LugarInstalacion,
            FechaContratacion = DateOnly.FromDateTime(DateTime.Now),
            Estado = "Activo"
        };

        var created = await _repo.AddAsync(servicio);

        // Create sub-type record
        if (request.TipoServicio == "Cable")
        {
            var cable = new ServicioCable
            {
                IdServicio = created.IdServicio,
                DireccionInstalacion = request.DireccionInstalacion
                    ?? throw new InvalidOperationException("La dirección de instalación es requerida para Cable"),
                PlanCanales = request.PlanCanales ?? "Basico",
                Estado = "Activo"
            };
            await _repo.AddCableAsync(cable);
            created.ServicioCable = cable;
        }
        else if (request.TipoServicio == "Internet")
        {
            var internet = new ServicioInternet
            {
                IdServicio = created.IdServicio,
                VelocidadMbps = request.VelocidadMbps
                    ?? throw new InvalidOperationException("La velocidad Mbps es requerida para Internet"),
                MesesConsecutivosPagos = 0,
                Estado = "Activo"
            };
            await _repo.AddInternetAsync(internet);
            created.ServicioInternet = internet;
        }

        _logger.LogInformation("Servicio creado ID: {Id}", created.IdServicio);
        return MapServicio(created);
    }

    public async Task<ServicioResponse?> UpdateAsync(int id, ServicioUpdateRequest request)
    {
        _logger.LogInformation("Actualizando servicio ID: {Id}", id);
        var servicio = await _repo.GetByIdWithSubtipoAsync(id);
        if (servicio is null) return null;

        if (request.CostoMensualBase.HasValue)
            servicio.CostoMensualBase = request.CostoMensualBase.Value;

        if (!string.IsNullOrWhiteSpace(request.LugarInstalacion))
            servicio.LugarInstalacion = request.LugarInstalacion;

        await _repo.UpdateAsync(servicio);
        _logger.LogInformation("Servicio actualizado ID: {Id}", id);
        return MapServicio(servicio);
    }

    public async Task<bool> SoftDeleteAsync(int id)
    {
        _logger.LogInformation("Soft-delete servicio ID: {Id}", id);
        var servicio = await _repo.GetByIdWithSubtipoAsync(id);
        if (servicio is null) return false;

        servicio.Estado = "Suspendido";

        if (servicio.ServicioCable is not null)
            servicio.ServicioCable.Estado = "Suspendido";

        if (servicio.ServicioInternet is not null)
            servicio.ServicioInternet.Estado = "Suspendido";

        await _repo.UpdateAsync(servicio);
        _logger.LogInformation("Servicio suspendido ID: {Id}", id);
        return true;
    }

    // ── Cable ─────────────────────────────────────────────────────────────────

    public async Task<IEnumerable<ServicioCableResponse>> GetAllCableAsync()
    {
        var cables = await _repo.GetAllCableAsync();
        return cables.Select(MapCable);
    }

    public async Task<ServicioCableResponse?> GetCableByIdAsync(int id)
    {
        var cable = await _repo.GetCableByIdAsync(id);
        return cable is null ? null : MapCable(cable);
    }

    public async Task<ServicioCableResponse?> UpdateCableAsync(int id, ServicioCableUpdateRequest request)
    {
        _logger.LogInformation("Actualizando cable ID: {Id}", id);
        var cable = await _repo.GetCableByIdAsync(id);
        if (cable is null) return null;

        if (!string.IsNullOrWhiteSpace(request.DireccionInstalacion))
            cable.DireccionInstalacion = request.DireccionInstalacion;

        if (!string.IsNullOrWhiteSpace(request.PlanCanales))
            cable.PlanCanales = request.PlanCanales;

        await _repo.SaveChangesAsync();
        return MapCable(cable);
    }

    public async Task<bool> SoftDeleteCableAsync(int id)
    {
        _logger.LogInformation("Soft-delete cable ID: {Id}", id);
        var cable = await _repo.GetCableByIdAsync(id);
        if (cable is null) return false;

        cable.Estado = "Suspendido";
        cable.Servicio.Estado = "Suspendido";
        await _repo.SaveChangesAsync();
        return true;
    }

    // ── Internet ──────────────────────────────────────────────────────────────

    public async Task<IEnumerable<ServicioInternetResponse>> GetAllInternetAsync()
    {
        var internets = await _repo.GetAllInternetAsync();
        return internets.Select(MapInternet);
    }

    public async Task<ServicioInternetResponse?> GetInternetByIdAsync(int id)
    {
        var internet = await _repo.GetInternetByIdAsync(id);
        return internet is null ? null : MapInternet(internet);
    }

    public async Task<ServicioInternetResponse?> UpdateInternetAsync(int id, ServicioInternetUpdateRequest request)
    {
        _logger.LogInformation("Actualizando internet ID: {Id}", id);
        var internet = await _repo.GetInternetByIdAsync(id);
        if (internet is null) return null;

        if (request.VelocidadMbps.HasValue)
            internet.VelocidadMbps = request.VelocidadMbps.Value;

        if (request.MesesConsecutivosPagos.HasValue)
            internet.MesesConsecutivosPagos = request.MesesConsecutivosPagos.Value;

        await _repo.SaveChangesAsync();
        return MapInternet(internet);
    }

    public async Task<bool> SoftDeleteInternetAsync(int id)
    {
        _logger.LogInformation("Soft-delete internet ID: {Id}", id);
        var internet = await _repo.GetInternetByIdAsync(id);
        if (internet is null) return false;

        internet.Estado = "Suspendido";
        internet.Servicio.Estado = "Suspendido";
        await _repo.SaveChangesAsync();
        return true;
    }

    // ── Mapping ───────────────────────────────────────────────────────────────

    private static ServicioResponse MapServicio(Servicio s) => new()
    {
        IdServicio = s.IdServicio,
        CodigoServicio = s.CodigoServicio,
        IdCliente = s.IdCliente,
        TipoServicio = s.TipoServicio,
        CostoMensualBase = s.CostoMensualBase,
        LugarInstalacion = s.LugarInstalacion,
        FechaContratacion = s.FechaContratacion,
        Estado = s.Estado,
        Cable = s.ServicioCable is null ? null : MapCable(s.ServicioCable),
        Internet = s.ServicioInternet is null ? null : MapInternet(s.ServicioInternet)
    };

    private static ServicioCableResponse MapCable(ServicioCable c) => new()
    {
        IdServicio = c.IdServicio,
        DireccionInstalacion = c.DireccionInstalacion,
        PlanCanales = c.PlanCanales,
        Estado = c.Estado,
        CodigoServicio = c.Servicio?.CodigoServicio ?? string.Empty,
        EstadoServicioPadre = c.Servicio?.Estado ?? string.Empty
    };

    private static ServicioInternetResponse MapInternet(ServicioInternet i) => new()
    {
        IdServicio = i.IdServicio,
        VelocidadMbps = i.VelocidadMbps,
        MesesConsecutivosPagos = i.MesesConsecutivosPagos,
        Estado = i.Estado,
        CodigoServicio = i.Servicio?.CodigoServicio ?? string.Empty,
        EstadoServicioPadre = i.Servicio?.Estado ?? string.Empty
    };
}
