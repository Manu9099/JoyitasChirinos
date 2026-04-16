using FluentValidation.Results;
namespace JoyitasChirinos.Application.Common.Exceptions;
public class ValidationException : Exception
{
    public IDictionary<string, string[]> Errores { get; }
    public ValidationException(IEnumerable<ValidationFailure> failures)
        : base("Ocurrieron uno o más errores de validación.")
    {
        Errores = failures.GroupBy(f => f.PropertyName, f => f.ErrorMessage)
            .ToDictionary(g => g.Key, g => g.ToArray());
    }
}
