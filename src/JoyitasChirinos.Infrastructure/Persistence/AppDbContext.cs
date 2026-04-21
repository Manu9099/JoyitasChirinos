using JoyitasChirinos.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System.Reflection;
using JoyitasChirinos.Application.Common.Interfaces;

namespace JoyitasChirinos.Infrastructure.Persistence;


public class AppDbContext : DbContext, IAppDbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }
    public DbSet<Producto> Productos => Set<Producto>();
    public DbSet<Categoria> Categorias => Set<Categoria>();
    public DbSet<Proveedor> Proveedores => Set<Proveedor>();
    public DbSet<Cliente> Clientes => Set<Cliente>();
    public DbSet<Venta> Ventas => Set<Venta>();
    public DbSet<VentaItem> VentaItems => Set<VentaItem>();
    public DbSet<Encargo> Encargos => Set<Encargo>();
    public DbSet<Usuario> Usuarios => Set<Usuario>();
 public DbSet<CajaSesion> CajaSesiones => Set<CajaSesion>();
 
  public DbSet<CajaMovimiento> CajaMovimientos => Set<CajaMovimiento>();
  protected override void OnModelCreating(ModelBuilder modelBuilder)
{
    base.OnModelCreating(modelBuilder);
    modelBuilder.ApplyConfigurationsFromAssembly(typeof(AppDbContext).Assembly);
}
        
        
        

}
