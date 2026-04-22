import { useState } from "react";
import { useMutation, useQuery, useQueryClient } from "@tanstack/react-query";
import { toast } from "sonner";
import { Plus, Trash2 } from "lucide-react";
import { clientsService } from "../services/clients";
import type { ClientPayload, ClientSummary } from "../types/api";
import { useAuthStore } from "../store/auth";
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
  Spinner,
  StatCard,
  Table,
  TableRow,
  Textarea
} from "../components/ui";

const initialForm: ClientPayload = {
  nombre: "",
  telefono: "",
  email: "",
  dni: "",
  notas: ""
};

export default function ClientsPage() {
  const queryClient = useQueryClient();
  const isAdmin = useAuthStore((state) => state.user?.rol === "Admin");
  const [search, setSearch] = useState("");
  const [page, setPage] = useState(1);
  const [openCreate, setOpenCreate] = useState(false);
  const [editingClient, setEditingClient] = useState<ClientSummary | null>(null);
  const [form, setForm] = useState<ClientPayload>(initialForm);

  const clientsQuery = useQuery({
    queryKey: ["clients", { search, page }],
    queryFn: () => clientsService.list({ busqueda: search || undefined, pagina: page, tamanoPagina: 10 })
  });

  const saveMutation = useMutation({
    mutationFn: async () => {
      if (editingClient) {
        await clientsService.update(editingClient.id, form);
      } else {
        await clientsService.create(form);
      }
    },
    onSuccess: () => {
      toast.success(editingClient ? "Cliente actualizado" : "Cliente creado");
      setOpenCreate(false);
      setEditingClient(null);
      setForm(initialForm);
      queryClient.invalidateQueries({ queryKey: ["clients"] });
    },
    onError: (error) => toast.error(error.message)
  });

  const deleteMutation = useMutation({
    mutationFn: (id: string) => clientsService.remove(id),
    onSuccess: () => {
      toast.success("Cliente eliminado");
      queryClient.invalidateQueries({ queryKey: ["clients"] });
    },
    onError: (error) => toast.error(error.message)
  });

  if (clientsQuery.isLoading) return <LoadingBlock />;

  return (
    <div className="grid gap-6">
      <SectionHeading
        title="Clientes"
        subtitle="Mantén una base de clientes limpia para ventas, seguimiento y fidelización."
        actions={
          <Button
            onClick={() => {
              setEditingClient(null);
              setForm(initialForm);
              setOpenCreate(true);
            }}
          >
            <Plus className="mr-2 h-4 w-4" />
            Nuevo cliente
          </Button>
        }
      />

      <div className="grid gap-4 md:grid-cols-3">
        <StatCard label="Clientes totales" value={String(clientsQuery.data?.total ?? 0)} helper="Base completa disponible" />
        <StatCard label="Página actual" value={String(clientsQuery.data?.items.length ?? 0)} helper="Resultados visibles ahora" />
        <StatCard label="Clientes VIP" value={String((clientsQuery.data?.items ?? []).filter((item) => item.puntosFidelidad > 0).length)} helper="Con puntos acumulados en esta consulta" />
      </div>

      <div className="rounded-[28px] border border-slate-200 bg-white p-4">
        <Input
          placeholder="Busca por nombre, teléfono, email o DNI"
          value={search}
          onChange={(event) => {
            setSearch(event.target.value);
            setPage(1);
          }}
        />
      </div>

      {(clientsQuery.data?.items ?? []).length ? (
        <Table headers={["Cliente", "Contacto", "Documento", "Fidelidad", "Acciones"]}>
          {clientsQuery.data?.items.map((client) => (
            <TableRow key={client.id}>
              <Cell>
                <div>
                  <p className="font-semibold text-slate-900">{client.nombre}</p>
                  <p className="mt-1 text-xs text-slate-500">{client.email || "Sin correo"}</p>
                </div>
              </Cell>
              <Cell>{client.telefono || "—"}</Cell>
              <Cell>{client.dni || "—"}</Cell>
              <Cell>
                <Badge tone={client.puntosFidelidad > 0 ? "amber" : "slate"}>
                  {client.puntosFidelidad} pts
                </Badge>
              </Cell>
              <Cell className="space-x-2">
                <Button
                  variant="secondary"
                  onClick={async () => {
                    try {
                      const detail = await clientsService.detail(client.id);
                      setEditingClient(client);
                      setForm({
                        nombre: detail.nombre,
                        telefono: detail.telefono ?? "",
                        email: detail.email ?? "",
                        dni: detail.dni ?? "",
                        notas: detail.notas ?? ""
                      });
                      setOpenCreate(true);
                    } catch (error) {
                      toast.error(error instanceof Error ? error.message : "No se pudo cargar el cliente");
                    }
                  }}
                >
                  Editar
                </Button>
                {isAdmin ? (
                  <Button
                    variant="danger"
                    onClick={() => {
                      const confirmed = window.confirm(`¿Eliminar a ${client.nombre}?`);
                      if (confirmed) deleteMutation.mutate(client.id);
                    }}
                  >
                    <Trash2 className="mr-2 h-4 w-4" />
                    Eliminar
                  </Button>
                ) : null}
              </Cell>
            </TableRow>
          ))}
        </Table>
      ) : (
        <EmptyState title="Sin clientes" description="Empieza agregando clientes para conectar ventas y encargos." />
      )}

      <div className="flex items-center justify-between">
        <p className="text-sm text-slate-500">
          Página {clientsQuery.data?.pagina ?? 1} de {clientsQuery.data?.totalPaginas ?? 1}
        </p>
        <div className="flex gap-3">
          <Button variant="secondary" disabled={page <= 1} onClick={() => setPage((value) => Math.max(1, value - 1))}>
            Anterior
          </Button>
          <Button
            variant="secondary"
            disabled={page >= (clientsQuery.data?.totalPaginas ?? 1)}
            onClick={() => setPage((value) => value + 1)}
          >
            Siguiente
          </Button>
        </div>
      </div>

      <Modal
        open={openCreate}
        onClose={() => setOpenCreate(false)}
        title={editingClient ? "Editar cliente" : "Nuevo cliente"}
        description="Mantén un registro limpio y utilizable para todo el flujo comercial."
      >
        <div className="grid gap-4 md:grid-cols-2">
          <Field label="Nombre" required>
            <Input value={form.nombre} onChange={(e) => setForm((f) => ({ ...f, nombre: e.target.value }))} />
          </Field>
          <Field label="Teléfono">
            <Input value={form.telefono ?? ""} onChange={(e) => setForm((f) => ({ ...f, telefono: e.target.value }))} />
          </Field>
          <Field label="Email">
            <Input value={form.email ?? ""} onChange={(e) => setForm((f) => ({ ...f, email: e.target.value }))} />
          </Field>
          <Field label="DNI">
            <Input value={form.dni ?? ""} onChange={(e) => setForm((f) => ({ ...f, dni: e.target.value }))} />
          </Field>
          <div className="md:col-span-2">
            <Field label="Notas">
              <Textarea value={form.notas ?? ""} onChange={(e) => setForm((f) => ({ ...f, notas: e.target.value }))} />
            </Field>
          </div>
        </div>

        <div className="mt-6 flex justify-end gap-3">
          <Button variant="secondary" onClick={() => setOpenCreate(false)}>Cancelar</Button>
          <Button onClick={() => saveMutation.mutate()} disabled={saveMutation.isPending || !form.nombre}>
            {saveMutation.isPending ? <span className="inline-flex items-center gap-2"><Spinner />Guardando...</span> : "Guardar cliente"}
          </Button>
        </div>
      </Modal>
    </div>
  );
}
