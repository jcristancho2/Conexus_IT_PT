import api from './client';

export interface Ticket {
  id: string;
  title: string;
  description: string;
  status: 'Open' | 'InProgress' | 'Resolved' | 'Closed';
  priority: 'Low' | 'Medium' | 'High' | 'Critical';
  customerId: string;
  customerName?: string;
  assignedTo?: string;
  assignedToName?: string;
  createdAt: string;
  updatedAt: string;
  resolvedAt?: string;
}

export interface PaginatedResponse<T> {
  items: T[];
  totalCount: number;
  page: number;
  pageSize: number;
}

export async function getTickets(page = 1, pageSize = 10, search = ''): Promise<PaginatedResponse<Ticket>> {
  const response = await api.get<PaginatedResponse<Ticket>>('/tickets', {
    params: { page, pageSize, search }
  });
  return response.data;
}

export async function getTicket(id: string): Promise<Ticket> {
  const response = await api.get<Ticket>(`/tickets/${id}`);
  return response.data;
}

export async function createTicket(data: Omit<Ticket, 'id' | 'createdAt' | 'updatedAt'>): Promise<Ticket> {
  const response = await api.post<Ticket>('/tickets', data);
  return response.data;
}

export async function updateTicket(id: string, data: Partial<Ticket>): Promise<Ticket> {
  const response = await api.put<Ticket>(`/tickets/${id}`, data);
  return response.data;
}

export async function deleteTicket(id: string): Promise<void> {
  await api.delete(`/tickets/${id}`);
}

export async function updateTicketStatus(id: string, status: Ticket['status']): Promise<Ticket> {
  const response = await api.patch<Ticket>(`/tickets/${id}/status`, { status });
  return response.data;
}