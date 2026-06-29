using api.Models.DTOs;

namespace api.Services.Interfaces;

public interface IClienteService
{
    Task<IEnumerable<ClienteResponse>> GetAllAsync();
    Task<ClienteResponse?> GetByIdAsync(int id);
    Task<ClienteResponse> CreateAsync(ClienteCreateRequest request);
    Task<ClienteResponse?> UpdateAsync(int id, ClienteUpdateRequest request);
    Task<bool> SoftDeleteAsync(int id); // sets estado = "Suspendido"
}
