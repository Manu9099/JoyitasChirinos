import { useState } from "react";
import { useMutation, useQuery, useQueryClient } from "@tanstack/react-query";
import { toast } from "sonner";
import { Plus } from "lucide-react";
import { clientsService } from "../services/clients";
import { ordersService } from "../services/orders";
import { ORDER_STATES, PRODUCT_MATERIALS } from "../types/domain";
import { formatCurrency, formatDate } from "../lib/format";
import {
  Badge,
  Button,
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

type OrderForm = {
  numero: number;
  clienteId: string;
  descripcion: string;
  material: string;
  pesoEstimado: number | "";
  precioAcordado: number;
  adelanto: number;
  fechaEntrega: string;
  fotoReferenciaUrl: string;
  notas: string;
};

const initialForm: OrderForm = {
  numero: 1,
  clienteId: "",
  descripcion: "",
  material: "Oro18k",
  pesoEstimado: "",
  precioAcordado: 0,
  adelanto: 0,
  fechaEntrega: "",
  fotoReferenciaUrl: "",
  notas: ""
};

export default function OrdersPage() {
  const queryClient = useQueryClient();
  const [openCreate, setOpenCreate] = useState(false);
  const [search, setSearch] = useState("");
  const [estado, setEstado] = useState("");
  const [page, setPage] = useState(1);
  const [form, setForm] = useState<OrderForm>(initialForm);

  const ordersQuery = useQuery({
    queryKey: ["orders", { search, estado, page }],
    queryFn: () =>
      ordersService.list({
        busqueda: search || undefined,
        estado: estado || undefined,
        pagina: page,
        tamanoPagina: 10
      })
  });

  const clientsQuery = useQuery({
    queryKey: ["orders", "clients-selector"],
    queryFn: () => clientsService.list({ pagina: 1, tamanoPagina: 100 })
  });

  const createMutation = useMutation({
    mutationFn: () =>
      ordersService.create({
        numero: Number(form.numero),
        clienteId: form.clienteId,
        descripcion: form.descripcion,
        material: form.material,
        pesoEstimado: form.pesoEstimado === "" ? null : Number(form.pesoEstimado),
        precioAcordado: Number(form.precioAcordado),
        adelanto: Number(form.adelanto),
        fechaEntrega: form.fechaEntrega || null,
        fotoReferenciaUrl: form.fotoReferenciaUrl || null,
        notas: form.notas || null
      }),
    onSuccess: () => {
      toast.success("Encargo creado");
      setOpenCreate(false);
      setForm(initialForm);
      queryClient.invalidateQueries({ queryKey: ["orders"] });
    },
    onError: (error) => toast.error(error.message)
  });

  const changeStatusMutation = useMutation({
    mutationFn: ({ id, value }: { id: string; value: string }) => ordersService.changeStatus(id, value),
    onSuccess: () => {
      toast.success("Estado actualizado");
      queryClient.invalidateQueries({ queryKey: ["orders"] });
    },
    onError: (error) => toast.error(error.message)
  });

  if (ordersQuery.isLoading || clientsQuery.isLoading) {
    return <LoadingBlock />;
  }

  return (
    <div className="grid gap-6">
      <SectionHeading
        title="Encargos"
        subtitle="Controla pedidos personalizados, saldos pendientes y estado de producción."
        actions={
          <Button onClick={() => setOpenCreate(true)}>
            <Plus className="mr-2 h-4 w-4" />
            Nuevo encargo
          </Button>
        }
      />

      <div className="grid gap-4 md:grid-cols-3">
        <StatCard label="Encargos totales" value={String(ordersQuery.data?.total ?? 0)} helper="Total encontrado con los filtros actuales" />
        <StatCard label="Pendientes" value={String((ordersQuery.data?.items ?? []).filter((item) => item.estado === "Pendiente").length)} helper="Necesitan arranque o seguimiento" />
        <StatCard label="Saldo pendiente" value={formatCurrency((ordersQuery.data?.items ?? []).reduce((sum, item) => sum + item.saldoPendiente, 0))} helper="Suma de la página actual" />
      </div>

      <div className="rounded-[28px] border border-slate-200 bg-white p-4">
        <div className="grid gap-3 md:grid-cols-[1fr_220px]">
          <Input
            placeholder="Busca por cliente o descripción"
            value={search}
            onChange={(event) => {
              setSearch(event.target.value);
              setPage(1);
            }}
          />
          <Select value={estado} onChange={(event) => setEstado(event.target.value)}>
            <option value="">Todos los estados</option>
            {ORDER_STATES.map((option) => (
              <option key={option} value={option}>{option}</option>
            ))}
          </Select>
        </div>
      </div>

      {(ordersQuery.data?.items ?? []).length ? (
        <Table headers={["Encargo", "Cliente", "Entrega", "Finanzas", "Estado", "Acciones"]}>
          {ordersQuery.data?.items.map((order) => (
            <TableRow key={order.id}>
              <Cell>
                <div>
                  <p className="font-semibold text-slate-900">#{order.numero}</p>
                  <p className="mt-1 text-xs text-slate-500">{order.descripcion}</p>
                </div>
              </Cell>
              <Cell>{order.clienteNombre}</Cell>
              <Cell>{formatDate(order.fechaEntrega)}</Cell>
              <Cell>
                <div>
                  <p className="font-medium text-slate-900">{formatCurrency(order.precioAcordado)}</p>
                  <p className="mt-1 text-xs text-slate-500">Saldo: {formatCurrency(order.saldoPendiente)}</p>
                </div>
              </Cell>
              <Cell>
                <Badge tone={order.estado === "Pendiente" ? "amber" : order.estado === "Entregado" ? "emerald" : "violet"}>
                  {order.estado}
                </Badge>
              </Cell>
              <Cell>
                <Select
                  value={order.estado}
                  onChange={(event) =>
                    changeStatusMutation.mutate({ id: order.id, value: event.target.value })
                  }
                >
                  {ORDER_STATES.map((option) => (
                    <option key={option} value={option}>{option}</option>
                  ))}
                </Select>
              </Cell>
            </TableRow>
          ))}
        </Table>
      ) : (
        <EmptyState title="Sin encargos" description="Aún no hay registros de trabajos personalizados en el sistema." />
      )}

      <div className="flex items-center justify-between">
        <p className="text-sm text-slate-500">
          Página {ordersQuery.data?.pagina ?? 1} de {ordersQuery.data?.totalPaginas ?? 1}
        </p>
        <div className="flex gap-3">
          <Button variant="secondary" disabled={page <= 1} onClick={() => setPage((value) => Math.max(1, value - 1))}>
            Anterior
          </Button>
          <Button
            variant="secondary"
            disabled={page >= (ordersQuery.data?.totalPaginas ?? 1)}
            onClick={() => setPage((value) => value + 1)}
          >
            Siguiente
          </Button>
        </div>
      </div>

      <Modal
        open={openCreate}
        onClose={() => setOpenCreate(false)}
        title="Nuevo encargo"
        description="Registra piezas personalizadas con precio acordado, adelanto y fecha de entrega."
      >
        <div className="grid gap-4 md:grid-cols-2">
          <Field label="Número" required>
            <Input type="number" min="1" value={form.numero} onChange={(e) => setForm((f) => ({ ...f, numero: Number(e.target.value) }))} />
          </Field>
          <Field label="Cliente" required>
            <Select value={form.clienteId} onChange={(e) => setForm((f) => ({ ...f, clienteId: e.target.value }))}>
              <option value="">Selecciona un cliente</option>
              {clientsQuery.data?.items.map((client) => (
                <option key={client.id} value={client.id}>{client.nombre}</option>
              ))}
            </Select>
          </Field>
          <div className="md:col-span-2">
            <Field label="Descripción" required>
              <Textarea value={form.descripcion} onChange={(e) => setForm((f) => ({ ...f, descripcion: e.target.value }))} />
            </Field>
          </div>
          <Field label="Material" required>
            <Select value={form.material} onChange={(e) => setForm((f) => ({ ...f, material: e.target.value }))}>
              {PRODUCT_MATERIALS.map((option) => (
                <option key={option} value={option}>{option}</option>
              ))}
            </Select>
          </Field>
          <Field label="Peso estimado (g)">
            <Input type="number" min="0" step="0.01" value={form.pesoEstimado} onChange={(e) => setForm((f) => ({ ...f, pesoEstimado: e.target.value ? Number(e.target.value) : "" }))} />
          </Field>
          <Field label="Precio acordado" required>
            <Input type="number" min="0" step="0.01" value={form.precioAcordado} onChange={(e) => setForm((f) => ({ ...f, precioAcordado: Number(e.target.value) }))} />
          </Field>
          <Field label="Adelanto" required>
            <Input type="number" min="0" step="0.01" value={form.adelanto} onChange={(e) => setForm((f) => ({ ...f, adelanto: Number(e.target.value) }))} />
          </Field>
          <Field label="Fecha de entrega">
            <Input type="date" value={form.fechaEntrega} onChange={(e) => setForm((f) => ({ ...f, fechaEntrega: e.target.value }))} />
          </Field>
          <Field label="Foto de referencia URL">
            <Input value={form.fotoReferenciaUrl} onChange={(e) => setForm((f) => ({ ...f, fotoReferenciaUrl: e.target.value }))} />
          </Field>
          <div className="md:col-span-2">
            <Field label="Notas">
              <Textarea value={form.notas} onChange={(e) => setForm((f) => ({ ...f, notas: e.target.value }))} />
            </Field>
          </div>
        </div>

        <div className="mt-6 flex justify-end gap-3">
          <Button variant="secondary" onClick={() => setOpenCreate(false)}>Cancelar</Button>
          <Button onClick={() => createMutation.mutate()} disabled={createMutation.isPending || !form.clienteId || !form.descripcion}>
            {createMutation.isPending ? <span className="inline-flex items-center gap-2"><Spinner />Guardando...</span> : "Guardar encargo"}
          </Button>
        </div>
      </Modal>
    </div>
  );
}
