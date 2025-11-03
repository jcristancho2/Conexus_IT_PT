using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace InvoicesSystem.API.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterDatabase()
                .Annotation("Npgsql:Enum:contact_type", "email,phone,other")
                .Annotation("Npgsql:Enum:invoice_status", "draft,final,cancelled")
                .Annotation("Npgsql:Enum:person_type", "natural,juridica");

            migrationBuilder.CreateTable(
                name: "country",
                columns: table => new
                {
                    id_country = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    cod_country = table.Column<string>(type: "character varying(2)", maxLength: 2, nullable: false),
                    name_country = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_country", x => x.id_country);
                });

            migrationBuilder.CreateTable(
                name: "payment_method",
                columns: table => new
                {
                    id_payment_method = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    method_name = table.Column<string>(type: "character varying(60)", maxLength: 60, nullable: false),
                    description = table.Column<string>(type: "character varying(150)", maxLength: 150, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_payment_method", x => x.id_payment_method);
                });

            migrationBuilder.CreateTable(
                name: "product",
                columns: table => new
                {
                    id_product = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    code_product = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    product_name = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    description = table.Column<string>(type: "text", nullable: true),
                    unit_price = table.Column<decimal>(type: "numeric(18,2)", nullable: false),
                    unit_measure = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                    is_active = table.Column<bool>(type: "boolean", nullable: false),
                    created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_product", x => x.id_product);
                });

            migrationBuilder.CreateTable(
                name: "tax",
                columns: table => new
                {
                    id_tax = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    tax_name = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    tax_rate = table.Column<decimal>(type: "numeric(7,4)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tax", x => x.id_tax);
                });

            migrationBuilder.CreateTable(
                name: "tax_regime",
                columns: table => new
                {
                    id_tax_regime = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    code = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    description = table.Column<string>(type: "character varying(150)", maxLength: 150, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tax_regime", x => x.id_tax_regime);
                });

            migrationBuilder.CreateTable(
                name: "tax_responsibility",
                columns: table => new
                {
                    id_tax_responsibility = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    code = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    description = table.Column<string>(type: "character varying(150)", maxLength: 150, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tax_responsibility", x => x.id_tax_responsibility);
                });

            migrationBuilder.CreateTable(
                name: "type_identification",
                columns: table => new
                {
                    id_type_identification = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    code = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                    description = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_type_identification", x => x.id_type_identification);
                });

            migrationBuilder.CreateTable(
                name: "department",
                columns: table => new
                {
                    id_department = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    name_department = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    id_country = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_department", x => x.id_department);
                    table.ForeignKey(
                        name: "FK_department_country_id_country",
                        column: x => x.id_country,
                        principalTable: "country",
                        principalColumn: "id_country",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "product_tax",
                columns: table => new
                {
                    id_product = table.Column<int>(type: "integer", nullable: false),
                    id_tax = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_product_tax", x => new { x.id_product, x.id_tax });
                    table.ForeignKey(
                        name: "FK_product_tax_product_id_product",
                        column: x => x.id_product,
                        principalTable: "product",
                        principalColumn: "id_product",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_product_tax_tax_id_tax",
                        column: x => x.id_tax,
                        principalTable: "tax",
                        principalColumn: "id_tax",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "city",
                columns: table => new
                {
                    id_city = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    name_city = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    id_department = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_city", x => x.id_city);
                    table.ForeignKey(
                        name: "FK_city_department_id_department",
                        column: x => x.id_department,
                        principalTable: "department",
                        principalColumn: "id_department",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "address",
                columns: table => new
                {
                    id_address = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    full_address = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    id_city = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_address", x => x.id_address);
                    table.ForeignKey(
                        name: "FK_address_city_id_city",
                        column: x => x.id_city,
                        principalTable: "city",
                        principalColumn: "id_city",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "customer",
                columns: table => new
                {
                    id_customer = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    id_address = table.Column<int>(type: "integer", nullable: false),
                    id_type_identification = table.Column<int>(type: "integer", nullable: false),
                    id_tax_regime = table.Column<int>(type: "integer", nullable: false),
                    id_tax_responsibility = table.Column<int>(type: "integer", nullable: false),
                    identification_number = table.Column<string>(type: "character varying(30)", maxLength: 30, nullable: false),
                    person_type = table.Column<int>(type: "integer", nullable: false),
                    first_name = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    last_name = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    business_name = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: true),
                    commercial_name = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    email = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    password_hash = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    updated_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_customer", x => x.id_customer);
                    table.ForeignKey(
                        name: "FK_customer_address_id_address",
                        column: x => x.id_address,
                        principalTable: "address",
                        principalColumn: "id_address",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_customer_tax_regime_id_tax_regime",
                        column: x => x.id_tax_regime,
                        principalTable: "tax_regime",
                        principalColumn: "id_tax_regime",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_customer_tax_responsibility_id_tax_responsibility",
                        column: x => x.id_tax_responsibility,
                        principalTable: "tax_responsibility",
                        principalColumn: "id_tax_responsibility",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_customer_type_identification_id_type_identification",
                        column: x => x.id_type_identification,
                        principalTable: "type_identification",
                        principalColumn: "id_type_identification",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "issuer",
                columns: table => new
                {
                    id_issuer = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    id_address = table.Column<int>(type: "integer", nullable: false),
                    id_tax_regime = table.Column<int>(type: "integer", nullable: false),
                    id_tax_responsibility = table.Column<int>(type: "integer", nullable: false),
                    identification_number = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    business_name = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    commercial_name = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    email = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    phone = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: true),
                    website = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    updated_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_issuer", x => x.id_issuer);
                    table.ForeignKey(
                        name: "FK_issuer_address_id_address",
                        column: x => x.id_address,
                        principalTable: "address",
                        principalColumn: "id_address",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_issuer_tax_regime_id_tax_regime",
                        column: x => x.id_tax_regime,
                        principalTable: "tax_regime",
                        principalColumn: "id_tax_regime",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_issuer_tax_responsibility_id_tax_responsibility",
                        column: x => x.id_tax_responsibility,
                        principalTable: "tax_responsibility",
                        principalColumn: "id_tax_responsibility",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "customer_contact",
                columns: table => new
                {
                    id_customer_contact = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    id_customer = table.Column<int>(type: "integer", nullable: false),
                    contact_type = table.Column<int>(type: "integer", nullable: false),
                    contact_value = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    updated_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_customer_contact", x => x.id_customer_contact);
                    table.ForeignKey(
                        name: "FK_customer_contact_customer_id_customer",
                        column: x => x.id_customer,
                        principalTable: "customer",
                        principalColumn: "id_customer",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "invoice",
                columns: table => new
                {
                    id_invoice = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    id_customer = table.Column<int>(type: "integer", nullable: false),
                    id_issuer = table.Column<int>(type: "integer", nullable: false),
                    invoice_number = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    invoice_date = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    due_date = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    status = table.Column<int>(type: "integer", nullable: false),
                    subtotal = table.Column<decimal>(type: "numeric(18,2)", nullable: false),
                    total_tax = table.Column<decimal>(type: "numeric(18,2)", nullable: false),
                    total = table.Column<decimal>(type: "numeric(18,2)", nullable: false),
                    notes = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    updated_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    CustomerIdCustomer = table.Column<int>(type: "integer", nullable: true),
                    IssuerIdIssuer = table.Column<int>(type: "integer", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_invoice", x => x.id_invoice);
                    table.ForeignKey(
                        name: "FK_invoice_customer_CustomerIdCustomer",
                        column: x => x.CustomerIdCustomer,
                        principalTable: "customer",
                        principalColumn: "id_customer");
                    table.ForeignKey(
                        name: "FK_invoice_issuer_IssuerIdIssuer",
                        column: x => x.IssuerIdIssuer,
                        principalTable: "issuer",
                        principalColumn: "id_issuer");
                });

            migrationBuilder.CreateTable(
                name: "invoice_detail",
                columns: table => new
                {
                    id_invoice = table.Column<int>(type: "integer", nullable: false),
                    id_product = table.Column<int>(type: "integer", nullable: false),
                    quantity = table.Column<decimal>(type: "numeric(18,2)", nullable: false),
                    unit_price = table.Column<decimal>(type: "numeric(18,2)", nullable: false),
                    discount = table.Column<decimal>(type: "numeric(18,2)", nullable: false),
                    subtotal = table.Column<decimal>(type: "numeric(18,2)", nullable: false),
                    description = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: true),
                    created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    updated_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_invoice_detail", x => new { x.id_invoice, x.id_product });
                    table.ForeignKey(
                        name: "FK_invoice_detail_invoice_id_invoice",
                        column: x => x.id_invoice,
                        principalTable: "invoice",
                        principalColumn: "id_invoice",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_invoice_detail_product_id_product",
                        column: x => x.id_product,
                        principalTable: "product",
                        principalColumn: "id_product",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "invoice_payment",
                columns: table => new
                {
                    id_invoice = table.Column<int>(type: "integer", nullable: false),
                    id_payment_method = table.Column<int>(type: "integer", nullable: false),
                    amount = table.Column<decimal>(type: "numeric(18,2)", nullable: false),
                    payment_date = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    reference = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    updated_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_invoice_payment", x => new { x.id_invoice, x.id_payment_method });
                    table.ForeignKey(
                        name: "FK_invoice_payment_invoice_id_invoice",
                        column: x => x.id_invoice,
                        principalTable: "invoice",
                        principalColumn: "id_invoice",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_invoice_payment_payment_method_id_payment_method",
                        column: x => x.id_payment_method,
                        principalTable: "payment_method",
                        principalColumn: "id_payment_method",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "invoice_detail_tax",
                columns: table => new
                {
                    id_invoice = table.Column<int>(type: "integer", nullable: false),
                    id_product = table.Column<int>(type: "integer", nullable: false),
                    id_tax = table.Column<int>(type: "integer", nullable: false),
                    tax_base = table.Column<decimal>(type: "numeric(18,2)", nullable: false),
                    tax_amount = table.Column<decimal>(type: "numeric(18,2)", nullable: false),
                    created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    updated_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_invoice_detail_tax", x => new { x.id_invoice, x.id_product, x.id_tax });
                    table.ForeignKey(
                        name: "FK_invoice_detail_tax_invoice_detail_id_invoice_id_product",
                        columns: x => new { x.id_invoice, x.id_product },
                        principalTable: "invoice_detail",
                        principalColumns: new[] { "id_invoice", "id_product" },
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_invoice_detail_tax_tax_id_tax",
                        column: x => x.id_tax,
                        principalTable: "tax",
                        principalColumn: "id_tax",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_address_id_city",
                table: "address",
                column: "id_city");

            migrationBuilder.CreateIndex(
                name: "IX_city_id_department",
                table: "city",
                column: "id_department");

            migrationBuilder.CreateIndex(
                name: "IX_customer_id_address",
                table: "customer",
                column: "id_address");

            migrationBuilder.CreateIndex(
                name: "IX_customer_id_tax_regime",
                table: "customer",
                column: "id_tax_regime");

            migrationBuilder.CreateIndex(
                name: "IX_customer_id_tax_responsibility",
                table: "customer",
                column: "id_tax_responsibility");

            migrationBuilder.CreateIndex(
                name: "IX_customer_id_type_identification",
                table: "customer",
                column: "id_type_identification");

            migrationBuilder.CreateIndex(
                name: "IX_customer_contact_id_customer",
                table: "customer_contact",
                column: "id_customer");

            migrationBuilder.CreateIndex(
                name: "IX_department_id_country",
                table: "department",
                column: "id_country");

            migrationBuilder.CreateIndex(
                name: "IX_invoice_CustomerIdCustomer",
                table: "invoice",
                column: "CustomerIdCustomer");

            migrationBuilder.CreateIndex(
                name: "IX_invoice_IssuerIdIssuer",
                table: "invoice",
                column: "IssuerIdIssuer");

            migrationBuilder.CreateIndex(
                name: "IX_invoice_detail_id_product",
                table: "invoice_detail",
                column: "id_product");

            migrationBuilder.CreateIndex(
                name: "IX_invoice_detail_tax_id_tax",
                table: "invoice_detail_tax",
                column: "id_tax");

            migrationBuilder.CreateIndex(
                name: "IX_invoice_payment_id_payment_method",
                table: "invoice_payment",
                column: "id_payment_method");

            migrationBuilder.CreateIndex(
                name: "IX_issuer_id_address",
                table: "issuer",
                column: "id_address");

            migrationBuilder.CreateIndex(
                name: "IX_issuer_id_tax_regime",
                table: "issuer",
                column: "id_tax_regime");

            migrationBuilder.CreateIndex(
                name: "IX_issuer_id_tax_responsibility",
                table: "issuer",
                column: "id_tax_responsibility");

            migrationBuilder.CreateIndex(
                name: "IX_product_tax_id_tax",
                table: "product_tax",
                column: "id_tax");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "customer_contact");

            migrationBuilder.DropTable(
                name: "invoice_detail_tax");

            migrationBuilder.DropTable(
                name: "invoice_payment");

            migrationBuilder.DropTable(
                name: "product_tax");

            migrationBuilder.DropTable(
                name: "invoice_detail");

            migrationBuilder.DropTable(
                name: "payment_method");

            migrationBuilder.DropTable(
                name: "tax");

            migrationBuilder.DropTable(
                name: "invoice");

            migrationBuilder.DropTable(
                name: "product");

            migrationBuilder.DropTable(
                name: "customer");

            migrationBuilder.DropTable(
                name: "issuer");

            migrationBuilder.DropTable(
                name: "type_identification");

            migrationBuilder.DropTable(
                name: "address");

            migrationBuilder.DropTable(
                name: "tax_regime");

            migrationBuilder.DropTable(
                name: "tax_responsibility");

            migrationBuilder.DropTable(
                name: "city");

            migrationBuilder.DropTable(
                name: "department");

            migrationBuilder.DropTable(
                name: "country");
        }
    }
}
