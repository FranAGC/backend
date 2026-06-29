// Domain model for ServicioCable entity — maps to 'servicios_cable' table
namespace api.Models.Domain;

public class ServicioCable
{
    public int IdServicio { get; set; }
    public string DireccionInstalacion { get; set; } = string.Empty;
    public string PlanCanales { get; set; } = "Basico"; // Basico | Premium
    public string Estado { get; set; } = "Activo"; // Activo | Suspendido

    public Servicio Servicio { get; set; } = null!;
}
