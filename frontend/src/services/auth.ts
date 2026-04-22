import { http } from "./http";
import type { LoginRequest, LoginResponse, MeResponse } from "../types/api";

export const authService = {
  async login(payload: LoginRequest) {
    const { data } = await http.post<LoginResponse>("/api/auth/login", payload);
    return data;
  },
  async me() {
    const { data } = await http.get<MeResponse>("/api/auth/me");
    return data;
  }
};
