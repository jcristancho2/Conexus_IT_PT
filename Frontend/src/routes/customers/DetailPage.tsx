import { useEffect, useState } from "react";
import { useParams, useNavigate } from "react-router-dom";
import { getCustomer, type CustomerDto } from "../../api/customers";

export default function CustomerDetailPage() {
    const { id } = useParams();
    const navigate = useNavigate();
    const [customer, setCustomer] = useState<CustomerDto | null>(null);
    const [loading, setLoading] = useState(true);
    const [error, setError] = useState<string | null>(null);

    useEffect(() => {
        if (!id) return;
        setLoading(true);
        setError(null);
        getCustomer(Number(id))
            .then(setCustomer)
            .catch((err: any) => {
                setError(err?.response?.data?.message || "Error al cargar el cliente");
            })
            .finally(() => setLoading(false));
    }, [id]);

    if (loading) {
        return (
            <div className="p-4 sm:p-6 max-w-4xl mx-auto min-h-screen bg-gray-50 dark:bg-gray-900 transition-colors">
                <div className="flex items-center justify-center h-64">
                    <div className="text-gray-500 dark:text-gray-400">Cargando cliente...</div>
                </div>
            </div>
        );
    }

    if (error || !customer) {
        return (
            <div className="p-4 sm:p-6 max-w-4xl mx-auto min-h-screen bg-gray-50 dark:bg-gray-900 transition-colors">
                <div className="card">
                    <div className="text-center text-gray-500 dark:text-gray-400 mb-4">
                        {error || "Cliente no encontrado"}
                    </div>
                    <button onClick={() => navigate("/customers")} className="btn-secondary">
                        Volver a Clientes
                    </button>
                </div>
            </div>
        );
    }

    return (
        <div className="p-4 sm:p-6 max-w-4xl mx-auto min-h-screen bg-gray-50 dark:bg-gray-900 transition-colors">
            <div className="mb-6 flex flex-col sm:flex-row sm:items-center sm:justify-between gap-4">
                <div>
                    <h1 className="text-2xl font-bold text-gray-900 dark:text-white">Información del Cliente</h1>
                    <p className="text-gray-600 dark:text-gray-400 mt-1">Detalles completos del cliente</p>
                </div>
                <button
                    onClick={() => navigate("/customers")}
                    className="btn-secondary"
                >
                    ← Volver
                </button>
            </div>

            <div className="card mb-6">
                <h2 className="text-lg font-semibold text-gray-900 dark:text-white mb-4">Datos Personales</h2>
                <div className="grid grid-cols-1 md:grid-cols-2 gap-6">
                    <div>
                        <label className="block text-sm font-medium text-gray-500 dark:text-gray-400 mb-1">Número de Identificación</label>
                        <p className="text-sm font-semibold text-gray-900 dark:text-white">{customer.identificationNumber}</p>
                    </div>
                    <div>
                        <label className="block text-sm font-medium text-gray-500 dark:text-gray-400 mb-1">Nombre Completo</label>
                        <p className="text-sm text-gray-900 dark:text-white">
                            {customer.businessName || `${customer.firstName || ""} ${customer.lastName || ""}`.trim() || "N/A"}
                        </p>
                    </div>
                    {customer.firstName && (
                        <div>
                            <label className="block text-sm font-medium text-gray-500 dark:text-gray-400 mb-1">Nombre</label>
                            <p className="text-sm text-gray-900 dark:text-white">{customer.firstName}</p>
                        </div>
                    )}
                    {customer.lastName && (
                        <div>
                            <label className="block text-sm font-medium text-gray-500 dark:text-gray-400 mb-1">Apellido</label>
                            <p className="text-sm text-gray-900 dark:text-white">{customer.lastName}</p>
                        </div>
                    )}
                    {customer.businessName && (
                        <div>
                            <label className="block text-sm font-medium text-gray-500 dark:text-gray-400 mb-1">Razón Social</label>
                            <p className="text-sm text-gray-900 dark:text-white">{customer.businessName}</p>
                        </div>
                    )}
                </div>
            </div>

            <div className="card">
                <h2 className="text-lg font-semibold text-gray-900 dark:text-white mb-4">Información Adicional</h2>
                <div className="text-sm text-gray-600 dark:text-gray-400">
                    <p>ID del Cliente: {customer.idCustomer}</p>
                    <p className="mt-2">Para más información, contacta al administrador del sistema.</p>
                </div>
            </div>
        </div>
    );
}

