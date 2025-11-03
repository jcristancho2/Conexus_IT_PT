using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using AutoMapper;
using InvoicesSystem.API.Models.DTOs;
using InvoicesSystem.API.Models.Entities;
using InvoicesSystem.API.Models.Enums;

namespace InvoicesSystem.API.Profiles;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        // Customer Mappings
        CreateMap<Customer, CustomerDto>();
        CreateMap<CreateCustomerDto, Customer>()
            .ForMember(dest => dest.IdCustomer, opt => opt.Ignore())
            .ForMember(dest => dest.CreatedAt, opt => opt.MapFrom(src => DateTime.UtcNow))
            .ForMember(dest => dest.UpdatedAt, opt => opt.Ignore());
        CreateMap<UpdateCustomerDto, Customer>()
            .ForMember(dest => dest.IdCustomer, opt => opt.Ignore())
            .ForMember(dest => dest.CreatedAt, opt => opt.Ignore())
            .ForMember(dest => dest.UpdatedAt, opt => opt.MapFrom(src => DateTime.UtcNow));

        // Product Mappings
        CreateMap<Product, ProductDto>();
        CreateMap<CreateProductDto, Product>()
            .ForMember(dest => dest.IdProduct, opt => opt.Ignore())
            .ForMember(dest => dest.CreatedAt, opt => opt.MapFrom(src => DateTime.UtcNow));
        CreateMap<UpdateProductDto, Product>()
            .ForMember(dest => dest.IdProduct, opt => opt.Ignore())
            .ForMember(dest => dest.CreatedAt, opt => opt.Ignore());

        // Invoice Mappings - con Customer e Issuer
        CreateMap<Invoice, InvoiceDto>()
            .ForMember(dest => dest.CustomerName, opt => opt.MapFrom(src =>
                src.Customer != null
                    ? (string.IsNullOrWhiteSpace(src.Customer.BusinessName)
                        ? ($"{src.Customer.FirstName} {src.Customer.LastName}").Trim()
                        : src.Customer.BusinessName)
                    : string.Empty))
            .ForMember(dest => dest.CustomerIdentification, opt => opt.MapFrom(src => src.Customer != null ? src.Customer.IdentificationNumber : string.Empty))
            .ForMember(dest => dest.IssuerBusinessName, opt => opt.MapFrom(src => src.Issuer != null ? src.Issuer.BusinessName : string.Empty))
            .ForMember(dest => dest.Details, opt => opt.MapFrom(src => src.InvoiceDetails))
            .ForMember(dest => dest.Payments, opt => opt.MapFrom(src => src.InvoicePayments));

        CreateMap<CreateInvoiceDto, Invoice>()
            .ForMember(dest => dest.IdInvoice, opt => opt.Ignore())
            .ForMember(dest => dest.InvoiceNumber, opt => opt.Ignore())
            .ForMember(dest => dest.InvoiceDate, opt => opt.MapFrom(src => DateTime.UtcNow))
            .ForMember(dest => dest.Status, opt => opt.MapFrom(src => InvoiceStatus.Draft))
            .ForMember(dest => dest.Subtotal, opt => opt.MapFrom(src => src.SubtotalAmount))
            .ForMember(dest => dest.TotalTax, opt => opt.MapFrom(src => src.TaxAmount))
            .ForMember(dest => dest.Total, opt => opt.MapFrom(src => src.TotalAmount))
            .ForMember(dest => dest.CreatedAt, opt => opt.MapFrom(src => DateTime.UtcNow))
            .ForMember(dest => dest.UpdatedAt, opt => opt.Ignore())
            .ForMember(dest => dest.InvoiceDetails, opt => opt.Ignore())
            .ForMember(dest => dest.InvoicePayments, opt => opt.Ignore());

        // UpdateInvoiceDto mapping - CON LAS NUEVAS PROPIEDADES
        CreateMap<UpdateInvoiceDto, Invoice>()
            .ForMember(dest => dest.IdInvoice, opt => opt.Ignore())
            .ForMember(dest => dest.InvoiceNumber, opt => opt.Ignore())
            .ForMember(dest => dest.InvoiceDate, opt => opt.Ignore())
            .ForMember(dest => dest.IdCustomer, opt => opt.MapFrom(src => src.IdCustomer)) // NUEVA
            .ForMember(dest => dest.Subtotal, opt => opt.MapFrom(src => src.SubtotalAmount)) // NUEVA
            .ForMember(dest => dest.TotalTax, opt => opt.MapFrom(src => src.TaxAmount)) // NUEVA
            .ForMember(dest => dest.Total, opt => opt.MapFrom(src => src.TotalAmount)) // NUEVA
            .ForMember(dest => dest.DueDate, opt => opt.MapFrom(src => src.DueDate.HasValue ? DateTime.SpecifyKind(src.DueDate.Value, DateTimeKind.Utc) : (DateTime?)null))
            .ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.Status))
            .ForMember(dest => dest.Notes, opt => opt.MapFrom(src => src.Notes))
            .ForMember(dest => dest.UpdatedAt, opt => opt.MapFrom(src => DateTime.UtcNow))
            .ForMember(dest => dest.CreatedAt, opt => opt.Ignore())
            .ForMember(dest => dest.IdIssuer, opt => opt.Ignore())
            .ForMember(dest => dest.InvoiceDetails, opt => opt.Ignore())
            .ForMember(dest => dest.InvoicePayments, opt => opt.Ignore());

        // InvoiceDetail Mappings
        CreateMap<InvoiceDetail, InvoiceDetailDto>()
            .ForMember(dest => dest.ProductName, opt => opt.MapFrom(src => src.Product != null ? src.Product.ProductName : ""))
            .ForMember(dest => dest.Taxes, opt => opt.MapFrom(src => src.InvoiceDetailTaxes));

        CreateMap<CreateInvoiceDetailDto, InvoiceDetail>()
            .ForMember(dest => dest.IdInvoice, opt => opt.Ignore())
            .ForMember(dest => dest.CreatedAt, opt => opt.MapFrom(src => DateTime.UtcNow))
            .ForMember(dest => dest.UpdatedAt, opt => opt.Ignore())
            .ForMember(dest => dest.Invoice, opt => opt.Ignore())
            .ForMember(dest => dest.Product, opt => opt.Ignore())
            .ForMember(dest => dest.InvoiceDetailTaxes, opt => opt.Ignore());

        // InvoiceDetailTax Mappings
        CreateMap<InvoiceDetailTax, InvoiceDetailTaxDto>()
            .ForMember(dest => dest.TaxName, opt => opt.MapFrom(src => src.Tax != null ? src.Tax.TaxName : ""))
            .ForMember(dest => dest.TaxRate, opt => opt.MapFrom(src => src.Tax != null ? src.Tax.TaxRate : 0));

        // InvoicePayment Mappings
        CreateMap<InvoicePayment, InvoicePaymentDto>()
            .ForMember(dest => dest.PaymentMethodName, opt => opt.MapFrom(src => src.PaymentMethod != null ? src.PaymentMethod.MethodName : ""));
    }
}