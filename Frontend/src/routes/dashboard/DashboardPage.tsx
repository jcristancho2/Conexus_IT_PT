import { useEffect, useState } from "react";
import { getProductsRevenue, type ProductRevenueDto } from "../../api/dashboard";
import { PieChart, Pie, Cell, ResponsiveContainer, Legend, Tooltip } from "recharts";

const COLORS = ["#3b82f6", "#10b981", "#f59e0b", "#ef4444", "#8b5cf6", "#ec4899"];

export default function DashboardPage() {
  const [data, setData] = useState<ProductRevenueDto[]>([]);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState<string | null>(null);

  useEffect(() => {
    async function loadData() {
      setLoading(true);
      setError(null);
      try {
        const result = await getProductsRevenue();
        console.log("Datos recibidos del API:", result);
        setData(result || []);
      } catch (err: any) {
        console.error("Error al cargar datos:", err);
        setError(err?.response?.data?.message || "Error al cargar los datos del dashboard");
        setData([]);
      } finally {
        setLoading(false);
      }
    }
    loadData();
  }, []);

  const total = data.reduce((acc, d) => acc + (d.totalRevenue || 0), 0);

  const chartData = data
    .filter(d => d.totalRevenue > 0)
    .map((d) => {
      const percentage = total > 0 ? ((d.totalRevenue / total) * 100).toFixed(1) : "0";
      return {
        name: d.productName || "Sin nombre",
        value: Number(d.totalRevenue.toFixed(2)),
        percentage: percentage
      };
    });

  if (loading) {
    return (
      <div className="p-4 sm:p-6 max-w-7xl mx-auto min-h-screen bg-gray-50 dark:bg-gray-900 transition-colors">
        <div className="flex items-center justify-center h-64">
          <div className="text-gray-500 dark:text-gray-400">Cargando datos...</div>
        </div>
      </div>
    );
  }

  if (error) {
    return (
      <div className="p-4 sm:p-6 max-w-7xl mx-auto min-h-screen bg-gray-50 dark:bg-gray-900 transition-colors">
        <div className="bg-red-50 dark:bg-red-900/20 border border-red-200 dark:border-red-800 rounded-lg p-4">
          <p className="text-red-600 dark:text-red-400 font-semibold">Error</p>
          <p className="text-red-500 dark:text-red-300 text-sm mt-1">{error}</p>
        </div>
      </div>
    );
  }

  return (
    <div className="p-4 sm:p-6 max-w-7xl mx-auto min-h-screen bg-gray-50 dark:bg-gray-900 transition-colors">
      <div className="mb-6">
        <h1 className="text-2xl font-bold text-gray-900 dark:text-white">Dashboard</h1>
        <p className="text-gray-600 dark:text-gray-400 mt-1">Análisis de ventas por producto</p>
      </div>

      <div className="grid grid-cols-1 lg:grid-cols-2 gap-6">
        <div className="card">
          <h2 className="text-lg font-semibold text-gray-900 dark:text-white mb-4">Gráfica de Torta - Ventas por Producto</h2>
          {chartData.length > 0 && total > 0 ? (
            <div style={{ width: "100%", height: "400px" }}>
              <ResponsiveContainer width="100%" height="100%">
                <PieChart>
                  <Pie
                    data={chartData}
                    cx="50%"
                    cy="50%"
                    labelLine={false}
                    label={({ name, percentage }) => `${name}: ${percentage}%`}
                    outerRadius={120}
                    innerRadius={40}
                    fill="#8884d8"
                    dataKey="value"
                    nameKey="name"
                  >
                    {chartData.map((entry, index) => (
                      <Cell key={`cell-${index}`} fill={COLORS[index % COLORS.length]} />
                    ))}
                  </Pie>
                  <Tooltip
                    formatter={(value: number) => [`$${Number(value).toFixed(2)}`, "Ingresos"]}
                    contentStyle={{
                      backgroundColor: "var(--card)",
                      border: "1px solid var(--border)",
                      borderRadius: "8px",
                      padding: "8px"
                    }}
                  />
                  <Legend
                    verticalAlign="bottom"
                    height={36}
                    formatter={(value) => value}
                    iconType="circle"
                  />
                </PieChart>
              </ResponsiveContainer>
            </div>
          ) : (
            <div className="flex flex-col items-center justify-center h-64 text-gray-500 dark:text-gray-400">
              <p className="mb-2 font-medium">No hay datos disponibles</p>
              <p className="text-sm text-gray-400 dark:text-gray-500 text-center">
                {data.length === 0
                  ? "No hay productos con ventas registradas. Asegúrate de tener facturas en estado 'Final'."
                  : "Las facturas deben estar en estado 'Final' para aparecer en el gráfico"}
              </p>
            </div>
          )}
        </div>

        <div className="card">
          <h2 className="text-lg font-semibold text-gray-900 dark:text-white mb-4">Top Productos por Ingresos</h2>
          <div className="space-y-3">
            {chartData.length > 0 && total > 0 ? (
              <>
                <div className="mb-4 p-4 bg-blue-50 dark:bg-blue-900/20 rounded-lg border border-blue-200 dark:border-blue-800">
                  <div className="text-sm text-gray-600 dark:text-gray-400 mb-1">Total de Ingresos</div>
                  <div className="text-2xl font-bold text-blue-600 dark:text-blue-400">${total.toFixed(2)}</div>
                  <div className="text-xs text-gray-500 dark:text-gray-400 mt-1">{data.length} productos con ventas</div>
                </div>
                {chartData.map((item, index) => (
                  <div key={index} className="flex items-center justify-between p-3 border border-gray-200 dark:border-gray-700 rounded-lg hover:bg-gray-50 dark:hover:bg-gray-700 transition-colors">
                    <div className="flex items-center gap-3 flex-1">
                      <div
                        className="w-4 h-4 rounded-full flex-shrink-0"
                        style={{ backgroundColor: COLORS[index % COLORS.length] }}
                      />
                      <span className="font-medium text-gray-900 dark:text-white truncate">{item.name}</span>
                    </div>
                    <div className="text-right flex-shrink-0 ml-4">
                      <div className="font-semibold text-gray-900 dark:text-white">${item.value.toFixed(2)}</div>
                      <div className="text-sm text-gray-500 dark:text-gray-400">{item.percentage}%</div>
                    </div>
                  </div>
                ))}
              </>
            ) : (
              <div className="text-center text-gray-500 dark:text-gray-400 py-8">
                <p className="font-medium mb-2">No hay datos disponibles</p>
                <p className="text-sm text-gray-400 dark:text-gray-500">
                  {data.length === 0
                    ? "No hay productos con ventas registradas"
                    : "Las facturas deben estar en estado 'Final'"}
                </p>
              </div>
            )}
          </div>
        </div>
      </div>
    </div>
  );
}
