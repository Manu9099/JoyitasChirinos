import type { Rol } from "./domain";

export interface PaginatedResponse<T> {
  items: T[];
  total: number;
  pagina: number;
  tamanoPagina: number;
  totalPaginas: number;
}

export interface LoginRequest {
  email: string;
  password: string;
}

export interface LoginResponse {
  token: string;
  nombre: string;
  email: string;
  rol: Rol;
}

export interface MeResponse {
  id: string;
  nombre: string;
  email: string;
  rol: Rol;
}

export interface ProductSummary {
  id: string;
  codigo: string;
  nombre: string;
  tipo: string;
  material: string;
  precioVenta: number;
  stockActual: number;
  fotoUrl?: string | null;
  estado: string;
}

export interface ProductDetail extends ProductSummary {
  pesoGramos?: number | null;
  precioCosto: number;
  stockMinimo: number;
  tieneBajoStock: boolean;
  descripcion?: string | null;
  categoriaId: string;
  categoriaNombre: string;
  proveedorId?: string | null;
  proveedorNombre?: string | null;
  createdAt: string;
}

export interface CreateProductPayload {
  codigo: string;
  nombre: string;
  tipo: string;
  material: string;
  precioCosto: number;
  precioVenta: number;
  stockInicial: number;
  stockMinimo: number;
  categoriaId: string;
  proveedorId?: string | null;
  pesoGramos?: number | null;
  descripcion?: string | null;
}

export interface ClientSummary {
  id: string;
  nombre: string;
  telefono?: string | null;
  email?: string | null;
  dni?: string | null;
  puntosFidelidad: number;
}

export interface ClientDetail extends ClientSummary {
  notas?: string | null;
  createdAt: string;
}

export interface ClientPayload {
  nombre: string;
  telefono?: string | null;
  email?: string | null;
  dni?: string | null;
  notas?: string | null;
}

export interface SaleSummary {
  id: string;
  numero: number;
  fecha: string;
  clienteId?: string | null;
  clienteNombre?: string | null;
  usuarioId: string;
  subtotal: number;
  descuento: number;
  total: number;
  metodoPago: string;
  estado: string;
  anulada: boolean;
}

export interface SaleItemPayload {
  productoId: string;
  cantidad: number;
}

export interface CreateSalePayload {
  clienteId?: string | null;
  descuento: number;
  metodoPago: string;
  notas?: string | null;
  items: SaleItemPayload[];
}

export interface OrderSummary {
  id: string;
  numero: number;
  clienteId: string;
  clienteNombre?: string | null;
  usuarioId: string;
  descripcion: string;
  material: string;
  pesoEstimado?: number | null;
  precioAcordado: number;
  adelanto: number;
  saldoPendiente: number;
  estado: string;
  fechaEntrega?: string | null;
}

export interface CreateOrderPayload {
  numero: number;
  clienteId: string;
  descripcion: string;
  material: string;
  pesoEstimado?: number | null;
  precioAcordado: number;
  adelanto: number;
  fechaEntrega?: string | null;
  fotoReferenciaUrl?: string | null;
  notas?: string | null;
}


export interface CashMovement {
  id: string;
  tipo: number | string;
  monto: number;
  motivo: string;
  observaciones?: string | null;
  fechaMovimiento: string;
  usuarioId: string;
}

export interface CashCurrent {
  id: string;
  usuarioAperturaId: string;
  fechaApertura: string;
  montoInicial: number;
  abierta: boolean;
  observacionesApertura?: string | null;
  ventasEfectivo: number;
  ventasYape: number;
  ventasPlin: number;
  ventasTarjeta: number;
  ventasTransferencia: number;
  ventasOtros: number;
  totalVentasGeneral: number;
  totalIngresosManuales: number;
  totalEgresosManuales: number;
  montoEsperado: number;
  movimientos: CashMovement[];
}

export interface CashHistoryItem {
  id: string;
  fechaApertura?: string;
  fechaCierre?: string | null;
  montoInicial?: number;
  montoFinalContado?: number | null;
  montoEsperado?: number | null;
  diferencia?: number | null;
  abierta?: boolean;
  totalVentasGeneral?: number;
}

export interface OpenCashPayload {
  montoInicial: number;
  observaciones?: string | null;
}

export interface CloseCashPayload {
  montoFinalContado: number;
  observaciones?: string | null;
}
