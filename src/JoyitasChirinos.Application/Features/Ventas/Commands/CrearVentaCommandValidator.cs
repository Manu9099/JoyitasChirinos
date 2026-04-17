using FluentValidation;

namespace JoyitasChirinos.Application.Features.Ventas.Commands;

public class CrearVentaCommandValidator : AbstractValidator<CrearVentaCommand>
{
    public CrearVentaCommandValidator()
    {
        RuleFor(x => x.MetodoPago)
            .NotEmpty().WithMessage("El método de pago es obligatorio.");

        RuleFor(x => x.Descuento)
            .GreaterThanOrEqualTo(0).WithMessage("El descuento no puede ser negativo.");

        RuleFor(x => x.Items)
            .NotNull().WithMessage("Debe enviar items.")
            .Must(x => x.Count > 0).WithMessage("La venta debe tener al menos un item.");

        RuleForEach(x => x.Items).ChildRules(item =>
        {
            item.RuleFor(i => i.ProductoId)
                .NotEmpty().WithMessage("El producto es obligatorio.");

            item.RuleFor(i => i.Cantidad)
                .GreaterThan(0).WithMessage("La cantidad debe ser mayor a cero.");
        });
    }
}