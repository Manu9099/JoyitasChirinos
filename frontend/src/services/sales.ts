import { http } from "./http";
import type { CreateSalePayload, PaginatedResponse, SaleSummary } from "../types/api";

export const salesService = {
  async list(params: {
    desde?: string;
    hasta?: string;
    clienteId?: string;
    metodoPago?: string;
    anulada?: boolean;
    pagina?: number;
    tamanoPagina?: number;
  }) {
    const { data } = await http.get<PaginatedResponse<SaleSummary>>("/api/ventas", { params });
    return data;
  },
  async create(payload: CreateSalePayload) {
    const { data } = await http.post<{ id: string }>("/api/ventas", payload);
    return data;
  },
  async cancel(id: string) {
    await http.patch(`/api/ventas/${id}/anular`);
  }
};
