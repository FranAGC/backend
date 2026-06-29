using api.Models.Domain;
using Microsoft.EntityFrameworkCore;

namespace api.Data.Repositories;

public class ClienteRepository : BaseRepository<Cliente>, IClienteRepository
{
    public ClienteRepository(AppDbContext context) : base(context) { }

    public async Task<Cliente?> GetByCodigoAsync(string codigo) =>
        await _dbSet.FirstOrDefaultAsync(c => c.Codigo == codigo);

    public async Task<Cliente?> GetByCorreoAsync(string correo) =>
        await _dbSet.FirstOrDefaultAsync(c => c.Correo == correo);

    public async Task<IEnumerable<Cliente>> GetAllWithServiciosAsync() =>
        await _dbSet
            .Include(c => c.Servicios)
                .ThenInclude(s => s.ServicioCable)
            .Include(c => c.Servicios)
                .ThenInclude(s => s.ServicioInternet)
            .ToListAsync();

    public async Task<Cliente?> GetByIdWithServiciosAsync(int id) =>
        await _dbSet
            .Include(c => c.Servicios)
                .ThenInclude(s => s.ServicioCable)
            .Include(c => c.Servicios)
                .ThenInclude(s => s.ServicioInternet)
            .FirstOrDefaultAsync(c => c.IdCliente == id);
}
