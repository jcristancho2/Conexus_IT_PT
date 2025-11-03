"use client"

import { Card, CardContent, CardDescription, CardHeader, CardTitle } from "@/components/ui/card"
import { ChartContainer, ChartTooltip, ChartTooltipContent } from "@/components/ui/chart"
import { PieChart, Pie, Cell, ResponsiveContainer, Legend } from "recharts"
import { facturasConCliente } from "@/lib/mock-data"

export function FacturasPorEstadoChart() {
  const facturasPorEstado = [
    {
      estado: "Pagadas",
      cantidad: facturasConCliente.filter((f) => f.estado === "pagada").length,
      fill: "var(--color-chart-1)",
    },
    {
      estado: "Pendientes",
      cantidad: facturasConCliente.filter((f) => f.estado === "pendiente").length,
      fill: "var(--color-chart-2)",
    },
    {
      estado: "Canceladas",
      cantidad: facturasConCliente.filter((f) => f.estado === "cancelada").length,
      fill: "var(--color-chart-3)",
    },
  ]

  const chartConfig = {
    cantidad: {
      label: "Cantidad",
    },
    pagadas: {
      label: "Pagadas",
      color: "hsl(var(--chart-1))",
    },
    pendientes: {
      label: "Pendientes",
      color: "hsl(var(--chart-2))",
    },
    canceladas: {
      label: "Canceladas",
      color: "hsl(var(--chart-3))",
    },
  }

  return (
    <Card>
      <CardHeader>
        <CardTitle className="text-foreground">Distribución de Facturas</CardTitle>
        <CardDescription>Facturas por estado</CardDescription>
      </CardHeader>
      <CardContent>
        <ChartContainer config={chartConfig} className="h-[300px]">
          <ResponsiveContainer width="100%" height="100%">
            <PieChart>
              <ChartTooltip content={<ChartTooltipContent />} />
              <Pie
                data={facturasPorEstado}
                dataKey="cantidad"
                nameKey="estado"
                cx="50%"
                cy="50%"
                outerRadius={80}
                label={({ estado, cantidad }) => `${estado}: ${cantidad}`}
              >
                {facturasPorEstado.map((entry, index) => (
                  <Cell key={`cell-${index}`} fill={entry.fill} />
                ))}
              </Pie>
              <Legend />
            </PieChart>
          </ResponsiveContainer>
        </ChartContainer>
      </CardContent>
    </Card>
  )
}
