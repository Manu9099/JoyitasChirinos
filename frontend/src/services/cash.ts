import { http } from "./http";
import type {
  CashCurrent,
  CashHistoryItem,
  CloseCashPayload,
  OpenCashPayload,
  PaginatedResponse
} from "../types/api";

export const cashService = {
  async current() {
    const { data } = await http.get<CashCurrent>("/api/caja/actual");
    return data;
  },
  async history(page = 1, tamanoPagina = 10) {
    const { data } = await http.get<PaginatedResponse<CashHistoryItem>>("/api/caja/historial", {
      params: { pagina: page, tamanoPagina }
    });
    return data;
  },
  async open(payload: OpenCashPayload) {
    const { data } = await http.post<{ id: string; mensaje: string }>("/api/caja/apertura", payload);
    return data;
  },
  async close(payload: CloseCashPayload) {
    const { data } = await http.post("/api/caja/cierre", payload);
    return data;
  }
};
