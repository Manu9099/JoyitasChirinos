using FluentValidation;
using JoyitasChirinos.Application.Features.Productos.Commands;
using JoyitasChirinos.Domain.Enums;

namespace JoyitasChirinos.Application.Features.Productos.Validators;

public class CrearProductoValidator : AbstractValidator<CrearProductoCommand>
{
    private static readonly string[] TiposValidos =
        Enum.GetNames<TipoProducto>().Select(t => t.ToLower()).ToArray();
    private static readonly string[] MaterialesValidos =
        Enum.GetNames<MaterialProducto>().Select(m => m.ToLower()).ToArray();

    public CrearProductoValidator()
    {
        RuleFor(x => x.Datos.Codigo)
            .NotEmpty().WithMessage("El código es obligatorio.")
            .MaximumLength(30).WithMessage("El código no puede tener más de 30 caracteres.");

        RuleFor(x => x.Datos.Nombre)
            .NotEmpty().WithMessage("El nombre es obligatorio.")
            .MaximumLength(150).WithMessage("El nombre no puede tener más de 150 caracteres.");

        RuleFor(x => x.Datos.Tipo)
            .NotEmpty()
            .Must(t => TiposValidos.Contains(t?.ToLower()))
            .WithMessage($"Tipo inválido. Valores válidos: {string.Join(", ", TiposValidos)}");

        RuleFor(x => x.Datos.Material)
            .NotEmpty()
            .Must(m => MaterialesValidos.Contains(m?.ToLower()))
            .WithMessage($"Material inválido. Valores válidos: {string.Join(", ", MaterialesValidos)}");

        RuleFor(x => x.Datos.PrecioCosto)
            .GreaterThanOrEqualTo(0).WithMessage("El precio de costo no puede ser negativo.");

        RuleFor(x => x.Datos.PrecioVenta)
            .GreaterThan(0).WithMessage("El precio de venta debe ser mayor a cero.");

        RuleFor(x => x.Datos.StockInicial)
            .GreaterThanOrEqualTo(0).WithMessage("El stock inicial no puede ser negativo.");

        RuleFor(x => x.Datos.StockMinimo)
            .GreaterThanOrEqualTo(0).WithMessage("El stock mínimo no puede ser negativo.");

        RuleFor(x => x.Datos.CategoriaId)
            .NotEmpty().WithMessage("La categoría es obligatoria.");

        RuleFor(x => x.Datos.PesoGramos)
            .GreaterThan(0).When(x => x.Datos.PesoGramos.HasValue)
            .WithMessage("El peso debe ser mayor a cero.");
    }
}
