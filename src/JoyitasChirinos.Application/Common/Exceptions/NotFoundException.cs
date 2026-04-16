namespace JoyitasChirinos.Application.Common.Exceptions;
public class NotFoundException : Exception
{
    public NotFoundException(string entidad, object clave)
        : base($"{entidad} con id '{clave}' no fue encontrado.") { }
}
