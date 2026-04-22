import { useMemo, useState } from "react";
import { useMutation, useQuery, useQueryClient } from "@tanstack/react-query";
import { toast } from "sonner";
import { Plus, Trash2 } from "lucide-react";
import { clientsService } from "../services/clients";
import { productsService } from "../services/products";
import { salesService } from "../services/sales";
import { PAYMENT_METHODS } from "../types/domain";
import { formatCurrency, formatDateTime, safeNumber } from "../lib/format";
import {
  Badge,
  Button,
  Card,
  Cell,
  EmptyState,
  Field,
  Input,
  LoadingBlock,
  Modal,
  SectionHeading,
  Select,
  Spinner,
  StatCard,
  Table,
  TableRow,
  Textarea
} from "../components/ui";

type CartItem = {
  productoId: string;
  nombre: string;
  precioVenta: number;
  cantidad: number;
};

export default function SalesPage() {
  const queryClient = useQueryClient();
  const [page, setPage] = useState(1);
  const [openCreate, setOpenCreate] = useState(false);
  const [clienteId, setClienteId] = useState("");
  const [metodoPago, setMetodoPago] = useState("efectivo");
  const [descuento, setDescuento] = useState(0);
  const [notas, setNotas] = useState("");
  const [productoId, setProductoId] = useState("");
  const [cantidad, setCantidad] = useState(1);
  const [cart, setCart] = useState<CartItem[]>([]);

  const salesQuery = useQuery({
    queryKey: ["sales", page],
    queryFn: () => salesService.list({ pagina: page, tamanoPagina: 10 })
  });

  const clientsQuery = useQuery({
    queryKey: ["sales", "clients-selector"],
    queryFn: () => clientsService.list({ pagina: 1, tamanoPagina: 100 })
  });

  const productsQuery = useQuery({
    queryKey: ["sales", "products-selector"],
    queryFn: () => productsService.list({ pagina: 1, tamanoPagina: 100 })
  });

  const createMutation = useMutation({
    mutationFn: () =>
      salesService.create({
        clienteId: clienteId || null,
        descuento: safeNumber(descuento),
        metodoPago,
        notas: notas || null,
        items: cart.map((item) => ({
          productoId: item.productoId,
          cantidad: item.cantidad
        }))
      }),
    onSuccess: () => {
      toast.success("Venta registrada");
      setOpenCreate(false);
      setClienteId("");
      setMetodoPago("efectivo");
      setDescuento(0);
      setNotas("");
      setProductoId("");
      setCantidad(1);
      setCart([]);
      queryClient.invalidateQueries({ queryKey: ["sales"] });
      queryClient.invalidateQueries({ queryKey: ["products"] });
    },
    onError: (error) => toast.error(error.message)
  });

  const cancelMutation = useMutation({
    mutationFn: (id: string) => salesService.cancel(id),
    onSuccess: () => {
      toast.success("Venta anulada");
      queryClient.invalidateQueries({ queryKey: ["sales"] });
    },
    onError: (error) => toast.error(error.message)
  });

  const subtotal = useMemo(
    () => cart.reduce((sum, item) => sum + item.precioVenta * item.cantidad, 0),
    [cart]
  );
  const total = Math.max(0, subtotal - safeNumber(descuento));

  const addToCart = () => {
    const product = productsQuery.data?.items.find((item) => item.id === productoId);
    if (!product) {
      toast.error("Selecciona un producto");
      return;
    }
    if (cantidad <= 0) {
      toast.error("La cantidad debe ser mayor a 0");
      return;
    }

    setCart((items) => {
      const existing = items.find((item) => item.productoId === product.id);
      if (existing) {
        return items.map((item) =>
          item.productoId === product.id
            ? { ...item, cantidad: item.cantidad + cantidad }
            : item
        );
      }

      return [
        ...items,
        {
          productoId: product.id,
          nombre: product.nombre,
          precioVenta: product.precioVenta,
          cantidad
        }
      ];
    });

    setProductoId("");
    setCantidad(1);
  };

  if (salesQuery.isLoading || clientsQuery.isLoading || productsQuery.isLoading) {
    return <LoadingBlock />;
  }

  return (
    <div className="grid gap-6">
      <SectionHeading
        title="Ventas"
        subtitle="Registra ventas con rapidez, controla anulaciones y revisa el flujo comercial."
        actions={
          <Button onClick={() => setOpenCreate(true)}>
            <Plus className="mr-2 h-4 w-4" />
            Nueva venta
          </Button>
        }
      />

      <div className="grid gap-4 md:grid-cols-3">
        <StatCard label="Ventas totales" value={String(salesQuery.data?.total ?? 0)} helper="Historial visible desde el endpoint" />
        <StatCard label="Importe visible" value={formatCurrency((salesQuery.data?.items ?? []).reduce((sum, item) => sum + item.total, 0))} helper="Suma de la página actual" />
        <StatCard label="Anuladas en página" value={String((salesQuery.data?.items ?? []).filter((item) => item.anulada).length)} helper="Control de incidencias" />
      </div>

      {(salesQuery.data?.items ?? []).length ? (
        <Table headers={["Venta", "Cliente", "Método", "Estado", "Total", "Acciones"]}>
          {salesQuery.data?.items.map((sale) => (
            <TableRow key={sale.id}>
              <Cell>
                <div>
                  <p className="font-semibold text-slate-900">#{sale.numero}</p>
                  <p className="mt-1 text-xs text-slate-500">{formatDateTime(sale.fecha)}</p>
                </div>
              </Cell>
              <Cell>{sale.clienteNombre ?? "Cliente no especificado"}</Cell>
              <Cell>{sale.metodoPago}</Cell>
              <Cell>
                <Badge tone={sale.anulada ? "rose" : "emerald"}>
                  {sale.anulada ? "Anulada" : sale.estado}
                </Badge>
              </Cell>
              <Cell>{formatCurrency(sale.total)}</Cell>
              <Cell>
                {!sale.anulada ? (
                  <Button
                    variant="secondary"
                    onClick={() => {
                      const confirmed = window.confirm(`¿Anular la venta #${sale.numero}?`);
                      if (confirmed) cancelMutation.mutate(sale.id);
                    }}
                  >
                    Anular
                  </Button>
                ) : (
                  <span className="text-xs text-slate-400">Sin acciones</span>
                )}
              </Cell>
            </TableRow>
          ))}
        </Table>
      ) : (
        <EmptyState title="Sin ventas" description="Aún no se registraron ventas en el sistema." />
      )}

      <div className="flex items-center justify-between">
        <p className="text-sm text-slate-500">
          Página {salesQuery.data?.pagina ?? 1} de {salesQuery.data?.totalPaginas ?? 1}
        </p>
        <div className="flex gap-3">
          <Button variant="secondary" disabled={page <= 1} onClick={() => setPage((value) => Math.max(1, value - 1))}>
            Anterior
          </Button>
          <Button
            variant="secondary"
            disabled={page >= (salesQuery.data?.totalPaginas ?? 1)}
            onClick={() => setPage((value) => value + 1)}
          >
            Siguiente
          </Button>
        </div>
      </div>

      <Modal
        open={openCreate}
        onClose={() => setOpenCreate(false)}
        title="Registrar venta"
        description="Arma el carrito, aplica descuento y guarda la operación."
      >
        <div className="grid gap-4 lg:grid-cols-[1fr_1fr]">
          <div className="grid gap-4">
            <Field label="Cliente">
              <Select value={clienteId} onChange={(e) => setClienteId(e.target.value)}>
                <option value="">Venta sin cliente</option>
                {clientsQuery.data?.items.map((client) => (
                  <option key={client.id} value={client.id}>
                    {client.nombre}
                  </option>
                ))}
              </Select>
            </Field>

            <Field label="Método de pago" required>
              <Select value={metodoPago} onChange={(e) => setMetodoPago(e.target.value)}>
                {PAYMENT_METHODS.map((option) => (
                  <option key={option} value={option}>{option}</option>
                ))}
              </Select>
            </Field>

            <Field label="Descuento">
              <Input type="number" min="0" step="0.01" value={descuento} onChange={(e) => setDescuento(Number(e.target.value))} />
            </Field>

            <Field label="Notas">
              <Textarea value={notas} onChange={(e) => setNotas(e.target.value)} />
            </Field>
          </div>

          <div className="grid gap-4">
            <Card className="p-4">
              <div className="grid gap-3">
                <Field label="Producto">
                  <Select value={productoId} onChange={(e) => setProductoId(e.target.value)}>
                    <option value="">Selecciona un producto</option>
                    {productsQuery.data?.items
                      .filter((product) => product.estado === "Activo" && product.stockActual > 0)
                      .map((product) => (
                        <option key={product.id} value={product.id}>
                          {product.nombre} · {formatCurrency(product.precioVenta)}
                        </option>
                      ))}
                  </Select>
                </Field>
                <Field label="Cantidad">
                  <Input type="number" min="1" value={cantidad} onChange={(e) => setCantidad(Number(e.target.value))} />
                </Field>
                <Button variant="secondary" onClick={addToCart}>
                  Agregar al carrito
                </Button>
              </div>
            </Card>

            <Card className="p-4">
              <div className="flex items-center justify-between">
                <h4 className="font-semibold text-slate-950">Carrito</h4>
                <Badge tone="amber">{cart.length} ítems</Badge>
              </div>

              <div className="mt-4 grid gap-3">
                {cart.length ? (
                  cart.map((item) => (
                    <div key={item.productoId} className="flex items-center justify-between rounded-2xl border border-slate-200 p-3">
                      <div>
                        <p className="text-sm font-semibold text-slate-900">{item.nombre}</p>
                        <p className="mt-1 text-xs text-slate-500">
                          {item.cantidad} x {formatCurrency(item.precioVenta)}
                        </p>
                      </div>
                      <button
                        type="button"
                        className="rounded-xl p-2 text-rose-600 hover:bg-rose-50"
                        onClick={() => setCart((items) => items.filter((cartItem) => cartItem.productoId !== item.productoId))}
                      >
                        <Trash2 className="h-4 w-4" />
                      </button>
                    </div>
                  ))
                ) : (
                  <p className="text-sm text-slate-500">Todavía no agregaste productos.</p>
                )}
              </div>

              <div className="mt-4 space-y-2 border-t border-slate-200 pt-4 text-sm">
                <div className="flex items-center justify-between">
                  <span className="text-slate-500">Subtotal</span>
                  <span className="font-medium text-slate-900">{formatCurrency(subtotal)}</span>
                </div>
                <div className="flex items-center justify-between">
                  <span className="text-slate-500">Descuento</span>
                  <span className="font-medium text-slate-900">{formatCurrency(descuento)}</span>
                </div>
                <div className="flex items-center justify-between text-base">
                  <span className="font-semibold text-slate-950">Total</span>
                  <span className="font-semibold text-slate-950">{formatCurrency(total)}</span>
                </div>
              </div>
            </Card>
          </div>
        </div>

        <div className="mt-6 flex justify-end gap-3">
          <Button variant="secondary" onClick={() => setOpenCreate(false)}>Cancelar</Button>
          <Button onClick={() => createMutation.mutate()} disabled={createMutation.isPending || cart.length === 0}>
            {createMutation.isPending ? <span className="inline-flex items-center gap-2"><Spinner />Guardando...</span> : "Registrar venta"}
          </Button>
        </div>
      </Modal>
    </div>
  );
}
