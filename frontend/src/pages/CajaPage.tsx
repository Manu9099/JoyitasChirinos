import { useMemo, useState } from "react";
import { useMutation, useQuery, useQueryClient } from "@tanstack/react-query";
import { toast } from "sonner";
import { DoorClosed, DoorOpen, Wallet } from "lucide-react";
import { cashService } from "../services/cash";
import { formatCurrency, formatDateTime } from "../lib/format";
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
  Spinner,
  StatCard,
  Table,
  TableRow,
  Textarea
} from "../components/ui";

export default function CajaPage() {
  const queryClient = useQueryClient();
  const [page, setPage] = useState(1);
  const [openApertura, setOpenApertura] = useState(false);
  const [openCierre, setOpenCierre] = useState(false);
  const [montoInicial, setMontoInicial] = useState(0);
  const [observacionesApertura, setObservacionesApertura] = useState("");
  const [montoFinalContado, setMontoFinalContado] = useState(0);
  const [observacionesCierre, setObservacionesCierre] = useState("");

  const currentQuery = useQuery({
    queryKey: ["cash", "current"],
    queryFn: () => cashService.current(),
    retry: false
  });

  const historyQuery = useQuery({
    queryKey: ["cash", "history", page],
    queryFn: () => cashService.history(page, 10)
  });

  const openMutation = useMutation({
    mutationFn: () =>
      cashService.open({
        montoInicial: Number(montoInicial),
        observaciones: observacionesApertura || null
      }),
    onSuccess: () => {
      toast.success("Caja abierta correctamente");
      setOpenApertura(false);
      setMontoInicial(0);
      setObservacionesApertura("");
      queryClient.invalidateQueries({ queryKey: ["cash"] });
    },
    onError: (error) => toast.error(error.message)
  });

  const closeMutation = useMutation({
    mutationFn: () =>
      cashService.close({
        montoFinalContado: Number(montoFinalContado),
        observaciones: observacionesCierre || null
      }),
    onSuccess: () => {
      toast.success("Caja cerrada correctamente");
      setOpenCierre(false);
      setMontoFinalContado(0);
      setObservacionesCierre("");
      queryClient.invalidateQueries({ queryKey: ["cash"] });
    },
    onError: (error) => toast.error(error.message)
  });

  const cajaActual = currentQuery.data;

  const stats = useMemo(() => ({
    montoEsperado: cajaActual?.montoEsperado ?? 0,
    totalVentas: cajaActual?.totalVentasGeneral ?? 0,
    ingresosManuales: cajaActual?.totalIngresosManuales ?? 0,
    egresosManuales: cajaActual?.totalEgresosManuales ?? 0
  }), [cajaActual]);

  if (currentQuery.isLoading || historyQuery.isLoading) {
    return <LoadingBlock />;
  }

  return (
    <div className="grid gap-6">
      <SectionHeading
        title="Caja"
        subtitle="Este módulo merece menú propio porque tu backend ya maneja apertura, caja actual, cierre e historial operativo."
        actions={
          cajaActual?.abierta ? (
            <Button onClick={() => { setMontoFinalContado(Number(cajaActual.montoEsperado ?? 0)); setOpenCierre(true); }}>
              <DoorClosed className="mr-2 h-4 w-4" />
              Cerrar caja
            </Button>
          ) : (
            <Button onClick={() => setOpenApertura(true)}>
              <DoorOpen className="mr-2 h-4 w-4" />
              Abrir caja
            </Button>
          )
        }
      />

      <div className="grid gap-4 md:grid-cols-4">
        <StatCard label="Estado" value={cajaActual?.abierta ? "Abierta" : "Cerrada"} helper="Sesión operativa actual" />
        <StatCard label="Monto esperado" value={formatCurrency(stats.montoEsperado)} helper="Calculado por backend" />
        <StatCard label="Ventas del turno" value={formatCurrency(stats.totalVentas)} helper="Suma por métodos de pago" />
        <StatCard label="Movimientos" value={String(cajaActual?.movimientos?.length ?? 0)} helper="Ingresos y egresos manuales" />
      </div>

      {cajaActual?.abierta ? (
        <div className="grid gap-6 xl:grid-cols-[1.2fr_0.8fr]">
          <Card>
            <div className="flex items-center justify-between">
              <div>
                <h3 className="text-lg font-semibold text-slate-950">Caja actual</h3>
                <p className="mt-1 text-sm text-slate-500">Abierta desde {formatDateTime(cajaActual.fechaApertura)}</p>
              </div>
              <Badge tone="emerald">Operativa</Badge>
            </div>

            <div className="mt-6 grid gap-4 md:grid-cols-2">
              <div className="rounded-2xl border border-slate-200 p-4">
                <p className="text-xs uppercase tracking-[0.18em] text-slate-400">Monto inicial</p>
                <p className="mt-3 text-2xl font-semibold text-slate-950">{formatCurrency(cajaActual.montoInicial)}</p>
                <p className="mt-2 text-sm text-slate-500">Observación: {cajaActual.observacionesApertura || "Sin observaciones"}</p>
              </div>

              <div className="rounded-2xl border border-slate-200 p-4">
                <p className="text-xs uppercase tracking-[0.18em] text-slate-400">Desglose ventas</p>
                <div className="mt-3 grid gap-2 text-sm text-slate-600">
                  <div className="flex items-center justify-between"><span>Efectivo</span><strong>{formatCurrency(cajaActual.ventasEfectivo)}</strong></div>
                  <div className="flex items-center justify-between"><span>Yape</span><strong>{formatCurrency(cajaActual.ventasYape)}</strong></div>
                  <div className="flex items-center justify-between"><span>Plin</span><strong>{formatCurrency(cajaActual.ventasPlin)}</strong></div>
                  <div className="flex items-center justify-between"><span>Tarjeta</span><strong>{formatCurrency(cajaActual.ventasTarjeta)}</strong></div>
                  <div className="flex items-center justify-between"><span>Transferencia</span><strong>{formatCurrency(cajaActual.ventasTransferencia)}</strong></div>
                  <div className="flex items-center justify-between"><span>Otros</span><strong>{formatCurrency(cajaActual.ventasOtros)}</strong></div>
                </div>
              </div>
            </div>

            <div className="mt-6 rounded-2xl border border-slate-200 p-4">
              <div className="flex items-center gap-3">
                <div className="rounded-2xl bg-amber-100 p-3 text-amber-700">
                  <Wallet className="h-5 w-5" />
                </div>
                <div>
                  <p className="text-xs uppercase tracking-[0.18em] text-slate-400">Balance operativo</p>
                  <p className="mt-1 text-sm text-slate-500">Incluye ventas y movimientos manuales</p>
                </div>
              </div>
              <div className="mt-4 grid gap-2 text-sm">
                <div className="flex items-center justify-between"><span className="text-slate-500">Ingresos manuales</span><strong>{formatCurrency(stats.ingresosManuales)}</strong></div>
                <div className="flex items-center justify-between"><span className="text-slate-500">Egresos manuales</span><strong>{formatCurrency(stats.egresosManuales)}</strong></div>
                <div className="flex items-center justify-between border-t border-slate-200 pt-3"><span className="text-slate-500">Monto esperado</span><strong className="text-slate-950">{formatCurrency(stats.montoEsperado)}</strong></div>
              </div>
            </div>
          </Card>

          <Card>
            <div className="flex items-center justify-between">
              <h3 className="text-lg font-semibold text-slate-950">Movimientos recientes</h3>
              <Badge tone="amber">{cajaActual.movimientos?.length ?? 0}</Badge>
            </div>
            <div className="mt-4 grid gap-3">
              {(cajaActual.movimientos ?? []).length ? (
                cajaActual.movimientos.map((mov) => (
                  <div key={mov.id} className="rounded-2xl border border-slate-200 p-4">
                    <div className="flex items-center justify-between gap-3">
                      <div>
                        <p className="font-semibold text-slate-900">{mov.motivo}</p>
                        <p className="mt-1 text-xs text-slate-500">{formatDateTime(mov.fechaMovimiento)}</p>
                      </div>
                      <Badge tone={String(mov.tipo).toLowerCase().includes("egre") || String(mov.tipo) === "1" ? "rose" : "emerald"}>
                        {String(mov.tipo)}
                      </Badge>
                    </div>
                    <div className="mt-3 flex items-center justify-between text-sm">
                      <span className="text-slate-500">Monto</span>
                      <strong>{formatCurrency(mov.monto)}</strong>
                    </div>
                    {mov.observaciones ? (
                      <p className="mt-2 text-sm text-slate-500">{mov.observaciones}</p>
                    ) : null}
                  </div>
                ))
              ) : (
                <EmptyState title="Sin movimientos manuales" description="Todavía no se registran ingresos o egresos adicionales en la caja actual." />
              )}
            </div>
          </Card>
        </div>
      ) : (
        <EmptyState title="No hay caja abierta" description="Te conviene colocar Caja como módulo propio entre Dashboard y Ventas, porque controla el turno operativo antes del flujo comercial." />
      )}

      <Card>
        <div className="flex items-center justify-between">
          <div>
            <h3 className="text-lg font-semibold text-slate-950">Historial de caja</h3>
            <p className="mt-1 text-sm text-slate-500">Sesiones anteriores para auditoría rápida.</p>
          </div>
          <Badge tone="slate">{historyQuery.data?.total ?? 0} registros</Badge>
        </div>

        {(historyQuery.data?.items ?? []).length ? (
          <>
            <div className="mt-4">
              <Table headers={["Apertura", "Cierre", "Estado", "Inicial", "Esperado", "Diferencia"]}>
                {(historyQuery.data?.items ?? []).map((item) => (
                  <TableRow key={item.id}>
                    <Cell>{formatDateTime(item.fechaApertura)}</Cell>
                    <Cell>{formatDateTime(item.fechaCierre)}</Cell>
                    <Cell>
                      <Badge tone={item.abierta ? "amber" : "slate"}>
                        {item.abierta ? "Abierta" : "Cerrada"}
                      </Badge>
                    </Cell>
                    <Cell>{formatCurrency(item.montoInicial ?? 0)}</Cell>
                    <Cell>{formatCurrency(item.montoEsperado ?? 0)}</Cell>
                    <Cell>{formatCurrency(item.diferencia ?? 0)}</Cell>
                  </TableRow>
                ))}
              </Table>
            </div>

            <div className="mt-4 flex items-center justify-between">
              <p className="text-sm text-slate-500">
                Página {historyQuery.data?.pagina ?? 1} de {historyQuery.data?.totalPaginas ?? 1}
              </p>
              <div className="flex gap-3">
                <Button variant="secondary" disabled={page <= 1} onClick={() => setPage((value) => Math.max(1, value - 1))}>
                  Anterior
                </Button>
                <Button
                  variant="secondary"
                  disabled={page >= (historyQuery.data?.totalPaginas ?? 1)}
                  onClick={() => setPage((value) => value + 1)}
                >
                  Siguiente
                </Button>
              </div>
            </div>
          </>
        ) : (
          <div className="mt-4">
            <EmptyState title="Sin historial todavía" description="Cuando abras y cierres turnos de caja, aquí aparecerá el resumen histórico." />
          </div>
        )}
      </Card>

      <Modal
        open={openApertura}
        onClose={() => setOpenApertura(false)}
        title="Abrir caja"
        description="Inicia un nuevo turno de caja con el monto base disponible."
      >
        <div className="grid gap-4">
          <Field label="Monto inicial" required>
            <Input type="number" min="0" step="0.01" value={montoInicial} onChange={(e) => setMontoInicial(Number(e.target.value))} />
          </Field>
          <Field label="Observaciones">
            <Textarea value={observacionesApertura} onChange={(e) => setObservacionesApertura(e.target.value)} />
          </Field>
          <div className="flex justify-end gap-3">
            <Button variant="secondary" onClick={() => setOpenApertura(false)}>Cancelar</Button>
            <Button disabled={openMutation.isPending || montoInicial < 0} onClick={() => openMutation.mutate()}>
              {openMutation.isPending ? <Spinner /> : "Abrir caja"}
            </Button>
          </div>
        </div>
      </Modal>

      <Modal
        open={openCierre}
        onClose={() => setOpenCierre(false)}
        title="Cerrar caja"
        description="Contrasta el efectivo contado con el monto esperado del sistema."
      >
        <div className="grid gap-4">
          <Field label="Monto final contado" required>
            <Input type="number" min="0" step="0.01" value={montoFinalContado} onChange={(e) => setMontoFinalContado(Number(e.target.value))} />
          </Field>
          <Field label="Observaciones">
            <Textarea value={observacionesCierre} onChange={(e) => setObservacionesCierre(e.target.value)} />
          </Field>
          <div className="rounded-2xl border border-amber-200 bg-amber-50 p-4 text-sm text-amber-800">
            Monto esperado del sistema: <strong>{formatCurrency(cajaActual?.montoEsperado ?? 0)}</strong>
          </div>
          <div className="flex justify-end gap-3">
            <Button variant="secondary" onClick={() => setOpenCierre(false)}>Cancelar</Button>
            <Button disabled={closeMutation.isPending || montoFinalContado < 0} onClick={() => closeMutation.mutate()}>
              {closeMutation.isPending ? <Spinner /> : "Cerrar caja"}
            </Button>
          </div>
        </div>
      </Modal>
    </div>
  );
}
