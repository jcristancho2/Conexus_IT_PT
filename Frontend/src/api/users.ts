import api from './client';

export interface User {
  id: string;
  email: string;
  firstName: string;
  lastName: string;
  role: 'Admin' | 'User' | 'Technician';
  isActive: boolean;
  createdAt: string;
}

export interface PaginatedResponse<T> {
  items: T[];
  totalCount: number;
  page: number;
  pageSize: number;
}

export async function getUsers(page = 1, pageSize = 10, search = ''): Promise<PaginatedResponse<User>> {
  const response = await api.get<PaginatedResponse<User>>('/users', {
    params: { page, pageSize, search }
  });
  return response.data;
}

export async function getUser(id: string): Promise<User> {
  const response = await api.get<User>(`/users/${id}`);
  return response.data;
}

export async function createUser(data: Omit<User, 'id' | 'createdAt'> & { password: string }): Promise<User> {
  const response = await api.post<User>('/users', data);
  return response.data;
}

export async function updateUser(id: string, data: Partial<User>): Promise<User> {
  const response = await api.put<User>(`/users/${id}`, data);
  return response.data;
}

export async function deleteUser(id: string): Promise<void> {
  await api.delete(`/users/${id}`);
}