import { Gem, LayoutDashboard, LogOut, Package, ReceiptText, Users, Wrench, Menu, Wallet } from "lucide-react";
import { NavLink, Outlet, useNavigate } from "react-router-dom";
import { useState } from "react";
import { useAuthStore } from "../store/auth";
import { cn } from "../lib/cn";

const navItems = [
  { to: "/", label: "Dashboard", icon: LayoutDashboard },
  { to: "/caja", label: "Caja", icon: Wallet },
  { to: "/productos", label: "Productos", icon: Package },
  { to: "/clientes", label: "Clientes", icon: Users },
  { to: "/ventas", label: "Ventas", icon: ReceiptText },
  { to: "/encargos", label: "Encargos", icon: Wrench }
];

export function LayoutShell() {
  const navigate = useNavigate();
  const user = useAuthStore((state) => state.user);
  const logout = useAuthStore((state) => state.logout);
  const [mobileOpen, setMobileOpen] = useState(false);

  const handleLogout = () => {
    logout();
    navigate("/login");
  };

  return (
    <div className="min-h-screen bg-[radial-gradient(circle_at_top,_rgba(251,191,36,0.18),_transparent_25%),linear-gradient(180deg,_#fffaf1_0%,_#f8fafc_40%,_#ffffff_100%)]">
      <div className="mx-auto flex min-h-screen max-w-[1600px] gap-6 px-4 py-4 lg:px-6 lg:py-6">
        <aside
          className={cn(
            "fixed inset-y-4 left-4 z-40 w-[290px] rounded-[32px] border border-white/70 bg-slate-950 p-5 text-white shadow-2xl transition lg:static lg:block",
            mobileOpen ? "translate-x-0" : "-translate-x-[120%] lg:translate-x-0"
          )}
        >
          <div className="flex items-center gap-3">
            <div className="flex h-12 w-12 items-center justify-center rounded-2xl bg-gradient-to-br from-amber-300 to-yellow-500 text-slate-950">
              <Gem className="h-6 w-6" />
            </div>
            <div>
              <p className="text-sm text-slate-300">Sistema premium</p>
              <h2 className="text-lg font-semibold">Joyitas Chirinos</h2>
            </div>
          </div>

          <div className="mt-10 grid gap-2">
            {navItems.map((item) => {
              const Icon = item.icon;
              return (
                <NavLink
                  key={item.to}
                  to={item.to}
                  end={item.to === "/"}
                  onClick={() => setMobileOpen(false)}
                  className={({ isActive }) =>
                    cn(
                      "flex items-center gap-3 rounded-2xl px-4 py-3 text-sm font-medium transition",
                      isActive ? "bg-white text-slate-950" : "text-slate-300 hover:bg-white/10 hover:text-white"
                    )
                  }
                >
                  <Icon className="h-4 w-4" />
                  {item.label}
                </NavLink>
              );
            })}
          </div>

          <div className="mt-10 rounded-[28px] border border-white/10 bg-white/5 p-4">
            <p className="text-xs uppercase tracking-[0.18em] text-slate-400">Sesión activa</p>
            <p className="mt-3 text-sm font-semibold">{user?.nombre ?? "Usuario"}</p>
            <p className="mt-1 text-sm text-slate-400">{user?.email ?? "—"}</p>
            <p className="mt-3 inline-flex rounded-full bg-amber-400/20 px-3 py-1 text-xs font-semibold text-amber-300">
              {user?.rol ?? "Sin rol"}
            </p>
          </div>

          <button
            type="button"
            onClick={handleLogout}
            className="mt-6 inline-flex w-full items-center justify-center gap-2 rounded-2xl border border-white/10 px-4 py-3 text-sm font-medium text-slate-200 transition hover:bg-white/10"
          >
            <LogOut className="h-4 w-4" />
            Cerrar sesión
          </button>
        </aside>

        <div className="min-w-0 flex-1">
          <div className="mb-4 flex items-center justify-between rounded-[28px] border border-white/60 bg-white/80 px-4 py-3 shadow-sm backdrop-blur lg:hidden">
            <button
              type="button"
              onClick={() => setMobileOpen((value) => !value)}
              className="rounded-xl p-2 text-slate-700 hover:bg-slate-100"
            >
              <Menu className="h-5 w-5" />
            </button>
            <div className="text-right">
              <p className="text-sm font-semibold text-slate-900">Joyitas Chirinos</p>
              <p className="text-xs text-slate-500">{user?.rol}</p>
            </div>
          </div>

          <main className="grid gap-6">
            <Outlet />
          </main>
        </div>
      </div>
    </div>
  );
}
