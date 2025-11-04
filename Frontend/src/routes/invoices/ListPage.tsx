import { useEffect, useState, useMemo, useCallback } from "react";
import { useNavigate } from "react-router-dom";
import { listInvoices, deleteInvoice, updateInvoice, type InvoiceDto } from "../../api/invoices";

export default function InvoicesListPage() {
  const [items, setItems] = useState<InvoiceDto[]>([]);
  const [allItems, setAllItems] = useState<InvoiceDto[]>([]);
  const [total, setTotal] = useState(0);
  const [page, setPage] = useState(1);
  const [search, setSearch] = useState("");
  const [dateFrom, setDateFrom] = useState("");
  const [totalMin, setTotalMin] = useState("");
  const [loading, setLoading] = useState(true);
  const navigate = useNavigate();

  const load = useCallback(async () => {
    setLoading(true);
    try {
      const { data, total } = await listInvoices(1, 1000, search || undefined);
      setAllItems(data);
      setTotal(total);
    } finally {
      setLoading(false);
    }
  }, [search]);

  useEffect(() => {
    load();
  }, [load]);

  const filteredItems = useMemo(() => {
    let filtered = [...allItems];

    if (dateFrom) {
      const from = new Date(dateFrom);
      from.setHours(0, 0, 0, 0);
      filtered = filtered.filter(i => {
        const invoiceDate = new Date(i.invoiceDate);
        invoiceDate.setHours(0, 0, 0, 0);
        return invoiceDate >= from;
      });
    }

    if (totalMin) {
      const min = Number(totalMin);
      if (!isNaN(min)) {
        filtered = filtered.filter(i => i.total >= min);
      }
    }

    return filtered;
  }, [allItems, dateFrom, totalMin]);

  const paginatedItems = useMemo(() => {
    const start = (page - 1) * 10;
    const end = start + 10;
    return filteredItems.slice(start, end);
  }, [filteredItems, page]);

  useEffect(() => {
    setItems(paginatedItems);
  }, [paginatedItems]);

  useEffect(() => {
    setPage(1);
  }, [dateFrom, totalMin, search]);

  async function handleDelete(id: number) {
    if (!confirm("¿Está seguro de que desea eliminar esta factura?")) return;
    await deleteInvoice(id);
    load();
  }

  async function handleChangeStatus(id: number, newStatus: number) {
    const invoice = items.find(i => i.idInvoice === id);
    if (!invoice) return;

    try {
      await updateInvoice(id, {
        idCustomer: invoice.idCustomer,
        dueDate: invoice.dueDate,
        subtotalAmount: invoice.subtotal,
        taxAmount: invoice.totalTax,
        totalAmount: invoice.total,
        status: newStatus,
        notes: invoice.notes,
        details: invoice.details.map(d => ({
          idProduct: d.idProduct,
          quantity: d.quantity,
          unitPrice: d.unitPrice,
          discountAmount: 0,
          taxAmount: 0,
          totalAmount: d.subtotal
        }))
      });
      load();
    } catch (error) {
      console.error("Error al cambiar estado:", error);
      alert("Error al cambiar el estado de la factura");
    }
  }

  function getStatusBadge(status: number) {
    switch (status) {
      case 0:
        return <span className="px-2 py-1 rounded-full text-xs font-medium bg-yellow-100 dark:bg-yellow-900 text-yellow-800 dark:text-yellow-200">Borrador</span>;
      case 1:
        return <span className="px-2 py-1 rounded-full text-xs font-medium bg-green-100 dark:bg-green-900 text-green-800 dark:text-green-200">Finalizada</span>;
      case 2:
        return <span className="px-2 py-1 rounded-full text-xs font-medium bg-red-100 dark:bg-red-900 text-red-800 dark:text-red-200">Cancelada</span>;
      default:
        return <span className="px-2 py-1 rounded-full text-xs font-medium bg-gray-100 dark:bg-gray-800 text-gray-800 dark:text-gray-200">Desconocido</span>;
    }
  }

  function clearFilters() {
    setSearch("");
    setDateFrom("");
    setTotalMin("");
  }

  return (
    <div className="p-4 sm:p-6 max-w-7xl mx-auto min-h-screen bg-gray-50 dark:bg-gray-900 transition-colors">
      <div className="flex flex-col sm:flex-row sm:items-center sm:justify-between mb-6 gap-4">
        <div>
          <h1 className="text-2xl font-bold text-gray-900 dark:text-white">Facturas</h1>
          <p className="text-gray-600 dark:text-gray-400 mt-1">Gestiona todas tus facturas</p>
        </div>
        <button
          className="btn-primary whitespace-nowrap"
          onClick={() => navigate("/invoices/new")}
        >
          + Nueva factura
        </button>
      </div>

      <div className="bg-white dark:bg-gray-800 rounded-lg shadow-sm border border-gray-200 dark:border-gray-700 p-4 mb-4">
        <div className="grid grid-cols-1 sm:grid-cols-2 lg:grid-cols-3 gap-4">
          <div>
            <label className="block text-sm font-medium text-gray-700 dark:text-gray-300 mb-1">Buscar</label>
            <input
              className="input"
              placeholder="Número de factura, cliente..."
              value={search}
              onChange={(e) => setSearch(e.target.value)}
            />
          </div>
          <div>
            <label className="block text-sm font-medium text-gray-700 dark:text-gray-300 mb-1">Fecha Desde</label>
            <input
              className="input"
              type="date"
              value={dateFrom}
              onChange={(e) => setDateFrom(e.target.value)}
            />
          </div>
          <div>
            <label className="block text-sm font-medium text-gray-700 dark:text-gray-300 mb-1">Total Mínimo</label>
            <input
              className="input"
              type="number"
              step="0.01"
              min="0"
              placeholder="0"
              value={totalMin}
              onChange={(e) => setTotalMin(e.target.value)}
            />
          </div>
          <div className="flex items-end">
            <button
              onClick={clearFilters}
              className="btn-secondary w-full"
            >
              Limpiar Filtros
            </button>
          </div>
        </div>
      </div>

      <div className="bg-white dark:bg-gray-800 rounded-lg shadow-sm border border-gray-200 dark:border-gray-700 overflow-hidden">
        {loading ? (
          <div className="p-8 text-center text-gray-500 dark:text-gray-400">Cargando...</div>
        ) : filteredItems.length === 0 ? (
          <div className="p-8 text-center text-gray-500 dark:text-gray-400">No hay facturas disponibles</div>
        ) : (
          <div className="overflow-x-auto">
            <table className="min-w-full divide-y divide-gray-200 dark:divide-gray-700">
              <thead className="bg-gray-50 dark:bg-gray-800">
                <tr>
                  <th className="px-4 sm:px-6 py-3 text-left text-xs font-medium text-gray-500 dark:text-gray-400 uppercase tracking-wider">Número</th>
                  <th className="px-4 sm:px-6 py-3 text-left text-xs font-medium text-gray-500 dark:text-gray-400 uppercase tracking-wider">Cliente</th>
                  <th className="px-4 sm:px-6 py-3 text-left text-xs font-medium text-gray-500 dark:text-gray-400 uppercase tracking-wider">Fecha</th>
                  <th className="px-4 sm:px-6 py-3 text-left text-xs font-medium text-gray-500 dark:text-gray-400 uppercase tracking-wider">Estado</th>
                  <th className="px-4 sm:px-6 py-3 text-left text-xs font-medium text-gray-500 dark:text-gray-400 uppercase tracking-wider">Total</th>
                  <th className="px-4 sm:px-6 py-3 text-left text-xs font-medium text-gray-500 dark:text-gray-400 uppercase tracking-wider">Acciones</th>
                </tr>
              </thead>
              <tbody className="bg-white dark:bg-gray-800 divide-y divide-gray-200 dark:divide-gray-700">
                {items.map((i) => (
                  <tr key={i.idInvoice} className="hover:bg-gray-50 dark:hover:bg-gray-700 transition-colors">
                    <td className="px-4 sm:px-6 py-4 whitespace-nowrap">
                      <div className="text-sm font-medium text-gray-900 dark:text-white">{i.invoiceNumber}</div>
                    </td>
                    <td className="px-4 sm:px-6 py-4 whitespace-nowrap">
                      <div className="text-sm text-gray-900 dark:text-white">{i.customerName || "N/A"}</div>
                      {i.customerIdentification && (
                        <div className="text-xs text-gray-500 dark:text-gray-400">{i.customerIdentification}</div>
                      )}
                    </td>
                    <td className="px-4 sm:px-6 py-4 whitespace-nowrap">
                      <div className="text-sm text-gray-900 dark:text-white">{new Date(i.invoiceDate).toLocaleDateString()}</div>
                    </td>
                    <td className="px-4 sm:px-6 py-4 whitespace-nowrap">
                      <div className="flex items-center gap-2">
                        {getStatusBadge(i.status)}
                        {i.status === 0 && (
                          <button
                            onClick={() => handleChangeStatus(i.idInvoice, 1)}
                            className="px-2 py-1 text-xs bg-green-600 dark:bg-green-500 text-white rounded hover:bg-green-700 dark:hover:bg-green-600 transition-colors"
                            title="Cambiar a Finalizada"
                          >
                            Finalizar
                          </button>
                        )}
                      </div>
                    </td>
                    <td className="px-4 sm:px-6 py-4 whitespace-nowrap">
                      <div className="text-sm font-semibold text-gray-900 dark:text-white">${i.total.toFixed(2)}</div>
                    </td>
                    <td className="px-4 sm:px-6 py-4 whitespace-nowrap text-sm font-medium">
                      <div className="flex gap-2">
                        <button
                          className="px-3 py-1 text-blue-600 dark:text-blue-400 hover:text-blue-800 dark:hover:text-blue-300 hover:bg-blue-50 dark:hover:bg-blue-900/20 rounded transition-colors"
                          onClick={() => navigate(`/invoices/${i.idInvoice}`)}
                        >
                          Ver
                        </button>
                        <button
                          className="px-3 py-1 text-green-600 dark:text-green-400 hover:text-green-800 dark:hover:text-green-300 hover:bg-green-50 dark:hover:bg-green-900/20 rounded transition-colors"
                          onClick={() => navigate(`/invoices/${i.idInvoice}/edit`)}
                        >
                          Editar
                        </button>
                        <button
                          className="px-3 py-1 text-red-600 dark:text-red-400 hover:text-red-800 dark:hover:text-red-300 hover:bg-red-50 dark:hover:bg-red-900/20 rounded transition-colors"
                          onClick={() => handleDelete(i.idInvoice)}
                        >
                          Eliminar
                        </button>
                      </div>
                    </td>
                  </tr>
                ))}
              </tbody>
            </table>
          </div>
        )}
      </div>

      <div className="mt-4 flex flex-col sm:flex-row items-center justify-between gap-4">
        <div className="text-sm text-gray-600 dark:text-gray-400">
          Mostrando {items.length} de {filteredItems.length} facturas (Total: {total})
        </div>
        <div className="flex items-center gap-2">
          <button
            className="btn-secondary disabled:opacity-50 disabled:cursor-not-allowed"
            disabled={page <= 1}
            onClick={() => setPage((p) => p - 1)}
          >
            Anterior
          </button>
          <span className="px-4 py-2 text-sm text-gray-700 dark:text-gray-300">Página {page} de {Math.ceil(filteredItems.length / 10)}</span>
          <button
            className="btn-secondary disabled:opacity-50 disabled:cursor-not-allowed"
            disabled={page >= Math.ceil(filteredItems.length / 10)}
            onClick={() => setPage((p) => p + 1)}
          >
            Siguiente
          </button>
        </div>
      </div>
    </div>
  );
}
