import { format } from "date-fns";

export function formatCurrency(value: number) {
  return new Intl.NumberFormat("es-PE", {
    style: "currency",
    currency: "PEN",
    minimumFractionDigits: 2
  }).format(value);
}

export function formatDate(value?: string | Date | null) {
  if (!value) return "—";
  return format(new Date(value), "dd/MM/yyyy");
}

export function formatDateTime(value?: string | Date | null) {
  if (!value) return "—";
  return format(new Date(value), "dd/MM/yyyy HH:mm");
}

export function safeNumber(value: number | string | null | undefined) {
  return Number(value ?? 0);
}
