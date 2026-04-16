using MediatR;

namespace JoyitasChirinos.Application.Features.Productos.Commands;

public record AjustarStockCommand(Guid ProductoId, int Cantidad, string Operacion) : IRequest;
// Operacion: "agregar" | "retirar"
