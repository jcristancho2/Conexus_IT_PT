"use client"

import { useState, useMemo } from "react"
import { facturasConCliente } from "@/lib/mock-data"
import type { Factura } from "@/lib/types"
import { Card, CardContent, CardHeader, CardTitle } from "@/components/ui/card"
import { Input } from "@/components/ui/input"
import { Button } from "@/components/ui/button"
import { Badge } from "@/components/ui/badge"
import { Table, TableBody, TableCell, TableHead, TableHeader, TableRow } from "@/components/ui/table"
import { Select, SelectContent, SelectItem, SelectTrigger, SelectValue } from "@/components/ui/select"
import { Search, Eye, Edit, Trash2 } from "lucide-react"
import Link from "next/link"

export function FacturasTable() {
  const [searchTerm, setSearchTerm] = useState("")
  const [estadoFilter, setEstadoFilter] = useState<string>("todos")

  const facturasFiltradas = useMemo(() => {
    return facturasConCliente.filter((factura) => {
      const matchesSearch =
        factura.id.toString().includes(searchTerm) ||
        factura.cliente?.nombre.toLowerCase().includes(searchTerm.toLowerCase())

      const matchesEstado = estadoFilter === "todos" || factura.estado === estadoFilter

      return matchesSearch && matchesEstado
    })
  }, [searchTerm, estadoFilter])

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
        <CardTitle className="text-foreground">Listado de Facturas</CardTitle>
        <div className="flex flex-col sm:flex-row gap-4 mt-4">
          <div className="relative flex-1">
            <Search className="absolute left-3 top-1/2 -translate-y-1/2 h-4 w-4 text-muted-foreground" />
            <Input
              placeholder="Buscar por número o cliente..."
              value={searchTerm}
              onChange={(e) => setSearchTerm(e.target.value)}
              className="pl-10"
            />
          </div>
          <Select value={estadoFilter} onValueChange={setEstadoFilter}>
            <SelectTrigger className="w-full sm:w-[180px]">
              <SelectValue placeholder="Estado" />
            </SelectTrigger>
            <SelectContent>
              <SelectItem value="todos">Todos los estados</SelectItem>
              <SelectItem value="pendiente">Pendiente</SelectItem>
              <SelectItem value="pagada">Pagada</SelectItem>
              <SelectItem value="cancelada">Cancelada</SelectItem>
            </SelectContent>
          </Select>
        </div>
      </CardHeader>
      <CardContent>
        <div className="rounded-md border border-border">
          <Table>
            <TableHeader>
              <TableRow>
                <TableHead className="text-foreground">N° Factura</TableHead>
                <TableHead className="text-foreground">Cliente</TableHead>
                <TableHead className="text-foreground">Fecha</TableHead>
                <TableHead className="text-foreground">Total</TableHead>
                <TableHead className="text-foreground">Estado</TableHead>
                <TableHead className="text-right text-foreground">Acciones</TableHead>
              </TableRow>
            </TableHeader>
            <TableBody>
              {facturasFiltradas.length === 0 ? (
                <TableRow>
                  <TableCell colSpan={6} className="text-center py-8 text-muted-foreground">
                    No se encontraron facturas
                  </TableCell>
                </TableRow>
              ) : (
                facturasFiltradas.map((factura) => (
                  <TableRow key={factura.id}>
                    <TableCell className="font-medium text-foreground">
                      #{factura.id.toString().padStart(4, "0")}
                    </TableCell>
                    <TableCell className="text-foreground">
                      {factura.cliente?.nombre || "Cliente desconocido"}
                    </TableCell>
                    <TableCell className="text-foreground">
                      {new Date(factura.fecha).toLocaleDateString("es-ES")}
                    </TableCell>
                    <TableCell className="text-foreground">€{factura.total.toFixed(2)}</TableCell>
                    <TableCell>{getEstadoBadge(factura.estado)}</TableCell>
                    <TableCell className="text-right">
                      <div className="flex justify-end gap-2">
                        <Link href={`/facturas/${factura.id}`}>
                          <Button variant="ghost" size="icon">
                            <Eye className="h-4 w-4" />
                            <span className="sr-only">Ver factura</span>
                          </Button>
                        </Link>
                        <Link href={`/facturas/${factura.id}/editar`}>
                          <Button variant="ghost" size="icon">
                            <Edit className="h-4 w-4" />
                            <span className="sr-only">Editar factura</span>
                          </Button>
                        </Link>
                        <Button variant="ghost" size="icon">
                          <Trash2 className="h-4 w-4" />
                          <span className="sr-only">Eliminar factura</span>
                        </Button>
                      </div>
                    </TableCell>
                  </TableRow>
                ))
              )}
            </TableBody>
          </Table>
        </div>

        <div className="flex items-center justify-between mt-4">
          <p className="text-sm text-muted-foreground">
            Mostrando {facturasFiltradas.length} de {facturasConCliente.length} facturas
          </p>
        </div>
      </CardContent>
    </Card>
  )
}
