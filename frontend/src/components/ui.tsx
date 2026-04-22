import { Fragment, forwardRef, type ButtonHTMLAttributes, type InputHTMLAttributes, type PropsWithChildren, type SelectHTMLAttributes, type TextareaHTMLAttributes, type ReactNode } from "react";
import { X, Search } from "lucide-react";
import { cn } from "../lib/cn";

export function Button({
  className,
  variant = "primary",
  ...props
}: ButtonHTMLAttributes<HTMLButtonElement> & {
  variant?: "primary" | "secondary" | "ghost" | "danger";
}) {
  const variants = {
    primary:
      "bg-slate-950 text-white hover:bg-slate-800 shadow-luxury",
    secondary:
      "bg-white text-slate-900 ring-1 ring-slate-200 hover:bg-slate-50",
    ghost:
      "bg-transparent text-slate-700 hover:bg-slate-100",
    danger:
      "bg-rose-600 text-white hover:bg-rose-700"
  };

  return (
    <button
      className={cn(
        "inline-flex items-center justify-center rounded-2xl px-4 py-2.5 text-sm font-semibold transition disabled:cursor-not-allowed disabled:opacity-50",
        variants[variant],
        className
      )}
      {...props}
    />
  );
}

export const Input = forwardRef<HTMLInputElement, InputHTMLAttributes<HTMLInputElement>>(
  ({ className, ...props }, ref) => {
    return (
      <input
        ref={ref}
        className={cn(
          "w-full rounded-2xl border border-slate-200 bg-white px-4 py-3 text-sm text-slate-900 outline-none transition placeholder:text-slate-400 focus:border-amber-300 focus:ring-4 focus:ring-amber-100",
          className
        )}
        {...props}
      />
    );
  }
);

Input.displayName = "Input";

export const Select = forwardRef<HTMLSelectElement, SelectHTMLAttributes<HTMLSelectElement>>(
  ({ className, ...props }, ref) => {
    return (
      <select
        ref={ref}
        className={cn(
          "w-full rounded-2xl border border-slate-200 bg-white px-4 py-3 text-sm text-slate-900 outline-none transition focus:border-amber-300 focus:ring-4 focus:ring-amber-100",
          className
        )}
        {...props}
      />
    );
  }
);

Select.displayName = "Select";

export const Textarea = forwardRef<HTMLTextAreaElement, TextareaHTMLAttributes<HTMLTextAreaElement>>(
  ({ className, ...props }, ref) => {
    return (
      <textarea
        ref={ref}
        className={cn(
          "min-h-[120px] w-full rounded-2xl border border-slate-200 bg-white px-4 py-3 text-sm text-slate-900 outline-none transition placeholder:text-slate-400 focus:border-amber-300 focus:ring-4 focus:ring-amber-100",
          className
        )}
        {...props}
      />
    );
  }
);

Textarea.displayName = "Textarea";

export function Card({ className, children }: PropsWithChildren<{ className?: string }>) {
  return (
    <div
      className={cn(
        "rounded-[28px] border border-white/70 bg-white/95 p-6 shadow-luxury backdrop-blur",
        className
      )}
    >
      {children}
    </div>
  );
}

export function SectionHeading({
  title,
  subtitle,
  actions
}: {
  title: string;
  subtitle?: string;
  actions?: ReactNode;
}) {
  return (
    <div className="mb-6 flex flex-col gap-4 lg:flex-row lg:items-end lg:justify-between">
      <div>
        <h1 className="text-2xl font-semibold tracking-tight text-slate-950">{title}</h1>
        {subtitle ? <p className="mt-2 text-sm text-slate-500">{subtitle}</p> : null}
      </div>
      {actions ? <div className="flex flex-wrap gap-3">{actions}</div> : null}
    </div>
  );
}

export function Badge({
  children,
  tone = "slate"
}: PropsWithChildren<{
  tone?: "slate" | "amber" | "emerald" | "rose" | "violet";
}>) {
  const styles = {
    slate: "bg-slate-100 text-slate-700",
    amber: "bg-amber-100 text-amber-700",
    emerald: "bg-emerald-100 text-emerald-700",
    rose: "bg-rose-100 text-rose-700",
    violet: "bg-violet-100 text-violet-700"
  };

  return (
    <span className={cn("inline-flex rounded-full px-3 py-1 text-xs font-medium", styles[tone])}>
      {children}
    </span>
  );
}

export function StatCard({
  label,
  value,
  helper
}: {
  label: string;
  value: string;
  helper?: string;
}) {
  return (
    <Card className="p-5">
      <p className="text-xs uppercase tracking-[0.22em] text-slate-400">{label}</p>
      <p className="mt-4 text-3xl font-semibold text-slate-950">{value}</p>
      {helper ? <p className="mt-2 text-sm text-slate-500">{helper}</p> : null}
    </Card>
  );
}

export function EmptyState({
  title,
  description
}: {
  title: string;
  description: string;
}) {
  return (
    <Card className="flex min-h-[240px] flex-col items-center justify-center text-center">
      <div className="mx-auto mb-4 flex h-14 w-14 items-center justify-center rounded-full bg-slate-100 text-slate-500">
        <Search className="h-6 w-6" />
      </div>
      <h3 className="text-lg font-semibold text-slate-900">{title}</h3>
      <p className="mt-2 max-w-md text-sm text-slate-500">{description}</p>
    </Card>
  );
}

export function Field({
  label,
  required,
  children,
  hint
}: PropsWithChildren<{
  label: string;
  required?: boolean;
  hint?: string;
}>) {
  return (
    <label className="grid gap-2">
      <span className="text-sm font-medium text-slate-700">
        {label} {required ? <span className="text-rose-500">*</span> : null}
      </span>
      {children}
      {hint ? <span className="text-xs text-slate-400">{hint}</span> : null}
    </label>
  );
}

export function Modal({
  open,
  title,
  description,
  onClose,
  children,
  className
}: PropsWithChildren<{
  open: boolean;
  title: string;
  description?: string;
  onClose: () => void;
  className?: string;
}>) {
  if (!open) return null;

  return (
    <div className="fixed inset-0 z-50 flex items-center justify-center bg-slate-950/50 p-4 backdrop-blur-sm">
      <div className={cn("max-h-[90vh] w-full max-w-3xl overflow-auto rounded-[32px] bg-white p-6 shadow-2xl", className)}>
        <div className="mb-6 flex items-start justify-between gap-4">
          <div>
            <h3 className="text-xl font-semibold text-slate-950">{title}</h3>
            {description ? <p className="mt-2 text-sm text-slate-500">{description}</p> : null}
          </div>
          <button
            type="button"
            onClick={onClose}
            className="rounded-full p-2 text-slate-400 transition hover:bg-slate-100 hover:text-slate-700"
          >
            <X className="h-5 w-5" />
          </button>
        </div>
        {children}
      </div>
    </div>
  );
}

export function Table({
  headers,
  children
}: PropsWithChildren<{ headers: string[] }>) {
  return (
    <div className="overflow-hidden rounded-[28px] border border-slate-200 bg-white">
      <div className="overflow-auto">
        <table className="min-w-full divide-y divide-slate-200">
          <thead className="bg-slate-50">
            <tr>
              {headers.map((header) => (
                <th
                  key={header}
                  className="px-4 py-3 text-left text-xs font-semibold uppercase tracking-[0.18em] text-slate-400"
                >
                  {header}
                </th>
              ))}
            </tr>
          </thead>
          <tbody className="divide-y divide-slate-100 bg-white">{children}</tbody>
        </table>
      </div>
    </div>
  );
}

export function TableRow({
  children
}: PropsWithChildren) {
  return <tr className="hover:bg-slate-50/80">{children}</tr>;
}

export function Cell({
  className,
  children
}: PropsWithChildren<{ className?: string }>) {
  return <td className={cn("whitespace-nowrap px-4 py-4 text-sm text-slate-700", className)}>{children}</td>;
}

export function Spinner() {
  return (
    <div className="inline-flex h-5 w-5 animate-spin rounded-full border-2 border-slate-200 border-t-slate-700" />
  );
}

export function LoadingBlock() {
  return (
    <Card className="flex min-h-[220px] items-center justify-center">
      <Spinner />
    </Card>
  );
}

export function Divider() {
  return <div className="h-px w-full bg-slate-200" />;
}

export function Stack({
  children,
  className
}: PropsWithChildren<{ className?: string }>) {
  return <div className={cn("grid gap-4", className)}>{children}</div>;
}

export const ReactFragment = Fragment;
