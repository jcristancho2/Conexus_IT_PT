import { BrowserRouter, Routes, Route, Navigate, Outlet } from "react-router-dom";
import InvoicesListPage from "../routes/invoices/ListPage";
import InvoiceFormPage from "../routes/invoices/FormPage";
import InvoiceDetailPage from "../routes/invoices/DetailPage";
import CustomersListPage from "../routes/customers/ListPage";
import CustomerFormPage from "../routes/customers/FormPage";
import CustomerDetailPage from "../routes/customers/DetailPage";
import ProductsListPage from "../routes/products/ListPage";
import ProductFormPage from "../routes/products/FormPage";
import DashboardPage from "../routes/dashboard/DashboardPage";
import LoginPage from "../routes/auth/LoginPage";
import NavBar from "../components/NavBar";
import { ThemeProvider } from "../contexts/ThemeContext";

function RequireAuth({ children }: { children: JSX.Element }) {
  const token = localStorage.getItem("token");
  if (!token) return <Navigate to="/login" replace />;
  return children;
}

function ProtectedLayout() {
  return (
    <div className="min-h-screen flex flex-col bg-gray-50 dark:bg-gray-900 transition-colors">
      <NavBar />
      <main className="flex-1 pt-16">
        <Outlet />
      </main>
    </div>
  );
}

export default function AppRoutes() {
  return (
    <ThemeProvider>
      <BrowserRouter>
        <Routes>
          <Route path="/" element={<Navigate to="/invoices" replace />} />
          <Route path="/login" element={<LoginPage />} />

          <Route element={<RequireAuth><ProtectedLayout /></RequireAuth>}>
            <Route path="/invoices" element={<InvoicesListPage />} />
            <Route path="/invoices/new" element={<InvoiceFormPage />} />
            <Route path="/invoices/:id" element={<InvoiceDetailPage />} />
            <Route path="/invoices/:id/edit" element={<InvoiceFormPage />} />
            <Route path="/customers" element={<CustomersListPage />} />
            <Route path="/customers/new" element={<CustomerFormPage />} />
            <Route path="/customers/:id" element={<CustomerDetailPage />} />
            <Route path="/products" element={<ProductsListPage />} />
            <Route path="/products/new" element={<ProductFormPage />} />
            <Route path="/dashboard" element={<DashboardPage />} />
          </Route>

          <Route path="*" element={<Navigate to="/invoices" replace />} />
        </Routes>
      </BrowserRouter>
    </ThemeProvider>
  );
}
