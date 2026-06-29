// Domain model for Servicio entity — maps to 'servicios' table
namespace api.Models.Domain;

public class Servicio
{
    public int IdServicio { get; set; }
    public string CodigoServicio { get; set; } = string.Empty;
    public int IdCliente { get; set; }
    public string TipoServicio { get; set; } = string.Empty; // Cable | Internet
    public decimal CostoMensualBase { get; set; }
    public string LugarInstalacion { get; set; } = string.Empty;
    public DateOnly FechaContratacion { get; set; }
    public string Estado { get; set; } = "Activo"; // Activo | Suspendido

    public Cliente Cliente { get; set; } = null!;
    public ServicioCable? ServicioCable { get; set; }
    public ServicioInternet? ServicioInternet { get; set; }
}
