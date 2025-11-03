import http from "./http";

export type CustomerDto = {
  idCustomer: number;
  identificationNumber: string;
  firstName?: string;
  lastName?: string;
  businessName?: string;
};

export type CreateCustomerDto = {
  identificationNumber: string;
  firstName?: string;
  lastName?: string;
  businessName?: string;
  personType: number;
  idTypeIdentification: number;
  idCity: number;
  fullAddress: string;
  idTaxRegime: number;
  idTaxResponsibility: number;
  contacts?: Array<{ contactType: number; contactValue: string }>;
};

export async function listCustomers(page = 1, pageSize = 10, search?: string) {
  const res = await http.get("/api/Customers", { params: { page, pageSize, search } });
  const total = Number(res.headers["x-total-count"] ?? 0);
  return { data: res.data.data as CustomerDto[], total };
}

export async function getCustomer(id: number) {
  const res = await http.get(`/api/Customers/${id}`);
  return res.data.data as CustomerDto;
}

export async function createCustomer(dto: CreateCustomerDto) {
  const res = await http.post("/api/Customers", dto);
  return res.data.data as CustomerDto;
}
