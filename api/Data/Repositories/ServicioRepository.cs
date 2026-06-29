using api.Models.Domain;
using Microsoft.EntityFrameworkCore;

namespace api.Data.Repositories;

public class ServicioRepository : BaseRepository<Servicio>, IServicioRepository
{
    private readonly AppDbContext _ctx;

    public ServicioRepository(AppDbContext context) : base(context)
    {
        _ctx = context;
    }

    public async Task<Servicio?> GetByCodigoAsync(string codigo) =>
        await _dbSet.FirstOrDefaultAsync(s => s.CodigoServicio == codigo);

    public async Task<IEnumerable<Servicio>> GetByClienteIdAsync(int clienteId) =>
        await _dbSet
            .Include(s => s.ServicioCable)
            .Include(s => s.ServicioInternet)
            .Where(s => s.IdCliente == clienteId)
            .ToListAsync();

    public async Task<Servicio?> GetByIdWithSubtipoAsync(int id) =>
        await _dbSet
            .Include(s => s.ServicioCable)
            .Include(s => s.ServicioInternet)
            .FirstOrDefaultAsync(s => s.IdServicio == id);

    public async Task<IEnumerable<Servicio>> GetAllWithSubtiposAsync() =>
        await _dbSet
            .Include(s => s.ServicioCable)
            .Include(s => s.ServicioInternet)
            .ToListAsync();

    // ── Cable ──────────────────────────────────────────────────────
    public async Task<ServicioCable?> GetCableByIdAsync(int idServicio) =>
        await _ctx.ServiciosCable
            .Include(sc => sc.Servicio)
            .FirstOrDefaultAsync(sc => sc.IdServicio == idServicio);

    public async Task<IEnumerable<ServicioCable>> GetAllCableAsync() =>
        await _ctx.ServiciosCable
            .Include(sc => sc.Servicio)
            .ToListAsync();

    public async Task AddCableAsync(ServicioCable cable)
    {
        await _ctx.ServiciosCable.AddAsync(cable);
        await _ctx.SaveChangesAsync();
    }

    // ── Internet ──────────────────────────────────────────────────
    public async Task<ServicioInternet?> GetInternetByIdAsync(int idServicio) =>
        await _ctx.ServiciosInternet
            .Include(si => si.Servicio)
            .FirstOrDefaultAsync(si => si.IdServicio == idServicio);

    public async Task<IEnumerable<ServicioInternet>> GetAllInternetAsync() =>
        await _ctx.ServiciosInternet
            .Include(si => si.Servicio)
            .ToListAsync();

    public async Task AddInternetAsync(ServicioInternet internet)
    {
        await _ctx.ServiciosInternet.AddAsync(internet);
        await _ctx.SaveChangesAsync();
    }
}
