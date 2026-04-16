namespace JoyitasChirinos.Domain.ValueObjects;
public record Dinero(decimal Monto, string Moneda = "PEN")
{
    public static Dinero Cero => new(0m);
    public Dinero Sumar(Dinero otro)
    {
        if (Moneda != otro.Moneda) throw new InvalidOperationException("Monedas distintas");
        return new Dinero(Monto + otro.Monto, Moneda);
    }
    public override string ToString() => $"{Moneda} {Monto:F2}";
}
