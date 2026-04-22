import { create } from "zustand";
import { persist } from "zustand/middleware";
import type { LoginResponse } from "../types/api";

interface AuthState {
  token: string | null;
  user: Omit<LoginResponse, "token"> | null;
  setSession: (data: LoginResponse) => void;
  logout: () => void;
}

export const useAuthStore = create<AuthState>()(
  persist(
    (set) => ({
      token: null,
      user: null,
      setSession: (data) =>
        set({
          token: data.token,
          user: {
            nombre: data.nombre,
            email: data.email,
            rol: data.rol
          }
        }),
      logout: () =>
        set({
          token: null,
          user: null
        })
    }),
    {
      name: "joyitas-auth"
    }
  )
);
