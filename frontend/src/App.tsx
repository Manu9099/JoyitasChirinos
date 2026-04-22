import { Navigate, Route, Routes, useLocation } from "react-router-dom";
import { LayoutShell } from "./components/layout";
import { useAuthStore } from "./store/auth";
import LoginPage from "./pages/LoginPage";
import DashboardPage from "./pages/DashboardPage";
import ProductsPage from "./pages/ProductsPage";
import CajaPage from "./pages/CajaPage";
import ClientsPage from "./pages/ClientsPage";
import SalesPage from "./pages/SalesPage";
import OrdersPage from "./pages/OrdersPage";

function ProtectedRoute({ children }: { children: JSX.Element }) {
  const token = useAuthStore((state) => state.token);
  const location = useLocation();

  if (!token) {
    return <Navigate to="/login" replace state={{ from: location }} />;
  }

  return children;
}

export default function App() {
  return (
    <Routes>
      <Route path="/login" element={<LoginPage />} />

      <Route
        path="/"
        element={
          <ProtectedRoute>
            <LayoutShell />
          </ProtectedRoute>
        }
      >
        <Route index element={<DashboardPage />} />
        <Route path="caja" element={<CajaPage />} />
        <Route path="productos" element={<ProductsPage />} />
        <Route path="clientes" element={<ClientsPage />} />
        <Route path="ventas" element={<SalesPage />} />
        <Route path="encargos" element={<OrdersPage />} />
      </Route>

      <Route path="*" element={<Navigate to="/" replace />} />
    </Routes>
  );
}
