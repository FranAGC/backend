// Domain model for Usuario entity — maps to 'usuarios' table
namespace api.Models.Domain;

public class Usuario
{
    public int IdUsuario { get; set; }
    public int? IdEmpleado { get; set; }
    public int IdRol { get; set; }
    public string Username { get; set; } = string.Empty;
    public string PasswordHash { get; set; } = string.Empty;
    public string Estado { get; set; } = "Activo"; // Activo | Bloqueado
    public DateTime CreadoEn { get; set; }

    public Rol Rol { get; set; } = null!;
}
