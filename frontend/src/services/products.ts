import { http } from "./http";
import type {
  CreateProductPayload,
  PaginatedResponse,
  ProductDetail,
  ProductSummary
} from "../types/api";

export interface ProductFilters {
  tipo?: string;
  material?: string;
  estado?: string;
  busqueda?: string;
  pagina?: number;
  tamanoPagina?: number;
}

export const productsService = {
  async list(params: ProductFilters) {
    const { data } = await http.get<PaginatedResponse<ProductSummary>>("/api/productos", { params });
    return data;
  },
  async detail(id: string) {
    const { data } = await http.get<ProductDetail>(`/api/productos/${id}`);
    return data;
  },
  async lowStock() {
    const { data } = await http.get<ProductSummary[]>("/api/productos/bajo-stock");
    return data;
  },
  async create(payload: CreateProductPayload) {
    const { data } = await http.post<{ id: string }>("/api/productos", payload);
    return data;
  },
  async adjustStock(id: string, cantidad: number, operacion: "sumar" | "restar") {
    const operacionApi = operacion === "sumar" ? "agregar" : "retirar";
    await http.patch(`/api/productos/${id}/stock`, { cantidad, operacion: operacionApi });
  },
  async remove(id: string) {
    await http.delete(`/api/productos/${id}`);
  }
};
