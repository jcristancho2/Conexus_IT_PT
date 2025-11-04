import { useState } from "react";
import { useNavigate } from "react-router-dom";
import { createProduct, type CreateProductDto } from "../../api/products";

export default function ProductFormPage() {
    const navigate = useNavigate();
    const [form, setForm] = useState<CreateProductDto>({
        codeProduct: "",
        productName: "",
        description: "",
        unitPrice: 0,
        unitMeasure: "UND",
        isActive: true,
    });
    const [loading, setLoading] = useState(false);
    const [error, setError] = useState<string | null>(null);

    async function onSubmit(e: React.FormEvent) {
        e.preventDefault();
        setError(null);
        setLoading(true);
        try {
            await createProduct(form);
            navigate("/products");
        } catch (err: any) {
            setError(err?.response?.data?.message || "Error al crear el producto");
        } finally {
            setLoading(false);
        }
    }

    return (
        <div className="p-4 sm:p-6 max-w-4xl mx-auto min-h-screen bg-gray-50 dark:bg-gray-900 transition-colors">
            <div className="mb-6">
                <h1 className="text-2xl font-bold text-gray-900 dark:text-white">Nuevo Producto</h1>
                <p className="text-gray-600 dark:text-gray-400 mt-1">Crea un nuevo producto en el sistema</p>
            </div>

            {error && (
                <div className="mb-4 p-3 bg-red-50 dark:bg-red-900/20 border border-red-200 dark:border-red-800 rounded-lg">
                    <p className="text-sm text-red-600 dark:text-red-400">{error}</p>
                </div>
            )}

            <form onSubmit={onSubmit} className="card space-y-4">
                <div>
                    <label className="block text-sm font-medium text-gray-700 dark:text-gray-300 mb-1">Código del Producto</label>
                    <input
                        className="input"
                        type="text"
                        value={form.codeProduct || ""}
                        onChange={(e) => setForm({ ...form, codeProduct: e.target.value })}
                    />
                </div>

                <div>
                    <label className="block text-sm font-medium text-gray-700 dark:text-gray-300 mb-1">Nombre del Producto</label>
                    <input
                        className="input"
                        type="text"
                        value={form.productName}
                        onChange={(e) => setForm({ ...form, productName: e.target.value })}
                        required
                    />
                </div>

                <div>
                    <label className="block text-sm font-medium text-gray-700 dark:text-gray-300 mb-1">Descripción</label>
                    <textarea
                        className="input"
                        rows={3}
                        value={form.description || ""}
                        onChange={(e) => setForm({ ...form, description: e.target.value })}
                    />
                </div>

                <div className="grid grid-cols-1 sm:grid-cols-2 gap-4">
                    <div>
                        <label className="block text-sm font-medium text-gray-700 dark:text-gray-300 mb-1">Precio Unitario</label>
                        <input
                            className="input"
                            type="number"
                            step="0.01"
                            min="0"
                            value={form.unitPrice}
                            onChange={(e) => setForm({ ...form, unitPrice: Number(e.target.value) })}
                            required
                        />
                    </div>

                    <div>
                        <label className="block text-sm font-medium text-gray-700 dark:text-gray-300 mb-1">Unidad de Medida</label>
                        <select
                            className="input"
                            value={form.unitMeasure || "UND"}
                            onChange={(e) => setForm({ ...form, unitMeasure: e.target.value })}
                        >
                            <option value="UND">Unidad</option>
                            <option value="HRS">Horas</option>
                            <option value="KGS">Kilogramos</option>
                            <option value="LTS">Litros</option>
                            <option value="MTS">Metros</option>
                        </select>
                    </div>
                </div>

                <div>
                    <label className="flex items-center gap-2">
                        <input
                            type="checkbox"
                            checked={form.isActive}
                            onChange={(e) => setForm({ ...form, isActive: e.target.checked })}
                            className="w-4 h-4 text-blue-600 border-gray-300 rounded focus:ring-blue-500 dark:bg-gray-700 dark:border-gray-600"
                        />
                        <span className="text-sm font-medium text-gray-700 dark:text-gray-300">Producto Activo</span>
                    </label>
                </div>

                <div className="flex gap-2 pt-4">
                    <button
                        type="submit"
                        disabled={loading}
                        className="btn-primary flex-1 disabled:opacity-50 disabled:cursor-not-allowed"
                    >
                        {loading ? "Creando..." : "Crear Producto"}
                    </button>
                    <button
                        type="button"
                        onClick={() => navigate("/products")}
                        className="btn-secondary"
                    >
                        Cancelar
                    </button>
                </div>
            </form>
        </div>
    );
}

