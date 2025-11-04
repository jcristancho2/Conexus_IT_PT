"use client"

import { Card, CardContent, CardDescription, CardHeader, CardTitle } from "@/components/ui/card"
import { ChartContainer, ChartTooltip, ChartTooltipContent } from "@/components/ui/chart"
import { BarChart, Bar, XAxis, YAxis, CartesianGrid, ResponsiveContainer } from "recharts"
import { facturasConCliente } from "@/lib/mock-data"

export function IngresosPorClienteChart() {
  // Calcular ingresos por cliente
  const ingresosPorCliente = facturasConCliente
    .filter((f) => f.estado === "pagada")
    .reduce(
      (acc, factura) => {
        const clienteNombre = factura.cliente?.nombre || "Desconocido"
        if (!acc[clienteNombre]) {
          acc[clienteNombre] = 0
        }
        acc[clienteNombre] += factura.total
        return acc
      },
      {} as Record<string, number>,
    )

  const chartData = Object.entries(ingresosPorCliente).map(([nombre, total]) => ({
    cliente: nombre.length > 20 ? nombre.substring(0, 20) + "..." : nombre,
    total,
  }))

  const chartConfig = {
    total: {
      label: "Total",
      color: "hsl(var(--chart-1))",
    },
  }

  return (
    <Card>
      <CardHeader>
        <CardTitle className="text-foreground">Ingresos por Cliente</CardTitle>
        <CardDescription>Total de ingresos de facturas pagadas</CardDescription>
      </CardHeader>
      <CardContent>
        <ChartContainer config={chartConfig} className="h-[300px]">
          <ResponsiveContainer width="100%" height="100%">
            <BarChart data={chartData}>
              <CartesianGrid strokeDasharray="3 3" />
              <XAxis dataKey="cliente" tick={{ fill: "hsl(var(--foreground))" }} />
              <YAxis tick={{ fill: "hsl(var(--foreground))" }} />
              <ChartTooltip content={<ChartTooltipContent />} />
              <Bar dataKey="total" fill="var(--color-chart-1)" radius={[8, 8, 0, 0]} />
            </BarChart>
          </ResponsiveContainer>
        </ChartContainer>
      </CardContent>
    </Card>
  )
}
