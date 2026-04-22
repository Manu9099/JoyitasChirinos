import { http } from "./http";
import type {
  ClientDetail,
  ClientPayload,
  ClientSummary,
  PaginatedResponse
} from "../types/api";

export const clientsService = {
  async list(params: { busqueda?: string; pagina?: number; tamanoPagina?: number }) {
    const { data } = await http.get<PaginatedResponse<ClientSummary>>("/api/clientes", { params });
    return data;
  },
  async detail(id: string) {
    const { data } = await http.get<ClientDetail>(`/api/clientes/${id}`);
    return data;
  },
  async create(payload: ClientPayload) {
    const { data } = await http.post<{ id: string }>("/api/clientes", payload);
    return data;
  },
  async update(id: string, payload: ClientPayload) {
    await http.put(`/api/clientes/${id}`, payload);
  },
  async remove(id: string) {
    await http.delete(`/api/clientes/${id}`);
  }
};
