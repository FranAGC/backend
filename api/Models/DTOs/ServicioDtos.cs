using System.ComponentModel.DataAnnotations;

namespace api.Models.DTOs;

// ── Requests ──────────────────────────────────────────────────────────────────

/// <summary>
/// Crea un servicio (Cable o Internet) junto con su subtipo.
/// </summary>
public class ServicioCreateRequest
{
    [Required(ErrorMessage = "El código de servicio es requerido")]
    [MaxLength(20)]
    public string CodigoServicio { get; set; } = string.Empty;

    [Required(ErrorMessage = "El ID de cliente es requerido")]
    public int IdCliente { get; set; }

    [Required(ErrorMessage = "El tipo de servicio es requerido")]
    [RegularExpression("^(Cable|Internet)$", ErrorMessage = "TipoServicio debe ser 'Cable' o 'Internet'")]
    public string TipoServicio { get; set; } = string.Empty;

    [Required(ErrorMessage = "El costo mensual es requerido")]
    [Range(0.01, 999999.99)]
    public decimal CostoMensualBase { get; set; }

    [Required(ErrorMessage = "El lugar de instalación es requerido")]
    public string LugarInstalacion { get; set; } = string.Empty;

    // ── Cable fields ──────────────────────────────────────────────
    public string? DireccionInstalacion { get; set; }

    [RegularExpression("^(Basico|Premium)$", ErrorMessage = "PlanCanales debe ser 'Basico' o 'Premium'")]
    public string? PlanCanales { get; set; }

    // ── Internet fields ───────────────────────────────────────────
    public int? VelocidadMbps { get; set; }
}

public class ServicioUpdateRequest
{
    [Range(0.01, 999999.99)]
    public decimal? CostoMensualBase { get; set; }

    public string? LugarInstalacion { get; set; }
}

// ── Sub-type requests ─────────────────────────────────────────────────────────

public class ServicioCableUpdateRequest
{
    public string? DireccionInstalacion { get; set; }

    [RegularExpression("^(Basico|Premium)$", ErrorMessage = "PlanCanales debe ser 'Basico' o 'Premium'")]
    public string? PlanCanales { get; set; }
}

public class ServicioInternetUpdateRequest
{
    [Range(1, 100000)]
    public int? VelocidadMbps { get; set; }

    [Range(0, int.MaxValue)]
    public int? MesesConsecutivosPagos { get; set; }
}

// ── Responses ─────────────────────────────────────────────────────────────────

public class ServicioResponse
{
    public int IdServicio { get; set; }
    public string CodigoServicio { get; set; } = string.Empty;
    public int IdCliente { get; set; }
    public string TipoServicio { get; set; } = string.Empty;
    public decimal CostoMensualBase { get; set; }
    public string LugarInstalacion { get; set; } = string.Empty;
    public DateOnly FechaContratacion { get; set; }
    public string Estado { get; set; } = string.Empty;

    // Subtipo (null si no aplica)
    public ServicioCableResponse? Cable { get; set; }
    public ServicioInternetResponse? Internet { get; set; }
}

public class ServicioCableResponse
{
    public int IdServicio { get; set; }
    public string DireccionInstalacion { get; set; } = string.Empty;
    public string PlanCanales { get; set; } = string.Empty;
    public string Estado { get; set; } = string.Empty;
    public string CodigoServicio { get; set; } = string.Empty;
    public string EstadoServicioPadre { get; set; } = string.Empty;
}

public class ServicioInternetResponse
{
    public int IdServicio { get; set; }
    public int VelocidadMbps { get; set; }
    public int MesesConsecutivosPagos { get; set; }
    public string Estado { get; set; } = string.Empty;
    public string CodigoServicio { get; set; } = string.Empty;
    public string EstadoServicioPadre { get; set; } = string.Empty;
}
