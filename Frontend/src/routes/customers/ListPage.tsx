import { useEffect, useState } from "react";
import { useNavigate } from "react-router-dom";
import { listCustomers, type CustomerDto } from "../../api/customers";

export default function CustomersListPage() {
  const [items, setItems] = useState<CustomerDto[]>([]);
  const [total, setTotal] = useState(0);
  const [page, setPage] = useState(1);
  const [search, setSearch] = useState("");
  const [loading, setLoading] = useState(true);
  const navigate = useNavigate();

  useEffect(() => {
    setLoading(true);
    listCustomers(page, 10, search).then(({ data, total }) => {
      setItems(data);
      setTotal(total);
      setLoading(false);
    });
  }, [page, search]);

  return (
    <div className="p-4 sm:p-6 max-w-7xl mx-auto min-h-screen bg-gray-50 dark:bg-gray-900 transition-colors">
      <div className="mb-6 flex flex-col sm:flex-row sm:items-center sm:justify-between gap-4">
        <div>
          <h1 className="text-2xl font-bold text-gray-900 dark:text-white">Clientes</h1>
          <p className="text-gray-600 dark:text-gray-400 mt-1">Gestiona todos tus clientes</p>
        </div>
        <button
          onClick={() => navigate("/customers/new")}
          className="btn-primary whitespace-nowrap"
        >
          + Nuevo Cliente
        </button>
      </div>
      <div className="mb-4">
        <input
          className="input w-full"
          placeholder="Buscar por nombre o número de identificación..."
          value={search}
          onChange={(e) => setSearch(e.target.value)}
        />
      </div>
      <div className="bg-white dark:bg-gray-800 rounded-lg shadow-sm border border-gray-200 dark:border-gray-700 overflow-hidden">
        {loading ? (
          <div className="p-8 text-center text-gray-500 dark:text-gray-400">Cargando...</div>
        ) : items.length === 0 ? (
          <div className="p-8 text-center text-gray-500 dark:text-gray-400">No hay clientes disponibles</div>
        ) : (
          <div className="overflow-x-auto">
            <table className="min-w-full divide-y divide-gray-200 dark:divide-gray-700">
              <thead className="bg-gray-50 dark:bg-gray-800">
                <tr>
                  <th className="px-4 sm:px-6 py-3 text-left text-xs font-medium text-gray-500 dark:text-gray-400 uppercase tracking-wider">Identificación</th>
                  <th className="px-4 sm:px-6 py-3 text-left text-xs font-medium text-gray-500 dark:text-gray-400 uppercase tracking-wider">Nombre</th>
                  <th className="px-4 sm:px-6 py-3 text-right text-xs font-medium text-gray-500 dark:text-gray-400 uppercase tracking-wider">Acciones</th>
                </tr>
              </thead>
              <tbody className="bg-white dark:bg-gray-800 divide-y divide-gray-200 dark:divide-gray-700">
                {items.map((c) => (
                  <tr key={c.idCustomer} className="hover:bg-gray-50 dark:hover:bg-gray-700 transition-colors">
                    <td className="px-4 sm:px-6 py-4 whitespace-nowrap text-sm font-medium text-gray-900 dark:text-white">{c.identificationNumber}</td>
                    <td className="px-4 sm:px-6 py-4 whitespace-nowrap text-sm text-gray-900 dark:text-white">{c.businessName || `${c.firstName ?? ""} ${c.lastName ?? ""}`.trim() || "N/A"}</td>
                    <td className="px-4 sm:px-6 py-4 whitespace-nowrap text-right">
                      <button
                        onClick={() => navigate(`/customers/${c.idCustomer}`)}
                        className="btn-primary text-sm px-3 py-1.5"
                        title="Ver detalles del cliente"
                      >
                        Ver
                      </button>
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
          Mostrando {items.length} de {total} clientes
        </div>
        <div className="flex items-center gap-2">
          <button
            className="btn-secondary disabled:opacity-50 disabled:cursor-not-allowed"
            disabled={page <= 1}
            onClick={() => setPage((p) => p - 1)}
          >
            Anterior
          </button>
          <span className="px-4 py-2 text-sm text-gray-700 dark:text-gray-300">Página {page}</span>
          <button
            className="btn-secondary disabled:opacity-50 disabled:cursor-not-allowed"
            disabled={items.length < 10 || page * 10 >= total}
            onClick={() => setPage((p) => p + 1)}
          >
            Siguiente
          </button>
        </div>
      </div>
    </div>
  );
}
