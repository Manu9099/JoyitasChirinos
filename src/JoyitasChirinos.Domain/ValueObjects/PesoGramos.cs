namespace JoyitasChirinos.Domain.ValueObjects;
public record PesoGramos
{
    public decimal Valor { get; }
    public PesoGramos(decimal valor)
    {
        if (valor < 0) throw new ArgumentException("El peso no puede ser negativo");
        Valor = valor;
    }
    public override string ToString() => $"{Valor:F3} g";
}
