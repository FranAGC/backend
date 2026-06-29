// Domain model for Cliente entity — maps to 'clientes' table
namespace api.Models.Domain;

public class Cliente
{
    public int IdCliente { get; set; }
    public string Codigo { get; set; } = string.Empty;
    public string Nombre { get; set; } = string.Empty;
    public DateOnly FechaAlta { get; set; }
    public string Direccion { get; set; } = string.Empty;
    public string Correo { get; set; } = string.Empty;
    public string Telefono { get; set; } = string.Empty;
    public string Estado { get; set; } = "Activo"; // Activo | Suspendido | Cancelado
    public DateTime CreadoEn { get; set; }

    public ICollection<Servicio> Servicios { get; set; } = new List<Servicio>();
}
