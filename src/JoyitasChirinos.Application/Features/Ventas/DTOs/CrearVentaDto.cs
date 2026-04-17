namespace JoyitasChirinos.Application.Features.Ventas.DTOs;

public class CrearVentaDto
{
    public Guid? ClienteId { get; set; }
    public decimal Descuento { get; set; }
    public string MetodoPago { get; set; } = "efectivo";
    public string? Notas { get; set; }
    public List<CrearVentaItemDto> Items { get; set; } = new();
}