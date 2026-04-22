import { useMemo, useState } from "react";
import { useMutation, useQuery, useQueryClient } from "@tanstack/react-query";
import { toast } from "sonner";
import { PackagePlus, PencilLine, Trash2 } from "lucide-react";
import { productsService } from "../services/products";
import type { CreateProductPayload } from "../types/api";
import { PRODUCT_MATERIALS, PRODUCT_STATES, PRODUCT_TYPES } from "../types/domain";
import { formatCurrency } from "../lib/format";
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
import { useAuthStore } from "../store/auth";

const initialForm: CreateProductPayload = {
  codigo: "",
  nombre: "",
  tipo: "Cadena",
  material: "Oro18k",
  precioCosto: 0,
  precioVenta: 0,
  stockInicial: 0,
  stockMinimo: 1,
  categoriaId: "",
  proveedorId: "",
  pesoGramos: undefined,
  descripcion: ""
};

export default function ProductsPage() {
  const isAdmin = useAuthStore((state) => state.user?.rol === "Admin");
  const queryClient = useQueryClient();
  const [openCreate, setOpenCreate] = useState(false);
  const [stockModal, setStockModal] = useState<{ id: string; nombre: string } | null>(null);
  const [search, setSearch] = useState("");
  const [tipo, setTipo] = useState("");
  const [material, setMaterial] = useState("");
  const [estado, setEstado] = useState("");
  const [page, setPage] = useState(1);
  const [form, setForm] = useState<CreateProductPayload>(initialForm);
  const [stockQty, setStockQty] = useState(1);
  const [stockOp, setStockOp] = useState<"sumar" | "restar">("sumar");

  const productsQuery = useQuery({
    queryKey: ["products", { search, tipo, material, estado, page }],
    queryFn: () =>
      productsService.list({
        busqueda: search || undefined,
        tipo: tipo || undefined,
        material: material || undefined,
        estado: estado || undefined,
        pagina: page,
        tamanoPagina: 10
      })
  });

  const lowStockQuery = useQuery({
    queryKey: ["products", "low-stock"],
    queryFn: () => productsService.lowStock()
  });

  const createMutation = useMutation({
    mutationFn: () =>
      productsService.create({
        ...form,
        proveedorId: form.proveedorId || null,
        pesoGramos: form.pesoGramos ? Number(form.pesoGramos) : null,
        precioCosto: Number(form.precioCosto),
        precioVenta: Number(form.precioVenta),
        stockInicial: Number(form.stockInicial),
        stockMinimo: Number(form.stockMinimo)
      }),
    onSuccess: () => {
      toast.success("Producto creado correctamente");
      setOpenCreate(false);
      setForm(initialForm);
      queryClient.invalidateQueries({ queryKey: ["products"] });
    },
    onError: (error) => toast.error(error.message)
  });

  const adjustStockMutation = useMutation({
    mutationFn: () => {
      if (!stockModal) throw new Error("Producto no seleccionado");
      return productsService.adjustStock(stockModal.id, stockQty, stockOp);
    },
    onSuccess: () => {
      toast.success("Stock actualizado");
      setStockModal(null);
      setStockQty(1);
      setStockOp("sumar");
      queryClient.invalidateQueries({ queryKey: ["products"] });
    },
    onError: (error) => toast.error(error.message)
  });

  const deleteMutation = useMutation({
    mutationFn: (id: string) => productsService.remove(id),
    onSuccess: () => {
      toast.success("Producto eliminado");
      queryClient.invalidateQueries({ queryKey: ["products"] });
    },
    onError: (error) => toast.error(error.message)
  });

  const stats = useMemo(() => {
    const items = productsQuery.data?.items ?? [];
    return {
      catalogo: productsQuery.data?.total ?? 0,
      visibles: items.length,
      bajoStock: lowStockQuery.data?.length ?? 0
    };
  }, [productsQuery.data, lowStockQuery.data]);

  if (productsQuery.isLoading) return <LoadingBlock />;

  return (
    <div className="grid gap-6">
      <SectionHeading
        title="Productos"
        subtitle="Administra inventario, filtra por tipo o material y ajusta stock rápidamente."
        actions={
          isAdmin ? (
            <Button onClick={() => setOpenCreate(true)}>
              <PackagePlus className="mr-2 h-4 w-4" />
              Nuevo producto
            </Button>
          ) : undefined
        }
      />

      <div className="grid gap-4 md:grid-cols-3">
        <StatCard label="Catálogo total" value={String(stats.catalogo)} helper="Productos registrados en el módulo" />
        <StatCard label="En pantalla" value={String(stats.visibles)} helper="Resultados de la búsqueda actual" />
        <StatCard label="Bajo stock" value={String(stats.bajoStock)} helper="Productos que requieren atención" />
      </div>

      <Card>
        <div className="grid gap-3 md:grid-cols-2 xl:grid-cols-5">
          <Input
            placeholder="Buscar por código o nombre"
            value={search}
            onChange={(event) => {
              setSearch(event.target.value);
              setPage(1);
            }}
          />
          <Select value={tipo} onChange={(event) => setTipo(event.target.value)}>
            <option value="">Todos los tipos</option>
            {PRODUCT_TYPES.map((option) => (
              <option key={option} value={option}>{option}</option>
            ))}
          </Select>
          <Select value={material} onChange={(event) => setMaterial(event.target.value)}>
            <option value="">Todos los materiales</option>
            {PRODUCT_MATERIALS.map((option) => (
              <option key={option} value={option}>{option}</option>
            ))}
          </Select>
          <Select value={estado} onChange={(event) => setEstado(event.target.value)}>
            <option value="">Todos los estados</option>
            {PRODUCT_STATES.map((option) => (
              <option key={option} value={option}>{option}</option>
            ))}
          </Select>
          <Button variant="secondary" onClick={() => { setSearch(""); setTipo(""); setMaterial(""); setEstado(""); setPage(1); }}>
            Limpiar filtros
          </Button>
        </div>
      </Card>

      {(productsQuery.data?.items ?? []).length ? (
        <Table headers={["Producto", "Tipo", "Precio", "Stock", "Estado", "Acciones"]}>
          {productsQuery.data?.items.map((product) => (
            <TableRow key={product.id}>
              <Cell>
                <div>
                  <p className="font-semibold text-slate-900">{product.nombre}</p>
                  <p className="mt-1 text-xs text-slate-500">{product.codigo} · {product.material}</p>
                </div>
              </Cell>
              <Cell>{product.tipo}</Cell>
              <Cell>{formatCurrency(product.precioVenta)}</Cell>
              <Cell>
                <span className={product.stockActual <= 3 ? "font-semibold text-amber-700" : ""}>
                  {product.stockActual}
                </span>
              </Cell>
              <Cell>
                <Badge tone={product.estado === "Activo" ? "emerald" : product.estado === "Agotado" ? "amber" : "rose"}>
                  {product.estado}
                </Badge>
              </Cell>
              <Cell className="space-x-2">
                {isAdmin ? (
                  <>
                    <Button
                      variant="secondary"
                      onClick={() => setStockModal({ id: product.id, nombre: product.nombre })}
                    >
                      <PencilLine className="mr-2 h-4 w-4" />
                      Stock
                    </Button>
                    <Button
                      variant="danger"
                      onClick={() => {
                        const confirmed = window.confirm(`¿Eliminar ${product.nombre}?`);
                        if (confirmed) deleteMutation.mutate(product.id);
                      }}
                    >
                      <Trash2 className="mr-2 h-4 w-4" />
                      Eliminar
                    </Button>
                  </>
                ) : (
                  <span className="text-xs text-slate-400">Solo lectura</span>
                )}
              </Cell>
            </TableRow>
          ))}
        </Table>
      ) : (
        <EmptyState title="No se encontraron productos" description="Prueba con otra búsqueda o crea el primer producto del catálogo." />
      )}

      <div className="flex items-center justify-between">
        <p className="text-sm text-slate-500">
          Página {productsQuery.data?.pagina ?? 1} de {productsQuery.data?.totalPaginas ?? 1}
        </p>
        <div className="flex gap-3">
          <Button variant="secondary" disabled={page <= 1} onClick={() => setPage((value) => Math.max(1, value - 1))}>
            Anterior
          </Button>
          <Button
            variant="secondary"
            disabled={page >= (productsQuery.data?.totalPaginas ?? 1)}
            onClick={() => setPage((value) => value + 1)}
          >
            Siguiente
          </Button>
        </div>
      </div>

      <Modal
        open={openCreate}
        onClose={() => setOpenCreate(false)}
        title="Nuevo producto"
        description="Formulario completo para registrar una pieza nueva en inventario."
      >
        <div className="grid gap-4 md:grid-cols-2">
          <Field label="Código" required>
            <Input value={form.codigo} onChange={(e) => setForm((f) => ({ ...f, codigo: e.target.value }))} />
          </Field>
          <Field label="Nombre" required>
            <Input value={form.nombre} onChange={(e) => setForm((f) => ({ ...f, nombre: e.target.value }))} />
          </Field>
          <Field label="Tipo" required>
            <Select value={form.tipo} onChange={(e) => setForm((f) => ({ ...f, tipo: e.target.value }))}>
              {PRODUCT_TYPES.map((option) => (
                <option key={option} value={option}>{option}</option>
              ))}
            </Select>
          </Field>
          <Field label="Material" required>
            <Select value={form.material} onChange={(e) => setForm((f) => ({ ...f, material: e.target.value }))}>
              {PRODUCT_MATERIALS.map((option) => (
                <option key={option} value={option}>{option}</option>
              ))}
            </Select>
          </Field>
          <Field label="Precio costo" required>
            <Input type="number" min="0" step="0.01" value={form.precioCosto} onChange={(e) => setForm((f) => ({ ...f, precioCosto: Number(e.target.value) }))} />
          </Field>
          <Field label="Precio venta" required>
            <Input type="number" min="0" step="0.01" value={form.precioVenta} onChange={(e) => setForm((f) => ({ ...f, precioVenta: Number(e.target.value) }))} />
          </Field>
          <Field label="Stock inicial" required>
            <Input type="number" min="0" value={form.stockInicial} onChange={(e) => setForm((f) => ({ ...f, stockInicial: Number(e.target.value) }))} />
          </Field>
          <Field label="Stock mínimo" required>
            <Input type="number" min="0" value={form.stockMinimo} onChange={(e) => setForm((f) => ({ ...f, stockMinimo: Number(e.target.value) }))} />
          </Field>
          <Field label="Categoría ID" required hint="Campo técnico hasta exponer endpoint de categorías.">
            <Input value={form.categoriaId} onChange={(e) => setForm((f) => ({ ...f, categoriaId: e.target.value }))} />
          </Field>
          <Field label="Proveedor ID" hint="Opcional.">
            <Input value={form.proveedorId ?? ""} onChange={(e) => setForm((f) => ({ ...f, proveedorId: e.target.value }))} />
          </Field>
          <Field label="Peso (gramos)">
            <Input type="number" min="0" step="0.01" value={form.pesoGramos ?? ""} onChange={(e) => setForm((f) => ({ ...f, pesoGramos: e.target.value ? Number(e.target.value) : undefined }))} />
          </Field>
          <div className="md:col-span-2">
            <Field label="Descripción">
              <Textarea value={form.descripcion ?? ""} onChange={(e) => setForm((f) => ({ ...f, descripcion: e.target.value }))} />
            </Field>
          </div>
        </div>

        <div className="mt-6 flex justify-end gap-3">
          <Button variant="secondary" onClick={() => setOpenCreate(false)}>Cancelar</Button>
          <Button
            onClick={() => createMutation.mutate()}
            disabled={createMutation.isPending || !form.codigo || !form.nombre || !form.categoriaId}
          >
            {createMutation.isPending ? <span className="inline-flex items-center gap-2"><Spinner />Guardando...</span> : "Guardar producto"}
          </Button>
        </div>
      </Modal>

      <Modal
        open={Boolean(stockModal)}
        onClose={() => setStockModal(null)}
        title="Ajustar stock"
        description={stockModal ? `Actualizar inventario de ${stockModal.nombre}.` : undefined}
        className="max-w-xl"
      >
        <div className="grid gap-4">
          <Field label="Operación" required>
            <Select value={stockOp} onChange={(e) => setStockOp(e.target.value as "sumar" | "restar")}>
              <option value="sumar">Sumar</option>
              <option value="restar">Restar</option>
            </Select>
          </Field>
          <Field label="Cantidad" required>
            <Input type="number" min="1" value={stockQty} onChange={(e) => setStockQty(Number(e.target.value))} />
          </Field>
        </div>

        <div className="mt-6 flex justify-end gap-3">
          <Button variant="secondary" onClick={() => setStockModal(null)}>Cancelar</Button>
          <Button onClick={() => adjustStockMutation.mutate()} disabled={adjustStockMutation.isPending}>
            {adjustStockMutation.isPending ? <span className="inline-flex items-center gap-2"><Spinner />Actualizando...</span> : "Guardar ajuste"}
          </Button>
        </div>
      </Modal>
    </div>
  );
}
