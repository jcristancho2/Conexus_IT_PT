using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Microsoft.EntityFrameworkCore;
using InvoicesSystem.API.Models.Entities;

namespace InvoicesSystem.API.Persistence;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    
    }
    // DbSets Ubicación
    public DbSet<Country> Countries { get; set; }
    public DbSet<Department> Departments { get; set; }
    public DbSet<City> Cities { get; set; }
    public DbSet<Address> Addresses { get; set; }

    // DbSets Tipos
    public DbSet<TypeIdentification> TypeIdentifications { get; set; }
    public DbSet<TaxRegime> TaxRegimes { get; set; }
    public DbSet<TaxResponsibility> TaxResponsibilities { get; set; }
    public DbSet<PaymentMethod> PaymentMethods { get; set; }
    public DbSet<Tax> Taxes { get; set; }

    // DbSets Productos
    public DbSet<Product> Products { get; set; }
    public DbSet<ProductTax> ProductTaxes { get; set; }
    // DbSets Clientes
    public DbSet<Customer> Customers { get; set; }
    public DbSet<CustomerContact> CustomerContacts { get; set; }

    // DbSets Emisor
    public DbSet<Issuer> Issuers { get; set; }

    // DbSets Facturación
    public DbSet<Invoice> Invoices { get; set; }
    public DbSet<InvoiceDetail> InvoiceDetails { get; set; }
    public DbSet<InvoiceDetailTax> InvoiceDetailTaxes { get; set; }
    public DbSet<InvoicePayment> InvoicePayments { get; set; }

    public DbSet<User> Users { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Configuración de claves compuestas
        modelBuilder.Entity<ProductTax>()
            .HasKey(pt => new { pt.IdProduct, pt.IdTax });
        
        modelBuilder.Entity<InvoiceDetail>()
            .HasKey(id => new { id.IdInvoice, id.IdProduct });
        
        modelBuilder.Entity<InvoiceDetailTax>()
            .HasKey(idt => new { idt.IdInvoice, idt.IdProduct, idt.IdTax });
        
        modelBuilder.Entity<InvoicePayment>()
            .HasKey(ip => new { ip.IdInvoice, ip.IdPaymentMethod });

        // Configuración de relaciones InvoiceDetailTax
        modelBuilder.Entity<InvoiceDetailTax>()
            .HasOne(idt => idt.InvoiceDetail)
            .WithMany(id => id.InvoiceDetailTaxes)
            .HasForeignKey(idt => new { idt.IdInvoice, idt.IdProduct })
            .OnDelete(DeleteBehavior.Cascade);
        
        modelBuilder.Entity<InvoiceDetailTax>()
            .HasOne(idt => idt.Tax)
            .WithMany(t => t.InvoiceDetailTaxes)
            .HasForeignKey(idt => idt.IdTax)
            .OnDelete(DeleteBehavior.Restrict);

        // Configuración enums
        modelBuilder.HasPostgresEnum<Models.Enums.InvoiceStatus>();
        modelBuilder.HasPostgresEnum<Models.Enums.PersonType>();
        modelBuilder.HasPostgresEnum<Models.Enums.ContactType>();

        // Configuración explícita de Invoice - SIMPLIFICADA
        modelBuilder.Entity<Invoice>(entity =>
        {
            entity.HasKey(e => e.IdInvoice);
            entity.ToTable("invoice");
            
            // Configurar la clave primaria con auto-generación
            entity.Property(e => e.IdInvoice)
                .HasColumnName("id_invoice")
                .ValueGeneratedOnAdd();
            
            // Solo foreign keys, SIN relaciones de navegación
            entity.Property(e => e.IdCustomer).HasColumnName("id_customer");
            entity.Property(e => e.IdIssuer).HasColumnName("id_issuer");

            // Solo las colecciones que SÍ existen
            entity.HasMany(e => e.InvoiceDetails)
                .WithOne(d => d.Invoice)
                .HasForeignKey(d => d.IdInvoice)
                .OnDelete(DeleteBehavior.Cascade);

            entity.HasMany(e => e.InvoicePayments)
                .WithOne(p => p.Invoice)
                .HasForeignKey(p => p.IdInvoice)
                .OnDelete(DeleteBehavior.Cascade);
        });

    }
}
