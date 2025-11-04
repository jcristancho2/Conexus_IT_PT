import http from "./http";

export type LoginResponse = {
  token: string;
  expiration: string;
  user: {
    idUser: number;
    email: string;
    firstName: string;
    lastName: string;
    role: string;
    isActive: boolean;
    createdAt: string;
  };
};

export async function login(email: string, password: string) {
  const res = await http.post("/api/Auth/login", { email, password });
  return res.data.data as LoginResponse;
}

export async function register(payload: {
  email: string;
  password: string;
  firstName: string;
  lastName: string;
  role: string;
}) {
  const res = await http.post("/api/Auth/register", payload);
  return res.data.data as LoginResponse;
}

