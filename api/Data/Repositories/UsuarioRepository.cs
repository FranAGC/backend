using api.Models.Domain;
using Microsoft.EntityFrameworkCore;

namespace api.Data.Repositories;

public class UsuarioRepository : BaseRepository<Usuario>, IUsuarioRepository
{
    public UsuarioRepository(AppDbContext context) : base(context) { }

    public async Task<Usuario?> GetByUsernameAsync(string username) =>
        await _dbSet
            .Include(u => u.Rol)
            .FirstOrDefaultAsync(u => u.Username == username);
}
