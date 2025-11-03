using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using InvoicesSystem.API.Models.Enums;
using System.ComponentModel.DataAnnotations;

namespace InvoicesSystem.API.Models.DTOs;

public class UpdateInvoiceDto
{
    [Required]
    public InvoiceStatus Status { get; set; }
    
    public DateTime? DueDate { get; set; }
    
    [StringLength(500)]
    public string? Notes { get; set; }
}
