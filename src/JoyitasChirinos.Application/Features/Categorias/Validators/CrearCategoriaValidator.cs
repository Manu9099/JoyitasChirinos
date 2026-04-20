using FluentValidation;
using JoyitasChirinos.Application.Features.Categorias.Commands;

namespace JoyitasChirinos.Application.Features.Categorias.Validators;

public class CrearCategoriaValidator : AbstractValidator<CrearCategoriaCommand>
{
    public CrearCategoriaValidator()
    {
        RuleFor(x => x.Datos.Nombre)
            .NotEmpty().WithMessage("El nombre es obligatorio.")
            .MaximumLength(80).WithMessage("El nombre no puede exceder 80 caracteres.");

        RuleFor(x => x.Datos.Descripcion)
            .MaximumLength(300).When(x => !string.IsNullOrWhiteSpace(x.Datos.Descripcion));
    }
}