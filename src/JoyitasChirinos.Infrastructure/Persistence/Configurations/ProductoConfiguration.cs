using JoyitasChirinos.Domain.Entities;
using JoyitasChirinos.Domain.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
namespace JoyitasChirinos.Infrastructure.Persistence.Configurations;
public class ProductoConfiguration : IEntityTypeConfiguration<Producto>
{
    public void Configure(EntityTypeBuilder<Producto> b)
    {
        b.ToTable("productos");
        b.HasKey(x => x.Id);
        b.Property(x => x.Id).HasColumnName("id");
        b.Property(x => x.Codigo).HasColumnName("codigo").HasMaxLength(30).IsRequired();
        b.HasIndex(x => x.Codigo).IsUnique();
        b.Property(x => x.Nombre).HasColumnName("nombre").HasMaxLength(150).IsRequired();
        b.Property(x => x.Tipo).HasColumnName("tipo").HasConversion<string>().IsRequired();
        b.Property(x => x.Material).HasColumnName("material").HasConversion<string>().IsRequired();
        b.OwnsOne(x => x.Peso, peso => {
            peso.Property(p => p.Valor).HasColumnName("peso_gramos").HasColumnType("numeric(8,3)");
        });
        b.OwnsOne(x => x.PrecioCosto, d => {
            d.Property(p => p.Monto).HasColumnName("precio_costo").HasColumnType("numeric(10,2)");
            d.Ignore(p => p.Moneda);
        });
        b.OwnsOne(x => x.PrecioVenta, d => {
            d.Property(p => p.Monto).HasColumnName("precio_venta").HasColumnType("numeric(10,2)");
            d.Ignore(p => p.Moneda);
        });
        b.Property(x => x.StockActual).HasColumnName("stock_actual");
        b.Property(x => x.StockMinimo).HasColumnName("stock_minimo");
        b.Property(x => x.FotoUrl).HasColumnName("foto_url");
        b.Property(x => x.Descripcion).HasColumnName("descripcion");
        b.Property(x => x.Estado).HasColumnName("estado").HasConversion<string>();
        b.Property(x => x.CategoriaId).HasColumnName("categoria_id");
        b.Property(x => x.ProveedorId).HasColumnName("proveedor_id");
        b.Property(x => x.CreatedAt).HasColumnName("created_at");
        b.Property(x => x.UpdatedAt).HasColumnName("updated_at");
        b.HasOne(x => x.Categoria).WithMany(c => c.Productos).HasForeignKey(x => x.CategoriaId);
        b.HasOne(x => x.Proveedor).WithMany().HasForeignKey(x => x.ProveedorId);
    }
}
