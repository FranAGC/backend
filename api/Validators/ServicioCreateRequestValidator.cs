using api.Models.DTOs;
using FluentValidation;

namespace api.Validators;

public class ServicioCreateRequestValidator : AbstractValidator<ServicioCreateRequest>
{
    public ServicioCreateRequestValidator()
    {
        RuleFor(x => x.CodigoServicio)
            .NotEmpty().WithMessage("El código de servicio es requerido")
            .MaximumLength(20);

        RuleFor(x => x.IdCliente)
            .GreaterThan(0).WithMessage("El ID de cliente debe ser válido");

        RuleFor(x => x.TipoServicio)
            .NotEmpty()
            .Must(t => t == "Cable" || t == "Internet")
            .WithMessage("TipoServicio debe ser 'Cable' o 'Internet'");

        RuleFor(x => x.CostoMensualBase)
            .GreaterThan(0).WithMessage("El costo mensual debe ser mayor a 0");

        RuleFor(x => x.LugarInstalacion)
            .NotEmpty().WithMessage("El lugar de instalación es requerido");

        // Cable validations
        When(x => x.TipoServicio == "Cable", () =>
        {
            RuleFor(x => x.DireccionInstalacion)
                .NotEmpty().WithMessage("La dirección de instalación es requerida para servicios de Cable");

            RuleFor(x => x.PlanCanales)
                .Must(p => p == null || p == "Basico" || p == "Premium")
                .WithMessage("PlanCanales debe ser 'Basico' o 'Premium'");
        });

        // Internet validations
        When(x => x.TipoServicio == "Internet", () =>
        {
            RuleFor(x => x.VelocidadMbps)
                .NotNull().WithMessage("La velocidad Mbps es requerida para servicios de Internet")
                .GreaterThan(0).WithMessage("La velocidad Mbps debe ser mayor a 0");
        });
    }
}
