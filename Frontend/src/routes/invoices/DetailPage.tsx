import { useEffect, useState } from "react";
import { useParams, useNavigate } from "react-router-dom";
import { getInvoice, type InvoiceDto } from "../../api/invoices";

export default function InvoiceDetailPage() {
  const { id } = useParams();
  const navigate = useNavigate();
  const [invoice, setInvoice] = useState<InvoiceDto | null>(null);
  const [loading, setLoading] = useState(true);

  useEffect(() => {
    if (!id) return;
    setLoading(true);
    getInvoice(Number(id))
      .then(setInvoice)
      .finally(() => setLoading(false));
  }, [id]);

  if (loading) {
    return (
      <div className="p-4 sm:p-6 max-w-4xl mx-auto min-h-screen bg-gray-50 dark:bg-gray-900 transition-colors">
        <div className="flex items-center justify-center h-64">
          <div className="text-gray-500 dark:text-gray-400">Cargando factura...</div>
        </div>
      </div>
    );
  }

  if (!invoice) {
    return (
      <div className="p-4 sm:p-6 max-w-4xl mx-auto min-h-screen bg-gray-50 dark:bg-gray-900 transition-colors">
        <div className="text-center text-gray-500 dark:text-gray-400">Factura no encontrada</div>
      </div>
    );
  }

  return (
    <div className="p-4 sm:p-6 max-w-4xl mx-auto min-h-screen bg-gray-50 dark:bg-gray-900 transition-colors">
      <div className="mb-6 flex flex-col sm:flex-row sm:items-center sm:justify-between gap-4">
        <div>
          <h1 className="text-2xl font-bold text-gray-900 dark:text-white">Resumen de Factura</h1>
          <p className="text-gray-600 dark:text-gray-400 mt-1">Detalles de la factura #{invoice.invoiceNumber}</p>
        </div>
        <button
          onClick={() => navigate("/invoices")}
          className="btn-secondary"
        >
          ← Volver
        </button>
      </div>

      <div className="card mb-6">
        <div className="grid grid-cols-1 md:grid-cols-2 gap-6">
          <div>
            <h3 className="text-sm font-medium text-gray-500 dark:text-gray-400 mb-2">Información de la Factura</h3>
            <div className="space-y-2">
              <div>
                <span className="text-sm text-gray-600 dark:text-gray-400">Número:</span>
                <span className="ml-2 text-sm font-semibold text-gray-900 dark:text-white">{invoice.invoiceNumber}</span>
              </div>
              <div>
                <span className="text-sm text-gray-600 dark:text-gray-400">Fecha:</span>
                <span className="ml-2 text-sm text-gray-900 dark:text-gray-100">{new Date(invoice.invoiceDate).toLocaleDateString()}</span>
              </div>
              <div>
                <span className="text-sm text-gray-600 dark:text-gray-400">Vence:</span>
                <span className="ml-2 text-sm text-gray-900 dark:text-gray-100">{invoice.dueDate ? new Date(invoice.dueDate).toLocaleDateString() : "-"}</span>
              </div>
              <div>
                <span className="text-sm text-gray-600 dark:text-gray-400">Estado:</span>
                <span className={`ml-2 px-2 py-1 rounded text-xs font-medium ${invoice.status === 1 ? "bg-green-100 dark:bg-green-900 text-green-800 dark:text-green-200" : "bg-yellow-100 dark:bg-yellow-900 text-yellow-800 dark:text-yellow-200"
                  }`}>
                  {invoice.status === 1 ? "Finalizada" : "Borrador"}
                </span>
              </div>
            </div>
          </div>
          <div>
            <h3 className="text-sm font-medium text-gray-500 dark:text-gray-400 mb-2">Información del Cliente</h3>
            <div className="space-y-2">
              <div>
                <span className="text-sm text-gray-600 dark:text-gray-400">Cliente:</span>
                <span className="ml-2 text-sm font-semibold text-gray-900 dark:text-white">{invoice.customerName || "N/A"}</span>
              </div>
              <div>
                <span className="text-sm text-gray-600 dark:text-gray-400">NIT/CC:</span>
                <span className="ml-2 text-sm text-gray-900 dark:text-gray-100">{invoice.customerIdentification || "N/A"}</span>
              </div>
            </div>
          </div>
        </div>
      </div>

      <div className="card mb-6">
        <h2 className="text-lg font-semibold text-gray-900 dark:text-white mb-4">Detalles de la Factura</h2>
        <div className="overflow-x-auto">
          <table className="min-w-full divide-y divide-gray-200 dark:divide-gray-700">
            <thead className="bg-gray-50 dark:bg-gray-800">
              <tr>
                <th className="px-4 sm:px-6 py-3 text-left text-xs font-medium text-gray-500 dark:text-gray-400 uppercase tracking-wider">Producto</th>
                <th className="px-4 sm:px-6 py-3 text-left text-xs font-medium text-gray-500 dark:text-gray-400 uppercase tracking-wider">Cantidad</th>
                <th className="px-4 sm:px-6 py-3 text-left text-xs font-medium text-gray-500 dark:text-gray-400 uppercase tracking-wider">Precio Unit.</th>
                <th className="px-4 sm:px-6 py-3 text-left text-xs font-medium text-gray-500 dark:text-gray-400 uppercase tracking-wider">Descuento</th>
                <th className="px-4 sm:px-6 py-3 text-left text-xs font-medium text-gray-500 dark:text-gray-400 uppercase tracking-wider">Subtotal</th>
                <th className="px-4 sm:px-6 py-3 text-left text-xs font-medium text-gray-500 dark:text-gray-400 uppercase tracking-wider">Impuesto</th>
              </tr>
            </thead>
            <tbody className="bg-white dark:bg-gray-800 divide-y divide-gray-200 dark:divide-gray-700">
              {invoice.details.length === 0 ? (
                <tr>
                  <td colSpan={6} className="px-6 py-4 text-center text-gray-500 dark:text-gray-400">No hay detalles disponibles</td>
                </tr>
              ) : (
                invoice.details.map((d, idx) => {
                  const discount = (d.unitPrice * d.quantity) - d.subtotal;
                  const tax = d.subtotal * 0.19;
                  return (
                    <tr key={idx} className="hover:bg-gray-50 dark:hover:bg-gray-700 transition-colors">
                      <td className="px-4 sm:px-6 py-4 whitespace-nowrap text-sm font-medium text-gray-900 dark:text-gray-100">{d.productName || "N/A"}</td>
                      <td className="px-4 sm:px-6 py-4 whitespace-nowrap text-sm text-gray-900 dark:text-gray-100">{d.quantity}</td>
                      <td className="px-4 sm:px-6 py-4 whitespace-nowrap text-sm text-gray-900 dark:text-gray-100">${d.unitPrice.toFixed(2)}</td>
                      <td className="px-4 sm:px-6 py-4 whitespace-nowrap text-sm text-gray-900 dark:text-gray-100">${discount.toFixed(2)}</td>
                      <td className="px-4 sm:px-6 py-4 whitespace-nowrap text-sm font-semibold text-gray-900 dark:text-gray-100">${d.subtotal.toFixed(2)}</td>
                      <td className="px-4 sm:px-6 py-4 whitespace-nowrap text-sm text-gray-900 dark:text-gray-100">${tax.toFixed(2)}</td>
                    </tr>
                  );
                })
              )}
            </tbody>
          </table>
        </div>
      </div>

      <div className="card">
        <div className="flex justify-end">
          <div className="w-full max-w-md space-y-2">
            <div className="flex justify-between text-sm">
              <span className="text-gray-600 dark:text-gray-400">Subtotal:</span>
              <span className="font-medium text-gray-900 dark:text-white">${invoice.subtotal.toFixed(2)}</span>
            </div>
            <div className="flex justify-between text-sm">
              <span className="text-gray-600 dark:text-gray-400">Impuestos (19%):</span>
              <span className="font-medium text-gray-900 dark:text-white">${invoice.totalTax.toFixed(2)}</span>
            </div>
            <div className="border-t border-gray-200 dark:border-gray-700 pt-2 mt-2">
              <div className="flex justify-between">
                <span className="text-lg font-semibold text-gray-900 dark:text-white">Total:</span>
                <span className="text-lg font-bold text-blue-600 dark:text-blue-400">${invoice.total.toFixed(2)}</span>
              </div>
            </div>
          </div>
        </div>
        {invoice.notes && (
          <div className="mt-4 pt-4 border-t border-gray-200 dark:border-gray-700">
            <h3 className="text-sm font-medium text-gray-500 dark:text-gray-400 mb-2">Notas:</h3>
            <p className="text-sm text-gray-900 dark:text-gray-100">{invoice.notes}</p>
          </div>
        )}
      </div>
    </div>
  );
}
