import axios from "axios";

const API_URL = import.meta.env.VITE_API_URL ?? "http://localhost:5000";

export const http = axios.create({
  baseURL: API_URL,
  headers: {
    "Content-Type": "application/json"
  }
});

http.interceptors.request.use((config) => {
  const raw = localStorage.getItem("joyitas-auth");
  if (raw) {
    try {
      const parsed = JSON.parse(raw) as { state?: { token?: string | null } };
      const token = parsed.state?.token;
      if (token) {
        config.headers.Authorization = `Bearer ${token}`;
      }
    } catch {
      // ignore malformed storage
    }
  }
  return config;
});

http.interceptors.response.use(
  (response) => response,
  (error) => {
    const message =
      error?.response?.data?.message ??
      error?.response?.data?.mensaje ??
      error?.response?.data?.title ??
      "Ocurrió un error al procesar la solicitud.";
    return Promise.reject(new Error(message));
  }
);
