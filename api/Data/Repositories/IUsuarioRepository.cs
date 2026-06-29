using api.Models.Domain;

namespace api.Data.Repositories;

public interface IUsuarioRepository : IBaseRepository<Usuario>
{
    Task<Usuario?> GetByUsernameAsync(string username);
}
