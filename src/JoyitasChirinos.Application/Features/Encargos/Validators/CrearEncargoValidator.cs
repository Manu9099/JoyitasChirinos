using FluentValidation;
using JoyitasChirinos.Application.Features.Encargos.Commands;

namespace JoyitasChirinos.Application.Features.Encargos.Validators;

public class CrearEncargoValidator : AbstractValidator<CrearEncargoCommand>
{
    public CrearEncargoValidator()
    {
        RuleFor(x => x.UsuarioId)
            .NotEmpty().WithMessage("El usuario es obligatorio.");

        RuleFor(x => x.Datos.Numero)
            .GreaterThan(0).WithMessage("El número del encargo debe ser mayor a 0.");

        RuleFor(x => x.Datos.ClienteId)
            .NotEmpty().WithMessage("El cliente es obligatorio.");

        RuleFor(x => x.Datos.Descripcion)
            .NotEmpty().WithMessage("La descripción es obligatoria.")
            .MaximumLength(300).WithMessage("La descripción no puede exceder 300 caracteres.");

        RuleFor(x => x.Datos.Material)
            .IsInEnum().WithMessage("El material no es válido.");

        RuleFor(x => x.Datos.PesoEstimado)
            .GreaterThanOrEqualTo(0).WithMessage("El peso estimado no puede ser negativo.")
            .When(x => x.Datos.PesoEstimado.HasValue);

        RuleFor(x => x.Datos.PrecioAcordado)
            .GreaterThanOrEqualTo(0).WithMessage("El precio acordado no puede ser negativo.");

        RuleFor(x => x.Datos.Adelanto)
            .GreaterThanOrEqualTo(0).WithMessage("El adelanto no puede ser negativo.");

        RuleFor(x => x.Datos)
            .Must(x => x.Adelanto <= x.PrecioAcordado)
            .WithMessage("El adelanto no puede ser mayor al precio acordado.");

        RuleFor(x => x.Datos.FechaEntrega)
            .GreaterThanOrEqualTo(DateTime.Today).WithMessage("La fecha de entrega no puede ser menor a hoy.")
            .When(x => x.Datos.FechaEntrega.HasValue);

        RuleFor(x => x.Datos.FotoReferenciaUrl)
            .MaximumLength(500).WithMessage("La URL de referencia no puede exceder 500 caracteres.")
            .When(x => !string.IsNullOrWhiteSpace(x.Datos.FotoReferenciaUrl));

        RuleFor(x => x.Datos.Notas)
            .MaximumLength(500).WithMessage("Las notas no pueden exceder 500 caracteres.")
            .When(x => !string.IsNullOrWhiteSpace(x.Datos.Notas));
    }
}