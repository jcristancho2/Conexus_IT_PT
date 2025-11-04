using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;

using InvoicesSystem.API.Persistence;
using InvoicesSystem.API.Repositories;
using InvoicesSystem.API.Repositories.Interfaces;
using InvoicesSystem.API.Services;
using InvoicesSystem.API.Services.Interfaces;
using InvoicesSystem.API.Profiles;

var builder = WebApplication.CreateBuilder(args);

// ✅ CONFIGURACIÓN DE BASE DE DATOS - DETECTAR ENTORNO
string connectionString;
if (Environment.GetEnvironmentVariable("DOTNET_RUNNING_IN_CONTAINER") == "true")
{
    // Ejecutándose en Docker - usar conexión Docker
    connectionString = builder.Configuration.GetConnectionString("PostgresDocker") 
        ?? throw new InvalidOperationException("PostgresDocker connection string not found");
}
else
{
    // Ejecutándose localmente - usar conexión local
    connectionString = builder.Configuration.GetConnectionString("DefaultConnection") 
        ?? throw new InvalidOperationException("DefaultConnection connection string not found");
}

builder.Services.AddDbContext<AppDbContext>(options =>
{
    options.UseNpgsql(connectionString);
    options.EnableSensitiveDataLogging(builder.Environment.IsDevelopment());
});

// Repositories
builder.Services.AddScoped<ICustomerRepository, CustomerRepository>();
builder.Services.AddScoped<IProductRepository, ProductRepository>();
builder.Services.AddScoped<IInvoiceRepository, InvoiceRepository>();

// Services
builder.Services.AddScoped<ICustomerService, CustomerService>();
builder.Services.AddScoped<IProductService, ProductService>();
builder.Services.AddScoped<IInvoiceService, InvoiceService>();
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<IDashboardService, DashboardService>();

// AutoMapper
builder.Services.AddAutoMapper(typeof(MappingProfile));

// Asegurar que la configuración JWT esté correcta

// JWT Configuration
var jwtKey = builder.Configuration["Jwt:Key"];
if (string.IsNullOrEmpty(jwtKey))
{
    throw new InvalidOperationException("JWT Key no está configurado");
}

var key = Encoding.UTF8.GetBytes(jwtKey);

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = builder.Configuration["Jwt:Issuer"],
            ValidAudience = builder.Configuration["Jwt:Audience"],
            IssuerSigningKey = new SymmetricSecurityKey(key),
            ClockSkew = TimeSpan.Zero
        };
    });

builder.Services.AddAuthorization();

// Controllers
builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.PropertyNamingPolicy = System.Text.Json.JsonNamingPolicy.CamelCase;
        options.JsonSerializerOptions.WriteIndented = true;
    });

// Swagger
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo 
    { 
        Title = "Invoices System API", 
        Version = "v1",
        Description = "API para el sistema de facturación"
    });

    // JWT Authentication in Swagger
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Description = "JWT Authorization header using the Bearer scheme. Example: \"Authorization: Bearer {token}\"",
        Name = "Authorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer"
    });

    c.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            Array.Empty<string>()
        }
    });
});

// CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyMethod()
              .AllowAnyHeader();
    });
});

var app = builder.Build();

// Configure the HTTP request pipeline
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "Invoices System API v1");
        c.RoutePrefix = "swagger";
    });
}

// Middleware


app.UseCors("AllowAll");

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

// ✅ APLICAR MIGRACIONES AUTOMÁTICAMENTE EN DOCKER
if (Environment.GetEnvironmentVariable("DOTNET_RUNNING_IN_CONTAINER") == "true")
{
    using (var scope = app.Services.CreateScope())
    {
        try
        {
            var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();
            
            // Esperar a que la base de datos esté disponible
            await WaitForDatabase(context);
            
            // Aplicar migraciones pendientes
            await context.Database.MigrateAsync();
            
            Console.WriteLine("✅ Migraciones aplicadas exitosamente");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"❌ Error aplicando migraciones: {ex.Message}");
            // No lanzar la excepción para que la app pueda seguir ejecutándose
        }
    }
}

app.Run();

// ✅ MÉTODO PARA ESPERAR A QUE LA BASE DE DATOS ESTÉ DISPONIBLE
static async Task WaitForDatabase(AppDbContext context, int maxRetries = 30)
{
    for (int i = 0; i < maxRetries; i++)
    {
        try
        {
            await context.Database.CanConnectAsync();
            Console.WriteLine("✅ Conexión a base de datos establecida");
            return;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"⏳ Esperando conexión a base de datos... Intento {i + 1}/{maxRetries}");
            Console.WriteLine($"Error: {ex.Message}");
            
            if (i == maxRetries - 1)
            {
                throw new Exception($"No se pudo conectar a la base de datos después de {maxRetries} intentos");
            }
            
            await Task.Delay(2000); // Esperar 2 segundos
        }
    }
}