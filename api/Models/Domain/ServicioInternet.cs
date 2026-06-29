// Domain model for ServicioInternet entity — maps to 'servicios_internet' table
namespace api.Models.Domain;

public class ServicioInternet
{
    public int IdServicio { get; set; }
    public int VelocidadMbps { get; set; }
    public int MesesConsecutivosPagos { get; set; } = 0;
    public string Estado { get; set; } = "Activo"; // Activo | Suspendido

    public Servicio Servicio { get; set; } = null!;
}
