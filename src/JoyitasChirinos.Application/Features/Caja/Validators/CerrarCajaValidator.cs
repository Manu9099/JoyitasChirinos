using FluentValidation;
using JoyitasChirinos.Application.Features.Caja.Commands;

namespace JoyitasChirinos.Application.Features.Caja.Validators;

public class CerrarCajaValidator : AbstractValidator<CerrarCajaCommand>
{
    public CerrarCajaValidator()
    {
        RuleFor(x => x.Datos.MontoFinal)
            .GreaterThanOrEqualTo(0).WithMessage("El monto final no puede ser negativo.");

        RuleFor(x => x.Datos.Observaciones)
            .MaximumLength(500)
            .When(x => !string.IsNullOrWhiteSpace(x.Datos.Observaciones));
    }
}