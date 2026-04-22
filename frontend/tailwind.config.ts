import type { Config } from "tailwindcss";

export default {
  content: ["./index.html", "./src/**/*.{ts,tsx}"],
  theme: {
    extend: {
      boxShadow: {
        luxury: "0 24px 80px rgba(15, 23, 42, 0.16)"
      }
    }
  },
  plugins: []
} satisfies Config;
