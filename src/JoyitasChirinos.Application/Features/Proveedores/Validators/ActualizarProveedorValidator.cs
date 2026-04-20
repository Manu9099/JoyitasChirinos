using FluentValidation;
using JoyitasChirinos.Application.Features.Proveedores.Commands;

namespace JoyitasChirinos.Application.Features.Proveedores.Validators;

public class ActualizarProveedorValidator : AbstractValidator<ActualizarProveedorCommand>
{
    public ActualizarProveedorValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty().WithMessage("El id es obligatorio.");

        RuleFor(x => x.Datos.Nombre)
            .NotEmpty().WithMessage("El nombre es obligatorio.")
            .MaximumLength(120).WithMessage("El nombre no puede exceder 120 caracteres.");

        RuleFor(x => x.Datos.Telefono)
            .MaximumLength(20).When(x => !string.IsNullOrWhiteSpace(x.Datos.Telefono));

        RuleFor(x => x.Datos.Email)
            .EmailAddress().When(x => !string.IsNullOrWhiteSpace(x.Datos.Email))
            .WithMessage("El email no es válido.")
            .MaximumLength(120).When(x => !string.IsNullOrWhiteSpace(x.Datos.Email));

        RuleFor(x => x.Datos.Tipo)
            .NotEmpty().WithMessage("El tipo es obligatorio.")
            .MaximumLength(40).WithMessage("El tipo no puede exceder 40 caracteres.");

        RuleFor(x => x.Datos.Notas)
            .MaximumLength(500).When(x => !string.IsNullOrWhiteSpace(x.Datos.Notas));
    }
}