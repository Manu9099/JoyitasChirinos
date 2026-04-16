using FluentAssertions;
using JoyitasChirinos.Domain.Entities;
using JoyitasChirinos.Domain.Enums;
using Xunit;

namespace JoyitasChirinos.UnitTests.Domain;

public class ProductoTests
{
    private static Producto ProductoBase() => Producto.Crear(
        "CAD-001", "Cadena Oro 18k 50cm",
        TipoProducto.Cadena, MaterialProducto.Oro18k,
        precioCosto: 200, precioVenta: 350,
        stockInicial: 5, categoriaId: Guid.NewGuid());

    [Fact]
    public void Crear_ProductoValido_DebeCrearseCorrectamente()
    {
        var p = ProductoBase();
        p.Codigo.Should().Be("CAD-001");
        p.StockActual.Should().Be(5);
        p.Estado.Should().Be(EstadoProducto.Activo);
    }

    [Fact]
    public void AgregarStock_CantidadPositiva_DebeIncrementarStock()
    {
        var p = ProductoBase();
        p.AgregarStock(3);
        p.StockActual.Should().Be(8);
    }

    [Fact]
    public void RetirarStock_CantidadExacta_DebeQuedarEnCero_YEstadoAgotado()
    {
        var p = ProductoBase();
        p.RetirarStock(5);
        p.StockActual.Should().Be(0);
        p.Estado.Should().Be(EstadoProducto.Agotado);
    }

    [Fact]
    public void RetirarStock_MasDelDisponible_DebeLanzarExcepcion()
    {
        var p = ProductoBase();
        var act = () => p.RetirarStock(99);
        act.Should().Throw<InvalidOperationException>().WithMessage("*insuficiente*");
    }

    [Fact]
    public void AgregarStock_CuandoEstaAgotado_DebeVolverActivo()
    {
        var p = ProductoBase();
        p.RetirarStock(5);
        p.Estado.Should().Be(EstadoProducto.Agotado);
        p.AgregarStock(1);
        p.Estado.Should().Be(EstadoProducto.Activo);
    }

    [Fact]
    public void TieneBajoStock_StockIgualAlMinimo_DebeSerTrue()
    {
        var p = Producto.Crear("X", "X", TipoProducto.Anillo, MaterialProducto.Oro18k,
            100, 200, stockInicial: 1, Guid.NewGuid(), stockMinimo: 1);
        p.TieneBajoStock.Should().BeTrue();
    }

    [Fact]
    public void Crear_CodigoVacio_DebeThrow()
    {
        var act = () => Producto.Crear("", "Nombre", TipoProducto.Anillo, MaterialProducto.Plata,
            100, 200, 1, Guid.NewGuid());
        act.Should().Throw<ArgumentException>();
    }
}
