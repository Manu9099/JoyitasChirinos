using JoyitasChirinos.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace JoyitasChirinos.Application.Common.Interfaces;

public interface IAppDbContext
{
    DbSet<Producto> Productos { get; }
    DbSet<Categoria> Categorias { get; }
    DbSet<Proveedor> Proveedores { get; }
    DbSet<Cliente> Clientes { get; }

    DbSet<Venta> Ventas { get; }
    DbSet<VentaItem> VentaItems { get; }
    DbSet<Encargo> Encargos { get; }
     DbSet<CajaSesion> CajaSesiones { get; }
    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}