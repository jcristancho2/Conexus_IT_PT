import { useEffect, useState } from "react";
import { login, type LoginResponse } from "../api/auth";

export function useAuth() {
  const [user, setUser] = useState<LoginResponse["user"] | null>(null);
  const [token, setToken] = useState<string | null>(() => localStorage.getItem("token"));

  useEffect(() => {
    const raw = localStorage.getItem("user");
    if (raw) setUser(JSON.parse(raw));
  }, []);

  function setSession(data: LoginResponse) {
    localStorage.setItem("token", data.token);
    localStorage.setItem("user", JSON.stringify(data.user));
    setToken(data.token);
    setUser(data.user);
  }

  async function signIn(email: string, password: string) {
    const data = await login(email, password);
    setSession(data);
  }

  function signOut() {
    localStorage.removeItem("token");
    localStorage.removeItem("user");
    setToken(null);
    setUser(null);
  }

  return { user, token, signIn, signOut };
}

