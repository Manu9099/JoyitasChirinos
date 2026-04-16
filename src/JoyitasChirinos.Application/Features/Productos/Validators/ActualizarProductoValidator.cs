using FluentValidation;
using JoyitasChirinos.Application.Features.Productos.Commands;
using JoyitasChirinos.Domain.Enums;

namespace JoyitasChirinos.Application.Features.Productos.Validators;

public class ActualizarProductoValidator : AbstractValidator<ActualizarProductoCommand>
{
    public ActualizarProductoValidator()
    {
        RuleFor(x => x.Id).NotEmpty();

        RuleFor(x => x.Datos.Nombre)
            .NotEmpty().WithMessage("El nombre es obligatorio.")
            .MaximumLength(150);

        RuleFor(x => x.Datos.PrecioVenta)
            .GreaterThan(0).WithMessage("El precio de venta debe ser mayor a cero.");

        RuleFor(x => x.Datos.CategoriaId)
            .NotEmpty().WithMessage("La categoría es obligatoria.");
    }
}
