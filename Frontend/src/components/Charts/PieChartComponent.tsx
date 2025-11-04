    import { PieChart, Pie, Cell, ResponsiveContainer, Legend, Tooltip } from 'recharts';

    interface PieChartData {
    name: string;
    value: number;
    }

    interface PieChartComponentProps {
    data: PieChartData[];
    title?: string;
    }

    const COLORS = ['#0088FE', '#00C49F', '#FFBB28', '#FF8042', '#8884D8'];

    export function PieChartComponent({ data, title }: PieChartComponentProps) {
    if (!data || data.length === 0) {
        return (
        <div className="flex items-center justify-center h-64 text-gray-500">
            No hay datos disponibles
        </div>
        );
    }

    return (
        <div className="w-full h-full">
        {title && (
            <h3 className="text-lg font-semibold mb-4 text-gray-700">{title}</h3>
        )}
        <ResponsiveContainer width="100%" height={300}>
            <PieChart>
            <Pie
                data={data}
                cx="50%"
                cy="50%"
                labelLine={false}
                label={({ name, percent }) => `${name}: ${(percent * 100).toFixed(0)}%`}
                outerRadius={80}
                fill="#8884d8"
                dataKey="value"
            >
                {data.map((_, index) => (
                <Cell key={`cell-${index}`} fill={COLORS[index % COLORS.length]} />
                ))}
            </Pie>
            <Tooltip />
            <Legend />
            </PieChart>
        </ResponsiveContainer>
        </div>
    );
    }