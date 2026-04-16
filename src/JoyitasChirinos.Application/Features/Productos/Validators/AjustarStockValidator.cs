using FluentValidation;
using JoyitasChirinos.Application.Features.Productos.Commands;

namespace JoyitasChirinos.Application.Features.Productos.Validators;

public class AjustarStockValidator : AbstractValidator<AjustarStockCommand>
{
    public AjustarStockValidator()
    {
        RuleFor(x => x.ProductoId).NotEmpty();
        RuleFor(x => x.Cantidad).GreaterThan(0).WithMessage("La cantidad debe ser mayor a cero.");
        RuleFor(x => x.Operacion)
            .Must(op => op == "agregar" || op == "retirar")
            .WithMessage("Operación inválida. Use 'agregar' o 'retirar'.");
    }
}
