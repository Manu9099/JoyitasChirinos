using FluentValidation;
using JoyitasChirinos.Application.Features.Categorias.Commands;

namespace JoyitasChirinos.Application.Features.Categorias.Validators;

public class ActualizarCategoriaValidator : AbstractValidator<ActualizarCategoriaCommand>
{
    public ActualizarCategoriaValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty().WithMessage("El id es obligatorio.");

        RuleFor(x => x.Datos.Nombre)
            .NotEmpty().WithMessage("El nombre es obligatorio.")
            .MaximumLength(80).WithMessage("El nombre no puede exceder 80 caracteres.");

        RuleFor(x => x.Datos.Descripcion)
            .MaximumLength(300).When(x => !string.IsNullOrWhiteSpace(x.Datos.Descripcion));
    }
}