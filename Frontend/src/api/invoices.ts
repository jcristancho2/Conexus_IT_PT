import http from "./http";

export type InvoiceDetailInput = {
  idProduct: number;
  quantity: number;
  unitPrice: number;
  discountAmount: number;
  taxAmount: number;
  totalAmount: number;
};

export type InvoiceDto = {
  idInvoice: number;
  invoiceNumber: string;
  invoiceDate: string;
  dueDate?: string;
  status: number;
  subtotal: number;
  totalTax: number;
  total: number;
  notes?: string;
  idCustomer: number;
  customerName?: string;
  customerIdentification?: string;
  issuerBusinessName?: string;
  details: Array<{
    idProduct: number;
    productName?: string;
    quantity: number;
    unitPrice: number;
    subtotal: number;
  }>;
};

export async function listInvoices(page = 1, pageSize = 10, search?: string) {
  const res = await http.get("/api/Invoices", { params: { page, pageSize, search } });
  const total = Number(res.headers["x-total-count"] ?? 0);
  return { data: res.data.data as InvoiceDto[], total };
}

export async function getInvoice(id: number) {
  const res = await http.get(`/api/Invoices/${id}`);
  return res.data.data as InvoiceDto;
}

export async function createInvoice(payload: {
  idCustomer: number;
  dueDate?: string;
  subtotalAmount: number;
  taxAmount: number;
  totalAmount: number;
  notes?: string;
  details: InvoiceDetailInput[];
}) {
  const res = await http.post("/api/Invoices", payload);
  return res.data.data as InvoiceDto;
}

export async function updateInvoice(id: number, payload: {
  idCustomer: number;
  dueDate?: string;
  subtotalAmount: number;
  taxAmount: number;
  totalAmount: number;
  status?: number;
  notes?: string;
  details: InvoiceDetailInput[];
}) {
  const res = await http.put(`/api/Invoices/${id}`, payload);
  return res.data.data as InvoiceDto;
}

export async function deleteInvoice(id: number) {
  await http.delete(`/api/Invoices/${id}`);
}
