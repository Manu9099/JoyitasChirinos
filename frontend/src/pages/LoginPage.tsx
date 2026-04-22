import { useForm } from "react-hook-form";
import { useNavigate } from "react-router-dom";
import { z } from "zod";
import { zodResolver } from "@hookform/resolvers/zod";
import { Gem, LockKeyhole, Mail } from "lucide-react";
import { toast } from "sonner";
import { authService } from "../services/auth";
import { useAuthStore } from "../store/auth";
import { Button, Card, Field, Input, Spinner } from "../components/ui";

const schema = z.object({
  email: z.string().email("Ingresa un correo válido"),
  password: z.string().min(4, "Ingresa tu contraseña")
});

type FormValues = z.infer<typeof schema>;

export default function LoginPage() {
  const navigate = useNavigate();
  const setSession = useAuthStore((state) => state.setSession);

  const {
    register,
    handleSubmit,
    formState: { errors, isSubmitting }
  } = useForm<FormValues>({
    resolver: zodResolver(schema),
    defaultValues: {
      email: "",
      password: ""
    }
  });

  const onSubmit = async (values: FormValues) => {
    try {
      const data = await authService.login(values);
      setSession(data);
      toast.success(`Bienvenida, ${data.nombre}`);
      navigate("/");
    } catch (error) {
      toast.error(error instanceof Error ? error.message : "No se pudo iniciar sesión");
    }
  };

  return (
    <div className="min-h-screen bg-[radial-gradient(circle_at_top,_rgba(251,191,36,0.3),_transparent_24%),linear-gradient(180deg,_#0f172a_0%,_#111827_40%,_#020617_100%)] px-4 py-10">
      <div className="mx-auto grid min-h-[calc(100vh-5rem)] max-w-6xl items-center gap-8 lg:grid-cols-[1.1fr_0.9fr]">
        <div className="hidden lg:block">
          <div className="max-w-xl">
            <div className="inline-flex items-center gap-3 rounded-full border border-white/10 bg-white/5 px-4 py-2 text-sm text-amber-200 backdrop-blur">
              <Gem className="h-4 w-4" />
              Sistema de gestión premium
            </div>
            <h1 className="mt-6 text-5xl font-semibold leading-tight text-white">
              Una experiencia elegante para administrar tu joyería.
            </h1>
            <p className="mt-6 text-lg leading-8 text-slate-300">
              Controla inventario, ventas, clientes y encargos desde una sola interfaz con una estética fina, clara y lista para producción.
            </p>

            <div className="mt-10 grid grid-cols-2 gap-4">
              {[
                "Inventario visual y filtros potentes",
                "Registro rápido de ventas",
                "Seguimiento de clientes frecuentes",
                "Encargos con estados claros"
              ].map((item) => (
                <div key={item} className="rounded-[28px] border border-white/10 bg-white/5 p-5 text-sm text-slate-200 backdrop-blur">
                  {item}
                </div>
              ))}
            </div>
          </div>
        </div>

        <Card className="mx-auto w-full max-w-xl rounded-[36px] bg-white/95 p-8">
          <div className="mb-8">
            <div className="mb-4 flex h-14 w-14 items-center justify-center rounded-[20px] bg-slate-950 text-amber-300">
              <Gem className="h-7 w-7" />
            </div>
            <h2 className="text-3xl font-semibold tracking-tight text-slate-950">Iniciar sesión</h2>
            <p className="mt-2 text-sm text-slate-500">
              Accede al panel administrativo de Joyitas Chirinos.
            </p>
          </div>

          <form onSubmit={handleSubmit(onSubmit)} className="grid gap-5">
            <Field label="Correo" required>
              <div className="relative">
                <Mail className="pointer-events-none absolute left-4 top-1/2 h-4 w-4 -translate-y-1/2 text-slate-400" />
                <Input className="pl-11" placeholder="admin@joyitas.com" {...register("email")} />
              </div>
              {errors.email ? <p className="text-xs text-rose-600">{errors.email.message}</p> : null}
            </Field>

            <Field label="Contraseña" required>
              <div className="relative">
                <LockKeyhole className="pointer-events-none absolute left-4 top-1/2 h-4 w-4 -translate-y-1/2 text-slate-400" />
                <Input type="password" className="pl-11" placeholder="••••••••" {...register("password")} />
              </div>
              {errors.password ? <p className="text-xs text-rose-600">{errors.password.message}</p> : null}
            </Field>

            <Button type="submit" className="mt-2 h-12" disabled={isSubmitting}>
              {isSubmitting ? (
                <span className="inline-flex items-center gap-2">
                  <Spinner />
                  Ingresando...
                </span>
              ) : (
                "Entrar al sistema"
              )}
            </Button>
          </form>
        </Card>
      </div>
    </div>
  );
}
