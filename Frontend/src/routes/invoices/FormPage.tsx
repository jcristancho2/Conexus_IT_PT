import { useEffect, useMemo, useState } from "react";
import { useNavigate, useParams } from "react-router-dom";
import { createInvoice, getInvoice, updateInvoice, type InvoiceDetailInput } from "../../api/invoices";
import { listCustomers } from "../../api/customers";
import { listProducts, type ProductDto } from "../../api/products";

type FormState = {
  idCustomer: number | "";
  dueDate?: string;
  notes?: string;
  details: InvoiceDetailInput[];
};

export default function InvoiceFormPage() {
  const { id } = useParams();
  const isEdit = Boolean(id);
  const navigate = useNavigate();

  const [customers, setCustomers] = useState<{ idCustomer: number; label: string }[]>([]);
  const [products, setProducts] = useState<ProductDto[]>([]);
  const [form, setForm] = useState<FormState>({ idCustomer: "", details: [] });

  useEffect(() => {
    listCustomers(1, 50).then(({ data }) =>
      setCustomers(
        data.map((c) => ({
          idCustomer: c.idCustomer,
          label: c.businessName || `${c.firstName ?? ""} ${c.lastName ?? ""}`.trim() || c.identificationNumber,
        }))
      )
    );
    listProducts(1, 100).then(({ data }) => setProducts(data));
  }, []);

  useEffect(() => {
    if (!isEdit || !id) return;
    getInvoice(Number(id)).then((inv) => {
      setForm({
        idCustomer: inv.idCustomer,
        dueDate: inv.dueDate ? inv.dueDate.substring(0, 10) : undefined,
        notes: inv.notes,
        details: inv.details.map((d) => ({
          idProduct: d.idProduct,
          quantity: d.quantity,
          unitPrice: d.unitPrice,
          discountAmount: 0,
          taxAmount: 0,
          totalAmount: d.subtotal,
        })),
      });
    });
  }, [isEdit, id]);

  const subtotal = useMemo(() => form.details.reduce((acc, d) => acc + d.totalAmount, 0), [form.details]);
  const taxAmount = useMemo(() => Number((subtotal * 0.19).toFixed(2)), [subtotal]);
  const total = useMemo(() => Number((subtotal + taxAmount).toFixed(2)), [subtotal, taxAmount]);

  function addDetail(prod: ProductDto) {
    setForm((f) => ({
      ...f,
      details: [...f.details, { idProduct: prod.idProduct, quantity: 1, unitPrice: prod.unitPrice, discountAmount: 0, taxAmount: 0, totalAmount: prod.unitPrice }],
    }));
  }

  function updateDetail(idx: number, patch: Partial<InvoiceDetailInput>) {
    setForm((f) => {
      const next = [...f.details];
      next[idx] = { ...next[idx], ...patch } as InvoiceDetailInput;
      // recalcular totalAmount simple: qty * unitPrice - discount
      const d = next[idx];
      d.totalAmount = Number((d.quantity * d.unitPrice - d.discountAmount).toFixed(2));
      return { ...f, details: next };
    });
  }

  function removeDetail(idx: number) {
    setForm((f) => ({ ...f, details: f.details.filter((_, i) => i !== idx) }));
  }

  async function onSubmit() {
    if (!form.idCustomer || form.details.length === 0) return;
    const payload = {
      idCustomer: Number(form.idCustomer),
      dueDate: form.dueDate ? `${form.dueDate}T00:00:00Z` : undefined,
      subtotalAmount: subtotal,
      taxAmount,
      totalAmount: total,
      notes: form.notes,
      details: form.details,
    };
    if (isEdit && id) {
      await updateInvoice(Number(id), payload);
    } else {
      const created = await createInvoice(payload);
      navigate(`/invoices/${created.idInvoice}`);
      return;
    }
    navigate(`/invoices/${id}`);
  }

  return (
    <div className="p-4 max-w-4xl mx-auto">
      <h1 className="text-xl font-semibold mb-4">{isEdit ? "Editar factura" : "Nueva factura"}</h1>
      <div className="grid grid-cols-1 md:grid-cols-2 gap-3 mb-4">
        <div>
          <label className="block text-sm mb-1">Cliente</label>
          <select className="border px-3 py-2 w-full" value={form.idCustomer} onChange={(e) => setForm((f) => ({ ...f, idCustomer: e.target.value ? Number(e.target.value) : "" }))}>
            <option value="">Seleccione...</option>
            {customers.map((c) => (
              <option key={c.idCustomer} value={c.idCustomer}>{c.label}</option>
            ))}
          </select>
        </div>
        <div>
          <label className="block text-sm mb-1">Vencimiento</label>
          <input type="date" className="border px-3 py-2 w-full" value={form.dueDate ?? ""} onChange={(e) => setForm((f) => ({ ...f, dueDate: e.target.value || undefined }))} />
        </div>
      </div>

      <div className="mb-3">
        <label className="block text-sm mb-1">Agregar producto</label>
        <div className="flex gap-2">
          <select className="border px-3 py-2" onChange={(e) => {
            const p = products.find(pr => pr.idProduct === Number(e.target.value));
            if (p) addDetail(p);
            e.currentTarget.selectedIndex = 0;
          }}>
            <option value="">Seleccione...</option>
            {products.map((p) => (
              <option key={p.idProduct} value={p.idProduct}>{p.productName} (${p.unitPrice.toFixed(2)})</option>
            ))}
          </select>
        </div>
      </div>

      <div className="overflow-x-auto mb-4">
        <table className="min-w-full text-sm">
          <thead>
            <tr className="text-left border-b">
              <th className="py-2">Producto</th>
              <th className="py-2">Cant.</th>
              <th className="py-2">Precio</th>
              <th className="py-2">Desc.</th>
              <th className="py-2">Subtotal</th>
              <th className="py-2"></th>
            </tr>
          </thead>
          <tbody>
            {form.details.map((d, idx) => {
              const p = products.find(x => x.idProduct === d.idProduct);
              return (
                <tr key={idx} className="border-b">
                  <td className="py-2">{p?.productName}</td>
                  <td className="py-2"><input type="number" min={1} className="border px-2 py-1 w-24" value={d.quantity} onChange={(e) => updateDetail(idx, { quantity: Number(e.target.value) })} /></td>
                  <td className="py-2"><input type="number" step="0.01" className="border px-2 py-1 w-28" value={d.unitPrice} onChange={(e) => updateDetail(idx, { unitPrice: Number(e.target.value) })} /></td>
                  <td className="py-2"><input type="number" step="0.01" className="border px-2 py-1 w-28" value={d.discountAmount} onChange={(e) => updateDetail(idx, { discountAmount: Number(e.target.value) })} /></td>
                  <td className="py-2">${d.totalAmount.toFixed(2)}</td>
                  <td className="py-2"><button className="px-2 py-1 border rounded" onClick={() => removeDetail(idx)}>Quitar</button></td>
                </tr>
              );
            })}
          </tbody>
        </table>
      </div>

      <div className="mb-4">
        <label className="block text-sm mb-1">Notas</label>
        <textarea className="border px-3 py-2 w-full" rows={3} value={form.notes ?? ""} onChange={(e) => setForm((f) => ({ ...f, notes: e.target.value }))} />
      </div>

      <div className="flex justify-end gap-6 text-sm mb-4">
        <div><b>Subtotal:</b> ${subtotal.toFixed(2)}</div>
        <div><b>Impuestos (19%):</b> ${taxAmount.toFixed(2)}</div>
        <div><b>Total:</b> ${total.toFixed(2)}</div>
      </div>

      <div className="flex gap-2">
        <button className="px-3 py-2 bg-blue-600 text-white rounded" onClick={onSubmit}>{isEdit ? "Guardar" : "Crear"}</button>
        <button className="px-3 py-2 border rounded" onClick={() => navigate("/invoices")}>Cancelar</button>
      </div>
    </div>
  );
}

