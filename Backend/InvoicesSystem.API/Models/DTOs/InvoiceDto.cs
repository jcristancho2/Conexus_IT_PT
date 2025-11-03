using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using InvoicesSystem.API.Models.Enums;
using System.ComponentModel.DataAnnotations;

namespace InvoicesSystem.API.Models.DTOs;
public class InvoiceDto
{
    public int IdInvoice { get; set; } // id de la factura
    public string? InvoiceNumber { get; set; } // numero de la factura
    public DateTime InvoiceDate { get; set; } // fecha de la factura
    public DateTime? DueDate { get; set; } // fecha de vencimiento
    public InvoiceStatus Status { get; set; } // estado de la factura
    public decimal Subtotal { get; set; } // subtotal
    public decimal TotalTax { get; set; } // total impuestos
    public decimal Total { get; set; } // total
    public string? Notes { get; set; } // notas adicionales

    // datos del cliente
    public int IdCustomer { get; set; } // id del cliente
    public string? CustomerName { get; set; } // nombre del cliente
    public string? CustomerIdentification { get; set; } // identificacion del cliente

    // datos del emisor
    public int IdIssuer { get; set; } // id del emisor
    public string? IssuerBusinessName { get; set; } // nombre del emisor o empresa emisora


    // detalles de la factura
    public List<InvoiceDetailDto> Details { get; set; } = new(); // detalles de la factura

    // pagos de la factura
    public List<InvoicePaymentDto> Payments { get; set; } = new(); // pagos de la factura
    public DateTime CreatedAt { get; set; } 
    public DateTime? UpdatedAt { get; set; }

}
public class InvoiceDetailDto
{
    public int IdProduct { get; set; } // id del producto
    public string? ProductName { get; set; } // nombre del producto
    public decimal Quantity { get; set; } // cantidad
    public decimal UnitPrice { get; set; } // precio unitario
    public decimal Discount { get; set; } // descuento
    public decimal Subtotal { get; set; } // subtotal
    public string? Description { get; set; } // descripcion
    public List<InvoiceDetailTaxDto> Taxes { get; set; } = new(); // impuestos aplicados
}
public class InvoiceDetailTaxDto
{
    public int IdTax { get; set; } // id del impuesto
    public string? TaxName { get; set; } // nombre del impuesto
    public decimal TaxRate { get; set; } // tasa del impuesto
    public decimal TaxBase { get; set; } // base del impuesto
    public decimal TaxAmount { get; set; } // monto del impuesto
}
public class InvoicePaymentDto 
{
    public int IdPaymentMethod { get; set; } // id del metodo de pago
    public string? PaymentMethodName { get; set; } // nombre del metodo de pago
    public decimal Amount { get; set; } // monto
    public DateTime PaymentDate { get; set; } // fecha de pago
    public string? Reference { get; set; } // referencia
}