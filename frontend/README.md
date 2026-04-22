# Joyitas Chirinos Frontend

Frontend premium en **React 18 + TypeScript + Tailwind CSS + React Query** para el backend de `JoyitasChirinos`.

## Incluye

- Login con JWT
- Dashboard ejecutivo
- Gestión de productos
- Gestión de clientes
- Registro y listado de ventas
- Gestión de encargos
- Layout responsive con look premium
- Integración real con la API existente

## Levantar

```bash
cp .env.example .env
npm install
npm run dev
```

## Variables de entorno

```env
VITE_API_URL=http://localhost:5000
```

## Integración con tu repo

Puedes copiar esta carpeta como `frontend/` dentro de tu repo principal, o mantenerla como proyecto separado.

## Nota importante

Tu backend expone CRUD y filtros para:

- `/api/auth`
- `/api/productos`
- `/api/clientes`
- `/api/ventas`
- `/api/encargos`

Para que el formulario de productos sea 100% amigable todavía conviene exponer endpoints para **categorías** y **proveedores**, porque hoy el backend recibe `CategoriaId` y `ProveedorId` pero en el repo visible no se ve un catálogo consumible desde el frontend. Mientras tanto, el formulario deja esos IDs como campos técnicos.


## Cambios recientes

- Se corrigió el warning de React Router activando los future flags `v7_startTransition` y `v7_relativeSplatPath`.
- Se corrigió el ajuste de stock para enviar `agregar`/`retirar`, que es lo que valida el backend.
- Se añadió un módulo inicial de Caja con apertura, cierre, estado actual e historial.
