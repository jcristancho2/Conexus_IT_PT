import { useState } from "react";
import { useNavigate } from "react-router-dom";
import { createCustomer, type CreateCustomerDto } from "../../api/customers";

export default function CustomerFormPage() {
    const navigate = useNavigate();
    const [form, setForm] = useState<CreateCustomerDto>({
        identificationNumber: "",
        firstName: "",
        lastName: "",
        businessName: "",
        personType: 0,
        idTypeIdentification: 1,
        idAddress: 1,
        idCity: 1,
        fullAddress: "",
        idTaxRegime: 1,
        idTaxResponsibility: 1,
        contacts: [],
    });
    const [loading, setLoading] = useState(false);
    const [error, setError] = useState<string | null>(null);

    async function onSubmit(e: React.FormEvent) {
        e.preventDefault();
        setError(null);
        setLoading(true);
        try {
            await createCustomer(form);
            navigate("/customers");
        } catch (err: any) {
            setError(err?.response?.data?.message || "Error al crear el cliente");
        } finally {
            setLoading(false);
        }
    }

    return (
        <div className="p-4 sm:p-6 max-w-4xl mx-auto min-h-screen bg-gray-50 dark:bg-gray-900 transition-colors">
            <div className="mb-6">
                <h1 className="text-2xl font-bold text-gray-900 dark:text-white">Nuevo Cliente</h1>
                <p className="text-gray-600 dark:text-gray-400 mt-1">Crea un nuevo cliente en el sistema</p>
            </div>

            {error && (
                <div className="mb-4 p-3 bg-red-50 dark:bg-red-900/20 border border-red-200 dark:border-red-800 rounded-lg">
                    <p className="text-sm text-red-600 dark:text-red-400">{error}</p>
                </div>
            )}

            <form onSubmit={onSubmit} className="bg-white dark:bg-gray-800 rounded-lg shadow-sm border border-gray-200 dark:border-gray-700 p-6 space-y-4">
                <div>
                    <label className="block text-sm font-medium text-gray-700 dark:text-gray-300 mb-1">Tipo de Persona</label>
                    <select
                        className="input"
                        value={form.personType}
                        onChange={(e) => setForm({ ...form, personType: Number(e.target.value) })}
                    >
                        <option value={0}>Persona Natural</option>
                        <option value={1}>Empresa</option>
                    </select>
                </div>

                <div>
                    <label className="block text-sm font-medium text-gray-700 dark:text-gray-300 mb-1">Número de Identificación</label>
                    <input
                        className="input"
                        type="text"
                        value={form.identificationNumber}
                        onChange={(e) => setForm({ ...form, identificationNumber: e.target.value })}
                        required
                    />
                </div>

                {form.personType === 0 ? (
                    <>
                        <div>
                            <label className="block text-sm font-medium text-gray-700 dark:text-gray-300 mb-1">Nombre</label>
                            <input
                                className="input"
                                type="text"
                                value={form.firstName || ""}
                                onChange={(e) => setForm({ ...form, firstName: e.target.value })}
                                required
                            />
                        </div>
                        <div>
                            <label className="block text-sm font-medium text-gray-700 dark:text-gray-300 mb-1">Apellido</label>
                            <input
                                className="input"
                                type="text"
                                value={form.lastName || ""}
                                onChange={(e) => setForm({ ...form, lastName: e.target.value })}
                                required
                            />
                        </div>
                    </>
                ) : (
                    <div>
                        <label className="block text-sm font-medium text-gray-700 dark:text-gray-300 mb-1">Razón Social</label>
                        <input
                            className="input"
                            type="text"
                            value={form.businessName || ""}
                            onChange={(e) => setForm({ ...form, businessName: e.target.value })}
                            required
                        />
                    </div>
                )}

                <div>
                    <label className="block text-sm font-medium text-gray-700 dark:text-gray-300 mb-1">Ciudad</label>
                    <select
                        className="input"
                        value={form.idCity}
                        onChange={(e) => setForm({ ...form, idCity: Number(e.target.value) })}
                    >
                        <option value={1}>Medellín</option>
                        <option value={2}>Bogotá</option>
                        <option value={3}>Cali</option>
                    </select>
                </div>

                <div>
                    <label className="block text-sm font-medium text-gray-700 dark:text-gray-300 mb-1">Dirección</label>
                    <input
                        className="input"
                        type="text"
                        value={form.fullAddress || ""}
                        onChange={(e) => setForm({ ...form, fullAddress: e.target.value })}
                        required
                    />
                </div>

                <div className="flex gap-2 pt-4">
                    <button
                        type="submit"
                        disabled={loading}
                        className="btn-primary flex-1 disabled:opacity-50 disabled:cursor-not-allowed"
                    >
                        {loading ? "Creando..." : "Crear Cliente"}
                    </button>
                    <button
                        type="button"
                        onClick={() => navigate("/customers")}
                        className="btn-secondary"
                    >
                        Cancelar
                    </button>
                </div>
            </form>
        </div>
    );
}
