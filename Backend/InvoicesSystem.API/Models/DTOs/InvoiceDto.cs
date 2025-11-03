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


