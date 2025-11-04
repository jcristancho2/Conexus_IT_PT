import http from "./http";

export type ProductDto = {
  idProduct: number;
  codeProduct?: string;
  productName: string;
  unitPrice: number;
  unitMeasure?: string;
  isActive: boolean;
};

export type CreateProductDto = {
  codeProduct?: string;
  productName: string;
  description?: string;
  unitPrice: number;
  unitMeasure?: string;
  isActive: boolean;
};

export async function listProducts(page = 1, pageSize = 10, search?: string) {
  const res = await http.get("/api/Products", { params: { page, pageSize, search } });
  const total = Number(res.headers["x-total-count"] ?? 0);
  return { data: res.data.data as ProductDto[], total };
}

export async function getProduct(id: number) {
  const res = await http.get(`/api/Products/${id}`);
  return res.data.data as ProductDto;
}

export async function createProduct(dto: CreateProductDto) {
  const res = await http.post("/api/Products", dto);
  return res.data.data as ProductDto;
}
