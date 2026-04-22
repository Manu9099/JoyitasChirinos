export type Rol = "Admin" | "Vendedor";

export const PRODUCT_TYPES = [
  "Cadena",
  "Anillo",
  "Arete",
  "Medalla",
  "Pulsera",
  "Reloj",
  "Otro"
] as const;

export const PRODUCT_MATERIALS = ["Oro18k", "Plata", "Acero", "Otro"] as const;

export const PRODUCT_STATES = ["Activo", "Agotado", "Descontinuado"] as const;

export const ORDER_STATES = [
  "Pendiente",
  "EnProduccion",
  "Listo",
  "Entregado",
  "Cancelado"
] as const;

export const PAYMENT_METHODS = ["efectivo", "yape", "plin", "tarjeta", "transferencia"] as const;
