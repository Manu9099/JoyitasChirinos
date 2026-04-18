using FluentValidation;
using JoyitasChirinos.Application.Features.Clientes.Commands;

namespace JoyitasChirinos.Application.Features.Clientes.Validators;

public class ActualizarClienteValidator : AbstractValidator<ActualizarClienteCommand>
{
    public ActualizarClienteValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty().WithMessage("El id es obligatorio.");

        RuleFor(x => x.Datos.Nombre)
            .NotEmpty().WithMessage("El nombre es obligatorio.")
            .MaximumLength(150).WithMessage("El nombre no puede exceder 150 caracteres.");

        RuleFor(x => x.Datos.Telefono)
            .MaximumLength(20).When(x => !string.IsNullOrWhiteSpace(x.Datos.Telefono));

        RuleFor(x => x.Datos.Email)
            .EmailAddress().When(x => !string.IsNullOrWhiteSpace(x.Datos.Email))
            .WithMessage("El email no es válido.")
            .MaximumLength(120).When(x => !string.IsNullOrWhiteSpace(x.Datos.Email));

        RuleFor(x => x.Datos.Dni)
            .Matches(@"^\d{8}$").When(x => !string.IsNullOrWhiteSpace(x.Datos.Dni))
            .WithMessage("El DNI debe tener 8 dígitos.");

        RuleFor(x => x.Datos.Notas)
            .MaximumLength(500).When(x => !string.IsNullOrWhiteSpace(x.Datos.Notas));
    }
}