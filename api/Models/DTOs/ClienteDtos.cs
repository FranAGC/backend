using System.ComponentModel.DataAnnotations;

namespace api.Models.DTOs;

// ── Requests ──────────────────────────────────────────────────────────────────

public class ClienteCreateRequest
{
    [Required(ErrorMessage = "El código es requerido")]
    [MaxLength(20)]
    public string Codigo { get; set; } = string.Empty;

    [Required(ErrorMessage = "El nombre es requerido")]
    [MaxLength(150)]
    public string Nombre { get; set; } = string.Empty;

    [Required(ErrorMessage = "La dirección es requerida")]
    public string Direccion { get; set; } = string.Empty;

    [Required(ErrorMessage = "El correo es requerido")]
    [EmailAddress]
    [MaxLength(255)]
    public string Correo { get; set; } = string.Empty;

    [Required(ErrorMessage = "El teléfono es requerido")]
    [MaxLength(20)]
    public string Telefono { get; set; } = string.Empty;
}

public class ClienteUpdateRequest
{
    [MaxLength(150)]
    public string? Nombre { get; set; }

    public string? Direccion { get; set; }

    [EmailAddress]
    [MaxLength(255)]
    public string? Correo { get; set; }

    [MaxLength(20)]
    public string? Telefono { get; set; }
}

// ── Response ─────────────────────────────────────────────────────────────────

public class ClienteResponse
{
    public int IdCliente { get; set; }
    public string Codigo { get; set; } = string.Empty;
    public string Nombre { get; set; } = string.Empty;
    public DateOnly FechaAlta { get; set; }
    public string Direccion { get; set; } = string.Empty;
    public string Correo { get; set; } = string.Empty;
    public string Telefono { get; set; } = string.Empty;
    public string Estado { get; set; } = string.Empty;
    public DateTime CreadoEn { get; set; }
    public List<ServicioResponse> Servicios { get; set; } = new();
}
