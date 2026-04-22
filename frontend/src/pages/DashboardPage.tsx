import { useQueries } from "@tanstack/react-query";
import { AlertTriangle, ArrowRight, Boxes, HandCoins, UserRound } from "lucide-react";
import { Link } from "react-router-dom";
import { clientsService } from "../services/clients";
import { ordersService } from "../services/orders";
import { productsService } from "../services/products";
import { salesService } from "../services/sales";
import { Badge, Button, Card, EmptyState, LoadingBlock, SectionHeading, StatCard } from "../components/ui";
import { formatCurrency, formatDateTime } from "../lib/format";

export default function DashboardPage() {
  const results = useQueries({
    queries: [
      { queryKey: ["dashboard", "products"], queryFn: () => productsService.list({ pagina: 1, tamanoPagina: 6 }) },
      { queryKey: ["dashboard", "low-stock"], queryFn: () => productsService.lowStock() },
      { queryKey: ["dashboard", "clients"], queryFn: () => clientsService.list({ pagina: 1, tamanoPagina: 5 }) },
      { queryKey: ["dashboard", "sales"], queryFn: () => salesService.list({ pagina: 1, tamanoPagina: 5 }) },
      { queryKey: ["dashboard", "orders"], queryFn: () => ordersService.list({ pagina: 1, tamanoPagina: 5 }) },
      { queryKey: ["dashboard", "orders-pending"], queryFn: () => ordersService.list({ estado: "Pendiente", pagina: 1, tamanoPagina: 20 }) }
    ]
  });

  const [products, lowStock, clients, sales, orders, pendingOrders] = results.map((result) => result.data);
  const loading = results.some((result) => result.isLoading);

  if (loading) {
    return <LoadingBlock />;
  }

  const totalRevenue = (sales?.items ?? []).reduce((sum, sale) => sum + sale.total, 0);

  return (
    <div className="grid gap-6">
      <SectionHeading
        title="Dashboard"
        subtitle="Resumen ejecutivo del negocio con foco en inventario, ventas y seguimiento operativo."
        actions={
          <Link to="/ventas">
            <Button>Registrar venta</Button>
          </Link>
        }
      />

      <div className="grid gap-4 md:grid-cols-2 xl:grid-cols-4">
        <StatCard label="Productos" value={String(products?.total ?? 0)} helper="Inventario activo y visible" />
        <StatCard label="Clientes" value={String(clients?.total ?? 0)} helper="Base de clientes registrada" />
        <StatCard label="Ventas recientes" value={formatCurrency(totalRevenue)} helper="Suma de la última consulta" />
        <StatCard label="Encargos pendientes" value={String(pendingOrders?.total ?? 0)} helper="Órdenes que requieren seguimiento" />
      </div>

      <div className="grid gap-6 xl:grid-cols-[1.3fr_0.7fr]">
        <Card>
          <div className="flex items-center justify-between gap-4">
            <div>
              <h3 className="text-lg font-semibold text-slate-950">Ventas recientes</h3>
              <p className="mt-1 text-sm text-slate-500">Últimos movimientos consultados desde la API.</p>
            </div>
            <HandCoins className="h-5 w-5 text-slate-400" />
          </div>

          <div className="mt-6 grid gap-4">
            {(sales?.items ?? []).length ? (
              sales?.items.map((sale) => (
                <div key={sale.id} className="flex flex-col gap-3 rounded-[24px] border border-slate-200 p-4 md:flex-row md:items-center md:justify-between">
                  <div>
                    <p className="text-sm font-semibold text-slate-950">Venta #{sale.numero}</p>
                    <p className="mt-1 text-sm text-slate-500">
                      {sale.clienteNombre ?? "Cliente no especificado"} · {formatDateTime(sale.fecha)}
                    </p>
                  </div>
                  <div className="flex items-center gap-3">
                    <Badge tone={sale.anulada ? "rose" : "emerald"}>
                      {sale.anulada ? "Anulada" : sale.metodoPago}
                    </Badge>
                    <p className="text-sm font-semibold text-slate-950">{formatCurrency(sale.total)}</p>
                  </div>
                </div>
              ))
            ) : (
              <EmptyState title="Sin ventas recientes" description="Cuando registres ventas, aparecerán aquí para darte una lectura rápida del negocio." />
            )}
          </div>
        </Card>

        <Card>
          <div className="flex items-center justify-between gap-4">
            <div>
              <h3 className="text-lg font-semibold text-slate-950">Alertas de stock</h3>
              <p className="mt-1 text-sm text-slate-500">Productos con inventario sensible.</p>
            </div>
            <AlertTriangle className="h-5 w-5 text-amber-500" />
          </div>

          <div className="mt-6 grid gap-3">
            {(lowStock ?? []).length ? (
              lowStock?.slice(0, 6).map((product) => (
                <div key={product.id} className="rounded-[24px] border border-amber-100 bg-amber-50/80 p-4">
                  <div className="flex items-center justify-between gap-3">
                    <div>
                      <p className="text-sm font-semibold text-slate-950">{product.nombre}</p>
                      <p className="mt-1 text-xs text-slate-500">{product.codigo} · {product.material}</p>
                    </div>
                    <Badge tone="amber">Stock {product.stockActual}</Badge>
                  </div>
                </div>
              ))
            ) : (
              <EmptyState title="Todo en orden" description="No se detectan productos en bajo stock dentro de la consulta actual." />
            )}
          </div>
        </Card>
      </div>

      <div className="grid gap-6 xl:grid-cols-2">
        <Card>
          <div className="flex items-center justify-between gap-4">
            <div>
              <h3 className="text-lg font-semibold text-slate-950">Productos destacados</h3>
              <p className="mt-1 text-sm text-slate-500">Vista rápida del catálogo actual.</p>
            </div>
            <Boxes className="h-5 w-5 text-slate-400" />
          </div>

          <div className="mt-6 grid gap-3">
            {(products?.items ?? []).length ? (
              products?.items.map((product) => (
                <div key={product.id} className="flex items-center justify-between rounded-[24px] border border-slate-200 p-4">
                  <div>
                    <p className="text-sm font-semibold text-slate-950">{product.nombre}</p>
                    <p className="mt-1 text-xs text-slate-500">{product.codigo} · {product.tipo}</p>
                  </div>
                  <div className="text-right">
                    <p className="text-sm font-semibold text-slate-950">{formatCurrency(product.precioVenta)}</p>
                    <p className="mt-1 text-xs text-slate-500">Stock: {product.stockActual}</p>
                  </div>
                </div>
              ))
            ) : (
              <EmptyState title="Sin productos" description="Aún no hay productos visibles para poblar el panel." />
            )}
          </div>
        </Card>

        <Card>
          <div className="flex items-center justify-between gap-4">
            <div>
              <h3 className="text-lg font-semibold text-slate-950">Encargos activos</h3>
              <p className="mt-1 text-sm text-slate-500">Seguimiento de producción y entrega.</p>
            </div>
            <UserRound className="h-5 w-5 text-slate-400" />
          </div>

          <div className="mt-6 grid gap-3">
            {(orders?.items ?? []).length ? (
              orders?.items.map((order) => (
                <div key={order.id} className="rounded-[24px] border border-slate-200 p-4">
                  <div className="flex items-center justify-between gap-3">
                    <div>
                      <p className="text-sm font-semibold text-slate-950">Encargo #{order.numero}</p>
                      <p className="mt-1 text-xs text-slate-500">{order.clienteNombre} · {order.descripcion}</p>
                    </div>
                    <Badge tone={order.estado === "Pendiente" ? "amber" : order.estado === "Listo" ? "emerald" : "violet"}>
                      {order.estado}
                    </Badge>
                  </div>
                  <div className="mt-3 flex items-center justify-between text-xs text-slate-500">
                    <span>Saldo pendiente</span>
                    <span>{formatCurrency(order.saldoPendiente)}</span>
                  </div>
                </div>
              ))
            ) : (
              <EmptyState title="Sin encargos" description="Cuando registres encargos, aparecerán aquí con su estado actual." />
            )}
          </div>

          <div className="mt-6">
            <Link to="/encargos">
              <Button variant="secondary" className="w-full justify-between">
                Ir al módulo de encargos
                <ArrowRight className="h-4 w-4" />
              </Button>
            </Link>
          </div>
        </Card>
      </div>
    </div>
  );
}
