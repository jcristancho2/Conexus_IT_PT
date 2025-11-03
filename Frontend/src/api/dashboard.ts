import http from "./http";

export type ProductRevenueDto = {
  productId: number;
  productCode?: string;
  productName?: string;
  totalQuantity: number;
  totalRevenue: number;
  invoiceCount: number;
};

export async function getStats(params?: { startDate?: string; endDate?: string }) {
  const res = await http.get("/api/Dashboard/stats", { params });
  return res.data.data;
}

export async function getProductsRevenue(params?: { startDate?: string; endDate?: string }): Promise<ProductRevenueDto[]> {
  const res = await http.get("/api/Dashboard/products-revenue", { params });
  const data = res.data.data || [];
  // Normalizar datos para manejar tanto PascalCase como camelCase
  return data.map((item: any) => ({
    productId: item.productId || item.ProductId || 0,
    productCode: item.productCode || item.ProductCode,
    productName: item.productName || item.ProductName || "Sin nombre",
    totalQuantity: item.totalQuantity || item.TotalQuantity || 0,
    totalRevenue: item.totalRevenue || item.TotalRevenue || 0,
    invoiceCount: item.invoiceCount || item.InvoiceCount || 0
  }));
}
