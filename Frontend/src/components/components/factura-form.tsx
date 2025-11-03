"use client"

import type React from "react"

import { useState, useEffect } from "react"
import { useRouter } from "next/navigation"
import { Card, CardContent, CardHeader, CardTitle } from "@/components/ui/card"
import { Button } from "@/components/ui/button"
import { Input } from "@/components/ui/input"
import { Label } from "@/components/ui/label"
import { Select, SelectContent, SelectItem, SelectTrigger, SelectValue } from "@/components/ui/select"
import { Table, TableBody, TableCell, TableHead, TableHeader, TableRow } from "@/components/ui/table"
import { Plus, Trash2, Save, X } from "lucide-react"
import { clientes, productos } from "@/lib/mock-data"
import type { Factura } from "@/lib/types"

interface FacturaFormProps {
  factura?: Factura
  modo: "crear" | "editar"
}

interface LineaDetalle {
  id: string
  productoId: number
  cantidad: number
  precioUnitario: number
  subtotal: number
}

export function FacturaForm({ factura, modo }: FacturaFormProps) {
  const router = useRouter()
  const [clienteId, setClienteId] = useState<string>(factura?.clienteId.toString() || "")
  const [fecha, setFecha] = useState<string>(factura?.fecha || new Date().toISOString().split("T")[0])
  const [lineas, setLineas] = useState<LineaDetalle[]>([])

  useEffect(() => {
    if (factura?.detalles) {
      const lineasIniciales = factura.detalles.map((detalle) => ({
        id: Math.random().toString(),
        productoId: detalle.productoId,
        cantidad: detalle.cantidad,
        precioUnitario: detalle.precioUnitario,
        subtotal: detalle.subtotal,
      }))
      setLineas(lineasIniciales)
    }
  }, [factura])

  const agregarLinea = () => {
    const nuevaLinea: LineaDetalle = {
      id: Math.random().toString(),
      productoId: 0,
      cantidad: 1,
      precioUnitario: 0,
      subtotal: 0,
    }
    setLineas([...lineas, nuevaLinea])
  }

  const eliminarLinea = (id: string) => {
    setLineas(lineas.filter((linea) => linea.id !== id))
  }

  const actualizarLinea = (id: string, campo: keyof LineaDetalle, valor: number) => {
    setLineas(
      lineas.map((linea) => {
        if (linea.id !== id) return linea

        const lineaActualizada = { ...linea, [campo]: valor }

        if (campo === "productoId") {
          const producto = productos.find((p) => p.id === valor)
          if (producto) {
            lineaActualizada.precioUnitario = producto.precio
            lineaActualizada.subtotal = lineaActualizada.cantidad * producto.precio
          }
        } else if (campo === "cantidad") {
          lineaActualizada.subtotal = valor * lineaActualizada.precioUnitario
        }

        return lineaActualizada
      }),
    )
  }

  const calcularTotal = () => {
    return lineas.reduce((sum, linea) => sum + linea.subtotal, 0)
  }

  const handleSubmit = (e: React.FormEvent) => {
    e.preventDefault()

    if (!clienteId || lineas.length === 0) {
      alert("Por favor completa todos los campos requeridos")
      return
    }

    const facturaData = {
      clienteId: Number.parseInt(clienteId),
      fecha,
      total: calcularTotal(),
      estado: "pendiente" as const,
      detalles: lineas.map((linea) => ({
        productoId: linea.productoId,
        cantidad: linea.cantidad,
        precioUnitario: linea.precioUnitario,
        subtotal: linea.subtotal,
      })),
    }

    console.log("[v0] Datos de factura:", facturaData)
    // Aquí conectarías con tu API de ASP.NET Core
    // await fetch('/api/facturas', { method: 'POST', body: JSON.stringify(facturaData) })

    alert(`Factura ${modo === "crear" ? "creada" : "actualizada"} exitosamente`)
    router.push("/facturas")
  }

  return (
    <form onSubmit={handleSubmit}>
      <div className="grid gap-6">
        <Card>
          <CardHeader>
            <CardTitle className="text-foreground">Información General</CardTitle>
          </CardHeader>
          <CardContent className="grid gap-4 md:grid-cols-2">
            <div className="space-y-2">
              <Label htmlFor="cliente" className="text-foreground">
                Cliente *
              </Label>
              <Select value={clienteId} onValueChange={setClienteId} required>
                <SelectTrigger id="cliente">
                  <SelectValue placeholder="Seleccionar cliente" />
                </SelectTrigger>
                <SelectContent>
                  {clientes.map((cliente) => (
                    <SelectItem key={cliente.id} value={cliente.id.toString()}>
                      {cliente.nombre}
                    </SelectItem>
                  ))}
                </SelectContent>
              </Select>
            </div>

            <div className="space-y-2">
              <Label htmlFor="fecha" className="text-foreground">
                Fecha *
              </Label>
              <Input id="fecha" type="date" value={fecha} onChange={(e) => setFecha(e.target.value)} required />
            </div>
          </CardContent>
        </Card>

        <Card>
          <CardHeader className="flex flex-row items-center justify-between">
            <CardTitle className="text-foreground">Líneas de Detalle</CardTitle>
            <Button type="button" onClick={agregarLinea} size="sm">
              <Plus className="h-4 w-4 mr-2" />
              Agregar Línea
            </Button>
          </CardHeader>
          <CardContent>
            {lineas.length === 0 ? (
              <div className="text-center py-8 text-muted-foreground">
                <p>No hay líneas de detalle. Haz clic en "Agregar Línea" para comenzar.</p>
              </div>
            ) : (
              <div className="rounded-md border border-border overflow-x-auto">
                <Table>
                  <TableHeader>
                    <TableRow>
                      <TableHead className="text-foreground">Producto</TableHead>
                      <TableHead className="text-foreground">Cantidad</TableHead>
                      <TableHead className="text-foreground">Precio Unit.</TableHead>
                      <TableHead className="text-foreground">Subtotal</TableHead>
                      <TableHead className="w-[50px]"></TableHead>
                    </TableRow>
                  </TableHeader>
                  <TableBody>
                    {lineas.map((linea) => (
                      <TableRow key={linea.id}>
                        <TableCell>
                          <Select
                            value={linea.productoId.toString()}
                            onValueChange={(value) => actualizarLinea(linea.id, "productoId", Number.parseInt(value))}
                          >
                            <SelectTrigger className="w-full">
                              <SelectValue placeholder="Seleccionar" />
                            </SelectTrigger>
                            <SelectContent>
                              {productos.map((producto) => (
                                <SelectItem key={producto.id} value={producto.id.toString()}>
                                  {producto.nombre}
                                </SelectItem>
                              ))}
                            </SelectContent>
                          </Select>
                        </TableCell>
                        <TableCell>
                          <Input
                            type="number"
                            min="1"
                            value={linea.cantidad}
                            onChange={(e) =>
                              actualizarLinea(linea.id, "cantidad", Number.parseInt(e.target.value) || 1)
                            }
                            className="w-24"
                          />
                        </TableCell>
                        <TableCell className="text-foreground">€{linea.precioUnitario.toFixed(2)}</TableCell>
                        <TableCell className="font-medium text-foreground">€{linea.subtotal.toFixed(2)}</TableCell>
                        <TableCell>
                          <Button type="button" variant="ghost" size="icon" onClick={() => eliminarLinea(linea.id)}>
                            <Trash2 className="h-4 w-4" />
                            <span className="sr-only">Eliminar línea</span>
                          </Button>
                        </TableCell>
                      </TableRow>
                    ))}
                  </TableBody>
                </Table>
              </div>
            )}

            <div className="flex justify-end mt-4 pt-4 border-t border-border">
              <div className="text-right space-y-2">
                <div className="flex items-center gap-4">
                  <span className="text-lg font-semibold text-foreground">Total:</span>
                  <span className="text-2xl font-bold text-foreground">€{calcularTotal().toFixed(2)}</span>
                </div>
              </div>
            </div>
          </CardContent>
        </Card>

        <div className="flex justify-end gap-4">
          <Button type="button" variant="outline" onClick={() => router.push("/facturas")}>
            <X className="h-4 w-4 mr-2" />
            Cancelar
          </Button>
          <Button type="submit">
            <Save className="h-4 w-4 mr-2" />
            {modo === "crear" ? "Crear Factura" : "Guardar Cambios"}
          </Button>
        </div>
      </div>
    </form>
  )
}
