"use client"

import { Card, CardContent, CardDescription, CardHeader, CardTitle } from "@/components/ui/card"
import { Badge } from "@/components/ui/badge"
import { facturasConCliente } from "@/lib/mock-data"
import type { Factura } from "@/lib/types"
import Link from "next/link"
import { Eye } from "lucide-react"
import { Button } from "@/components/ui/button"

export function FacturasRecientes() {
  const facturasRecientes = [...facturasConCliente]
    .sort((a, b) => new Date(b.fecha).getTime() - new Date(a.fecha).getTime())
    .slice(0, 5)

  const getEstadoBadge = (estado: Factura["estado"]) => {
    const variants = {
      pagada: "default",
      pendiente: "secondary",
      cancelada: "destructive",
    } as const

    const labels = {
      pagada: "Pagada",
      pendiente: "Pendiente",
      cancelada: "Cancelada",
    }

    return <Badge variant={variants[estado]}>{labels[estado]}</Badge>
  }

  return (
    <Card>
      <CardHeader>
        <CardTitle className="text-foreground">Facturas Recientes</CardTitle>
        <CardDescription>Últimas 5 facturas creadas</CardDescription>
      </CardHeader>
      <CardContent>
        <div className="space-y-4">
          {facturasRecientes.map((factura) => (
            <div
              key={factura.id}
              className="flex items-center justify-between border-b border-border pb-4 last:border-0"
            >
              <div className="flex-1">
                <div className="flex items-center gap-2 mb-1">
                  <p className="font-medium text-foreground">#{factura.id.toString().padStart(4, "0")}</p>
                  {getEstadoBadge(factura.estado)}
                </div>
                <p className="text-sm text-muted-foreground">{factura.cliente?.nombre}</p>
                <p className="text-xs text-muted-foreground">{new Date(factura.fecha).toLocaleDateString("es-ES")}</p>
              </div>
              <div className="flex items-center gap-4">
                <p className="font-semibold text-foreground">€{factura.total.toFixed(2)}</p>
                <Link href={`/facturas/${factura.id}`}>
                  <Button variant="ghost" size="icon">
                    <Eye className="h-4 w-4" />
                    <span className="sr-only">Ver factura</span>
                  </Button>
                </Link>
              </div>
            </div>
          ))}
        </div>
      </CardContent>
    </Card>
  )
}
