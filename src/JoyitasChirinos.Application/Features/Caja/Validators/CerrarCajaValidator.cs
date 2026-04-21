using FluentValidation;
using JoyitasChirinos.Application.Features.Caja.Commands;

namespace JoyitasChirinos.Application.Features.Caja.Validators;

public class CerrarCajaValidator : AbstractValidator<CerrarCajaCommand> 
{
    public CerrarCajaValidator() 
    {
        RuleFor(x => x.UsuarioId).NotEmpty().WithMessage("El usuario es obligatorio.");
        RuleFor(x => x.Datos.MontoFinalContado).GreaterThanOrEqualTo(0).WithMessage("El monto final contado no puede ser negativo.");
        RuleFor(x => x.Datos.Observaciones).MaximumLength(500).When(x => !string.IsNullOrWhiteSpace(x.Datos.Observaciones));
    }
}