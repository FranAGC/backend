using api.Models.Domain;

namespace api.Data.Repositories;

public interface IServicioRepository : IBaseRepository<Servicio>
{
    Task<Servicio?> GetByCodigoAsync(string codigo);
    Task<IEnumerable<Servicio>> GetByClienteIdAsync(int clienteId);
    Task<Servicio?> GetByIdWithSubtipoAsync(int id);
    Task<IEnumerable<Servicio>> GetAllWithSubtiposAsync();

    // Sub-types
    Task<ServicioCable?> GetCableByIdAsync(int idServicio);
    Task<ServicioInternet?> GetInternetByIdAsync(int idServicio);
    Task<IEnumerable<ServicioCable>> GetAllCableAsync();
    Task<IEnumerable<ServicioInternet>> GetAllInternetAsync();
    Task AddCableAsync(ServicioCable cable);
    Task AddInternetAsync(ServicioInternet internet);
}
