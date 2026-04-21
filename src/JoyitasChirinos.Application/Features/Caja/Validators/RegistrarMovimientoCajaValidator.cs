using FluentValidation;
using JoyitasChirinos.Application.Features.Caja.Commands;

namespace JoyitasChirinos.Application.Features.Caja.Validators;

public class RegistrarMovimientoCajaValidator : AbstractValidator<RegistrarMovimientoCajaCommand> 
{
    public RegistrarMovimientoCajaValidator() 
    {
        RuleFor(x => x.UsuarioId).NotEmpty().WithMessage("El usuario es obligatorio.");
        RuleFor(x => x.Datos.Tipo).IsInEnum().WithMessage("El tipo de movimiento no es válido.");
        RuleFor(x => x.Datos.Monto).GreaterThan(0).WithMessage("El monto debe ser mayor a 0.");
        RuleFor(x => x.Datos.Motivo).NotEmpty().WithMessage("El motivo es obligatorio.").MaximumLength(150).WithMessage("El motivo no puede exceder 150 caracteres.");
        RuleFor(x => x.Datos.Observaciones).MaximumLength(500).When(x => !string.IsNullOrWhiteSpace(x.Datos.Observaciones));
    }
}