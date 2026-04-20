using FluentValidation;
using JoyitasChirinos.Application.Features.Encargos.Commands;

namespace JoyitasChirinos.Application.Features.Encargos.Validators;

public class CambiarEstadoEncargoValidator : AbstractValidator<CambiarEstadoEncargoCommand>
{
    public CambiarEstadoEncargoValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty().WithMessage("El id es obligatorio.");

        RuleFor(x => x.Estado)
            .IsInEnum().WithMessage("El estado no es válido.");
    }
}