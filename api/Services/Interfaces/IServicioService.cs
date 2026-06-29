using api.Models.DTOs;

namespace api.Services.Interfaces;

public interface IServicioService
{
    // Servicios (padre)
    Task<IEnumerable<ServicioResponse>> GetAllAsync();
    Task<ServicioResponse?> GetByIdAsync(int id);
    Task<IEnumerable<ServicioResponse>> GetByClienteIdAsync(int clienteId);
    Task<ServicioResponse> CreateAsync(ServicioCreateRequest request);
    Task<ServicioResponse?> UpdateAsync(int id, ServicioUpdateRequest request);
    Task<bool> SoftDeleteAsync(int id); // sets estado = "Suspendido" in servicios + subtipo

    // Cable
    Task<IEnumerable<ServicioCableResponse>> GetAllCableAsync();
    Task<ServicioCableResponse?> GetCableByIdAsync(int id);
    Task<ServicioCableResponse?> UpdateCableAsync(int id, ServicioCableUpdateRequest request);
    Task<bool> SoftDeleteCableAsync(int id);

    // Internet
    Task<IEnumerable<ServicioInternetResponse>> GetAllInternetAsync();
    Task<ServicioInternetResponse?> GetInternetByIdAsync(int id);
    Task<ServicioInternetResponse?> UpdateInternetAsync(int id, ServicioInternetUpdateRequest request);
    Task<bool> SoftDeleteInternetAsync(int id);
}
