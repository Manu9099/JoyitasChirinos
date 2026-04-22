import { http } from "./http";
import type { CreateOrderPayload, OrderSummary, PaginatedResponse } from "../types/api";

export const ordersService = {
  async list(params: {
    busqueda?: string;
    estado?: string;
    clienteId?: string;
    fechaEntregaDesde?: string;
    fechaEntregaHasta?: string;
    pagina?: number;
    tamanoPagina?: number;
  }) {
    const { data } = await http.get<PaginatedResponse<OrderSummary>>("/api/encargos", { params });
    return data;
  },
  async create(payload: CreateOrderPayload) {
    const { data } = await http.post<{ id: string }>("/api/encargos", payload);
    return data;
  },
  async changeStatus(id: string, estado: string) {
    await http.patch(`/api/encargos/${id}/estado`, { estado });
  }
};
