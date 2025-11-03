import { NavLink, useNavigate } from "react-router-dom";
import { useAuth } from "../hooks/useAuth";
import Logo from "./Logo";
import ThemeToggle from "./ThemeToggle";

export default function NavBar() {
  const { signOut, user } = useAuth();
  const navigate = useNavigate();

  function handleLogout() {
    signOut();
    navigate("/login");
  }

  const linkClass = ({ isActive }: { isActive: boolean }) =>
    `px-3 py-2 rounded-md transition-colors text-sm font-medium ${
      isActive
        ? "bg-blue-600 dark:bg-blue-500 text-white"
        : "text-gray-700 dark:text-gray-200 hover:bg-blue-50 dark:hover:bg-gray-800 hover:text-blue-700 dark:hover:text-blue-400"
    }`;

  return (
    <header className="sticky top-0 z-40 bg-white/80 dark:bg-gray-900/80 backdrop-blur supports-[backdrop-filter]:bg-white/60 dark:supports-[backdrop-filter]:bg-gray-900/60 border-b border-gray-200 dark:border-gray-800 shadow-sm">
      <div className="max-w-7xl mx-auto px-4 sm:px-6 lg:px-8 h-16 flex items-center justify-between">
        <div className="flex items-center gap-4">
          <button 
            onClick={() => navigate('/invoices')} 
            className="flex items-center gap-2 text-base font-semibold text-gray-900 dark:text-white hover:opacity-80 transition-opacity"
          >
            <Logo className="w-8 h-8" />
            <span className="hidden sm:inline">Conexus Billing</span>
          </button>
          <nav className="hidden md:flex items-center gap-1">
            <NavLink to="/invoices" className={linkClass}>Facturas</NavLink>
            <NavLink to="/customers" className={linkClass}>Clientes</NavLink>
            <NavLink to="/products" className={linkClass}>Productos</NavLink>
            <NavLink to="/dashboard" className={linkClass}>Dashboard</NavLink>
          </nav>
        </div>
        <div className="flex items-center gap-3">
          <ThemeToggle />
          {user && (
            <span className="text-gray-600 dark:text-gray-300 hidden sm:inline text-sm font-medium">
              {user.firstName} {user.lastName}
            </span>
          )}
          <button 
            className="px-3 py-2 border border-gray-300 dark:border-gray-700 rounded-md hover:bg-gray-50 dark:hover:bg-gray-800 text-sm font-medium text-gray-700 dark:text-gray-200 transition-colors" 
            onClick={handleLogout}
          >
            Salir
          </button>
        </div>
      </div>
    </header>
  );
}
