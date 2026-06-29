using api.Models.DTOs;
using FluentValidation;

namespace api.Validators;

public class ClienteCreateRequestValidator : AbstractValidator<ClienteCreateRequest>
{
    public ClienteCreateRequestValidator()
    {
        RuleFor(x => x.Codigo)
            .NotEmpty().WithMessage("El código es requerido")
            .MaximumLength(20).WithMessage("El código no puede exceder 20 caracteres");

        RuleFor(x => x.Nombre)
            .NotEmpty().WithMessage("El nombre es requerido")
            .MinimumLength(2).WithMessage("El nombre debe tener al menos 2 caracteres")
            .MaximumLength(150).WithMessage("El nombre no puede exceder 150 caracteres");

        RuleFor(x => x.Direccion)
            .NotEmpty().WithMessage("La dirección es requerida");

        RuleFor(x => x.Correo)
            .NotEmpty().WithMessage("El correo es requerido")
            .EmailAddress().WithMessage("Formato de correo inválido")
            .MaximumLength(255);

        RuleFor(x => x.Telefono)
            .NotEmpty().WithMessage("El teléfono es requerido")
            .Matches(@"^\d{7,20}$").WithMessage("El teléfono debe contener entre 7 y 20 dígitos");
    }
}
