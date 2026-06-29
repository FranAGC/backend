using api.Models.Domain;

namespace api.Data.Repositories;

public interface IClienteRepository : IBaseRepository<Cliente>
{
    Task<Cliente?> GetByCodigoAsync(string codigo);
    Task<Cliente?> GetByCorreoAsync(string correo);
    Task<IEnumerable<Cliente>> GetAllWithServiciosAsync();
    Task<Cliente?> GetByIdWithServiciosAsync(int id);
}
