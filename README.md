# Sistema de Facturación Conexus IT

![Lenguaje](https://img.shields.io/badge/Lenguaje-ASP.NET%20Core%209.0+-512BD4?style=for-the-badge&logo=dotnet)
![Frontend](https://img.shields.io/badge/Frontend-React%20%2B%20Vite-61DAFB?style=for-the-badge&logo=react)
![Base de Datos](https://img.shields.io/badge/Base%20de%20Datos-PostgreSQL-4479A1?style=for-the-badge&logo=postgresql)
![Estado](https://img.shields.io/badge/Estado-Producción-%2328A745?style=for-the-badge)
![Docker](https://img.shields.io/badge/Docker-Disponible-%232496ED?style=for-the-badge&logo=docker)

Sistema completo de facturación desarrollado con .NET Core 9.0 en el backend y React + Vite en el frontend, con soporte completo para Docker.

---

## 📋 Tabla de Contenidos

- [Características](#-características)
- [Requisitos Previos](#-requisitos-previos)
- [Instalación](#-instalación)
  - [Instalación Local](#instalación-local)
  - [Instalación con Docker](#instalación-con-docker)
- [Configuración](#-configuración)
- [Uso](#-uso)
- [Pruebas de Endpoints](#-pruebas-de-endpoints)
- [Frontend](#-frontend)
- [Estructura del Proyecto](#-estructura-del-proyecto)
- [Tecnologías Utilizadas](#-tecnologías-utilizadas)

---

## ✨ Características

### Backend
- ✅ API RESTful con .NET Core 9.0
- ✅ Autenticación JWT
- ✅ Entity Framework Core con PostgreSQL
- ✅ Arquitectura en capas (Repository, Service, Controller)
- ✅ AutoMapper para mapeo de DTOs
- ✅ Swagger/OpenAPI para documentación
- ✅ Migraciones automáticas de base de datos
- ✅ Manejo centralizado de excepciones

### Frontend
- ✅ React 19 + Vite
- ✅ React Router DOM para navegación
- ✅ Tailwind CSS para estilos
- ✅ Tema claro/oscuro
- ✅ PWA (Progressive Web App)
- ✅ Responsive design
- ✅ Gráficos con Recharts
- ✅ Autenticación con JWT

### Funcionalidades
- 📄 Gestión completa de facturas (CRUD)
- 👥 Gestión de clientes
- 📦 Gestión de productos
- 📊 Dashboard con estadísticas y gráficos
- 🔍 Búsqueda y filtrado avanzado
- 📱 Interfaz responsive y PWA

---

## 🔧 Requisitos Previos

### Para Desarrollo Local
- **.NET SDK 9.0** o superior
- **Node.js 20** o superior y npm
- **PostgreSQL 16** o superior
- **Git**

### Para Docker
- **Docker** 20.10 o superior
- **Docker Compose** 2.0 o superior

---

## 📦 Instalación

### Instalación Local

#### 1. Clonar el Repositorio
```bash
git clone <repository-url>
cd Conexus_IT_PT
```

#### 2. Configurar Base de Datos

Crear la base de datos en PostgreSQL:
```sql
CREATE DATABASE billing_system;
```

Configurar la cadena de conexión en `Backend/InvoicesSystem.API/appsettings.json`:
```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Host=localhost;Port=5432;Database=billing_system;Username=postgres;Password=tu_password"
  }
}
```

#### 3. Configurar Backend

```bash
cd Backend/InvoicesSystem.API

# Restaurar dependencias
dotnet restore

# Aplicar migraciones
dotnet ef database update

# Opcional: Alimentar base de datos con datos de prueba
psql -U postgres -d billing_system -f Scripts/SeedDatabase.sql
```

#### 4. Configurar Frontend

```bash
cd Frontend

# Instalar dependencias
npm install

# Crear archivo .env (opcional, usa valores por defecto)
echo "VITE_API_BASE_URL=http://localhost:5012" > .env
```

#### 5. Ejecutar la Aplicación

**Terminal 1 - Backend:**
```bash
cd Backend/InvoicesSystem.API
dotnet run
```
El backend estará disponible en: `http://localhost:5012`

**Terminal 2 - Frontend:**
```bash
cd Frontend
npm run dev
```
El frontend estará disponible en: `http://localhost:5173`

### Instalación con Docker

#### Opción 1: Docker Compose (Recomendado)

```bash
# Desde la raíz del proyecto
cd docker

# Construir y ejecutar todos los servicios
docker-compose up -d --build

# Ver logs
docker-compose logs -f

# Detener servicios
docker-compose down

# Detener y eliminar volúmenes (incluye datos de BD)
docker-compose down -v
```

**Servicios disponibles:**
- **Frontend**: http://localhost:3000
- **Backend API**: http://localhost:8081
- **Swagger**: http://localhost:8081/swagger
- **pgAdmin**: http://localhost:8080
  - Email: `admin@admin.com`
  - Password: `admin`
- **PostgreSQL**: localhost:5434

#### Opción 2: Contenedores Individuales

**Base de Datos:**
```bash
docker run -d \
  --name billing_postgres \
  -e POSTGRES_DB=billing_system \
  -e POSTGRES_USER=postgres \
  -e POSTGRES_PASSWORD=postgres \
  -p 5434:5432 \
  postgres:16
```

**Backend:**
```bash
cd Backend/InvoicesSystem.API
docker build -f Api.Dockerfile -t billing-api .
docker run -d \
  --name billing_api \
  -p 8081:8080 \
  -e ConnectionStrings__PostgresDocker="Host=billing_postgres;Port=5432;Database=billing_system;Username=postgres;Password=postgres" \
  --link billing_postgres \
  billing-api
```

**Frontend:**
```bash
cd Frontend
docker build -t billing-frontend .
docker run -d \
  --name billing_frontend \
  -p 3000:80 \
  -e VITE_API_BASE_URL=http://localhost:8081 \
  billing-frontend
```

---

## ⚙️ Configuración

### Variables de Entorno

#### Backend (`appsettings.json`)
```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Host=localhost;Port=5432;Database=billing_system;Username=postgres;Password=postgres",
    "PostgresDocker": "Host=billing_postgres;Port=5432;Database=billing_system;Username=postgres;Password=postgres"
  },
  "Jwt": {
    "Key": "TuClaveSecretaMuyLargaParaJWT12345678901234567890",
    "Issuer": "InvoicesSystem.API",
    "Audience": "InvoicesSystem.Users",
    "ExpirationInMinutes": 1440
  }
}
```

#### Frontend (`.env`)
```env
VITE_API_BASE_URL=http://localhost:5012
```

### Alimentar Base de Datos

```bash
# Ejecutar script de seed
psql -U postgres -d billing_system -f Backend/InvoicesSystem.API/Scripts/SeedDatabase.sql

# Credenciales por defecto del usuario de prueba:
# Email: admin@test.com
# Password: Admin123!
```

---

## 🚀 Uso

### Acceso a la Aplicación

1. Abre el navegador en: `http://localhost:3000` (Docker) o `http://localhost:5173` (Local)
2. Inicia sesión con las credenciales:
   - **Email**: `admin@test.com`
   - **Password**: `Admin123!`

### Funcionalidades Principales

#### Gestión de Facturas
- Ver lista de facturas con paginación y filtros
- Crear nueva factura
- Editar factura existente
- Ver detalle completo de factura
- Cambiar estado de factura (Borrador → Finalizada)

#### Gestión de Clientes
- Listar clientes con búsqueda
- Crear nuevo cliente
- Ver detalles del cliente
- Editar cliente

#### Gestión de Productos
- Listar productos con búsqueda
- Crear nuevo producto
- Editar producto

#### Dashboard
- Estadísticas generales
- Gráfico de torta: Ingresos por producto
- Lista de productos más vendidos

---

## 🧪 Pruebas de Endpoints

### Autenticación

#### 1. Login
```bash
curl -X POST http://localhost:8081/api/Auth/login \
  -H "Content-Type: application/json" \
  -d '{
    "email": "admin@test.com",
    "password": "Admin123!"
  }'
```

**Respuesta esperada:**
```json
{
  "success": true,
  "message": "Inicio de sesión exitoso",
  "data": {
    "token": "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9...",
    "user": {
      "idUser": 1,
      "email": "admin@test.com",
      "firstName": "Admin",
      "lastName": "User"
    }
  }
}
```

**Guardar el token para usar en las siguientes peticiones:**
```bash
TOKEN="tu_token_aqui"
```

#### 2. Registro (Opcional)
```bash
curl -X POST http://localhost:8081/api/Auth/register \
  -H "Content-Type: application/json" \
  -d '{
    "email": "nuevo@usuario.com",
    "password": "Password123!",
    "firstName": "Nuevo",
    "lastName": "Usuario"
  }'
```

### Facturas

#### 1. Listar Facturas
```bash
curl -X GET "http://localhost:8081/api/Invoices?page=1&pageSize=10" \
  -H "Authorization: Bearer $TOKEN"
```

#### 2. Obtener Factura por ID
```bash
curl -X GET http://localhost:8081/api/Invoices/1 \
  -H "Authorization: Bearer $TOKEN"
```

#### 3. Crear Factura
```bash
curl -X POST http://localhost:8081/api/Invoices \
  -H "Authorization: Bearer $TOKEN" \
  -H "Content-Type: application/json" \
  -d '{
    "idCustomer": 1,
    "dueDate": "2025-12-31T00:00:00Z",
    "subtotalAmount": 1000.00,
    "taxAmount": 190.00,
    "totalAmount": 1190.00,
    "status": 0,
    "notes": "Factura de prueba",
    "details": [
      {
        "idProduct": 1,
        "quantity": 2,
        "unitPrice": 500.00,
        "discountAmount": 0,
        "taxAmount": 190.00,
        "totalAmount": 1000.00
      }
    ]
  }'
```

#### 4. Actualizar Factura
```bash
curl -X PUT http://localhost:8081/api/Invoices/1 \
  -H "Authorization: Bearer $TOKEN" \
  -H "Content-Type: application/json" \
  -d '{
    "idCustomer": 1,
    "dueDate": "2025-12-31T00:00:00Z",
    "subtotalAmount": 1500.00,
    "taxAmount": 285.00,
    "totalAmount": 1785.00,
    "status": 1,
    "notes": "Factura actualizada",
    "details": [
      {
        "idProduct": 1,
        "quantity": 3,
        "unitPrice": 500.00,
        "discountAmount": 0,
        "taxAmount": 285.00,
        "totalAmount": 1500.00
      }
    ]
  }'
```

#### 5. Eliminar Factura
```bash
curl -X DELETE http://localhost:8081/api/Invoices/1 \
  -H "Authorization: Bearer $TOKEN"
```

### Clientes

#### 1. Listar Clientes
```bash
curl -X GET "http://localhost:8081/api/Customers?page=1&pageSize=10&search=Juan" \
  -H "Authorization: Bearer $TOKEN"
```

#### 2. Obtener Cliente por ID
```bash
curl -X GET http://localhost:8081/api/Customers/1 \
  -H "Authorization: Bearer $TOKEN"
```

#### 3. Crear Cliente
```bash
curl -X POST http://localhost:8081/api/Customers \
  -H "Authorization: Bearer $TOKEN" \
  -H "Content-Type: application/json" \
  -d '{
    "idTypeIdentification": 1,
    "identificationNumber": "1234567890",
    "personType": 0,
    "firstName": "Juan",
    "lastName": "Pérez",
    "idCity": 1,
    "fullAddress": "Calle 123 #45-67",
    "idTaxRegime": 1,
    "idTaxResponsibility": 1,
    "contacts": [
      {
        "contactType": 0,
        "contactValue": "juan@example.com"
      },
      {
        "contactType": 1,
        "contactValue": "3001234567"
      }
    ]
  }'
```

#### 4. Actualizar Cliente
```bash
curl -X PUT http://localhost:8081/api/Customers/1 \
  -H "Authorization: Bearer $TOKEN" \
  -H "Content-Type: application/json" \
  -d '{
    "idTypeIdentification": 1,
    "identificationNumber": "1234567890",
    "personType": 0,
    "firstName": "Juan Carlos",
    "lastName": "Pérez",
    "idCity": 1,
    "fullAddress": "Calle 123 #45-67",
    "idTaxRegime": 1,
    "idTaxResponsibility": 1
  }'
```

#### 5. Eliminar Cliente
```bash
curl -X DELETE http://localhost:8081/api/Customers/1 \
  -H "Authorization: Bearer $TOKEN"
```

### Productos

#### 1. Listar Productos
```bash
curl -X GET "http://localhost:8081/api/Products?page=1&pageSize=10&search=laptop" \
  -H "Authorization: Bearer $TOKEN"
```

#### 2. Obtener Producto por ID
```bash
curl -X GET http://localhost:8081/api/Products/1 \
  -H "Authorization: Bearer $TOKEN"
```

#### 3. Crear Producto
```bash
curl -X POST http://localhost:8081/api/Products \
  -H "Authorization: Bearer $TOKEN" \
  -H "Content-Type: application/json" \
  -d '{
    "codeProduct": "PROD001",
    "productName": "Laptop Dell",
    "description": "Laptop Dell Inspiron 15",
    "unitPrice": 1500000.00,
    "unitMeasure": "Unidad",
    "isActive": true
  }'
```

#### 4. Actualizar Producto
```bash
curl -X PUT http://localhost:8081/api/Products/1 \
  -H "Authorization: Bearer $TOKEN" \
  -H "Content-Type: application/json" \
  -d '{
    "codeProduct": "PROD001",
    "productName": "Laptop Dell (Actualizada)",
    "description": "Laptop Dell Inspiron 15 - Actualizada",
    "unitPrice": 1600000.00,
    "unitMeasure": "Unidad",
    "isActive": true
  }'
```

#### 5. Eliminar Producto
```bash
curl -X DELETE http://localhost:8081/api/Products/1 \
  -H "Authorization: Bearer $TOKEN"
```

### Dashboard

#### 1. Obtener Estadísticas
```bash
curl -X GET "http://localhost:8081/api/Dashboard/stats?startDate=2025-01-01&endDate=2025-12-31" \
  -H "Authorization: Bearer $TOKEN"
```

#### 2. Obtener Ingresos por Producto
```bash
curl -X GET "http://localhost:8081/api/Dashboard/products-revenue?startDate=2025-01-01&endDate=2025-12-31" \
  -H "Authorization: Bearer $TOKEN"
```

### Usando Swagger

1. Abre: `http://localhost:8081/swagger`
2. Haz clic en "Authorize" e ingresa: `Bearer tu_token_aqui`
3. Explora y prueba todos los endpoints desde la interfaz

---

## 💻 Frontend

### Arquitectura

El frontend está construido con **React 19** y **Vite**, siguiendo una arquitectura modular:

```
Frontend/
├── src/
│   ├── api/              # Servicios API (axios)
│   ├── components/       # Componentes reutilizables
│   ├── contexts/         # Context API (Theme)
│   ├── hooks/            # Custom hooks (useAuth)
│   ├── routes/           # Páginas y rutas
│   └── main.tsx          # Punto de entrada
```

### Funcionalidades del Frontend

#### 1. Autenticación
- **Login**: Formulario de inicio de sesión con validación
- **JWT Storage**: Token almacenado en `localStorage`
- **Protected Routes**: Rutas protegidas que requieren autenticación
- **Logout**: Cierre de sesión y limpieza de token

#### 2. Gestión de Estado
- **Context API**: Para el tema (claro/oscuro)
- **Local Storage**: Para persistencia del token y preferencias
- **React Router**: Para navegación y gestión de rutas

#### 3. Componentes Principales

**NavBar**: Barra de navegación con:
- Logo de la aplicación
- Enlaces a secciones principales
- Botón de cambio de tema
- Información del usuario
- Botón de logout

**ThemeToggle**: Botón para cambiar entre tema claro y oscuro

**Logo**: Componente reutilizable para el logo de la aplicación

#### 4. Páginas

**LoginPage** (`/login`):
- Formulario de autenticación
- Manejo de errores
- Redirección automática después del login

**InvoicesListPage** (`/invoices`):
- Lista paginada de facturas
- Filtros: búsqueda, fecha desde, total mínimo
- Acciones: Ver, Editar, Eliminar, Finalizar

**InvoiceFormPage** (`/invoices/new`, `/invoices/:id/edit`):
- Formulario para crear/editar facturas
- Selector de cliente
- Agregar/eliminar productos
- Cálculo automático de totales

**InvoiceDetailPage** (`/invoices/:id`):
- Vista detallada de factura
- Información del cliente
- Detalle de productos con precios, descuentos e impuestos
- Resumen financiero

**CustomersListPage** (`/customers`):
- Lista paginada de clientes
- Búsqueda por identificación o nombre
- Acción: Ver detalles

**CustomerFormPage** (`/customers/new`):
- Formulario para crear cliente
- Campos para persona natural/jurídica
- Gestión de contactos (email, teléfono)

**CustomerDetailPage** (`/customers/:id`):
- Vista detallada del cliente
- Información completa
- Lista de contactos

**ProductsListPage** (`/products`):
- Lista paginada de productos
- Búsqueda por código o nombre
- Estado activo/inactivo

**ProductFormPage** (`/products/new`):
- Formulario para crear producto
- Campos: código, nombre, descripción, precio, unidad de medida

**DashboardPage** (`/dashboard`):
- Gráfico de torta: Ingresos por producto
- Lista de productos más vendidos
- Estadísticas generales

#### 5. Servicios API

Todos los servicios API están en `src/api/`:

- **http.ts**: Configuración de Axios con interceptores para JWT
- **auth.ts**: Login y registro
- **invoices.ts**: CRUD de facturas
- **customers.ts**: CRUD de clientes
- **products.ts**: CRUD de productos
- **dashboard.ts**: Estadísticas y gráficos

#### 6. Tema Claro/Oscuro

- **ThemeContext**: Context API para gestión del tema
- **Persistencia**: Preferencia guardada en `localStorage`
- **Detección automática**: Respeta preferencias del sistema
- **Aplicación global**: Clases Tailwind `dark:` aplicadas automáticamente

#### 7. PWA (Progressive Web App)

- **Service Worker**: Para caché offline
- **Manifest**: Configuración de PWA
- **Instalable**: Puede instalarse como app móvil/desktop

### Scripts Disponibles

```bash
# Desarrollo
npm run dev          # Servidor de desarrollo en http://localhost:5173

# Producción
npm run build        # Compilar para producción
npm run preview      # Previsualizar build de producción

# Linting
npm run lint         # Ejecutar ESLint
```

### Variables de Entorno

Crear archivo `.env` en la raíz de `Frontend/`:

```env
VITE_API_BASE_URL=http://localhost:5012
```

---

## 📁 Estructura del Proyecto

```
Conexus_IT_PT/
├── Backend/
│   └── InvoicesSystem.API/
│       ├── Controllers/         # Controladores API
│       ├── Models/              # Entidades, DTOs, Enums
│       ├── Repositories/        # Capa de acceso a datos
│       ├── Services/            # Lógica de negocio
│       ├── Persistence/         # DbContext y Migraciones
│       ├── Profiles/            # AutoMapper profiles
│       ├── Middleware/          # Middleware personalizado
│       ├── Scripts/             # Scripts SQL (seed, etc.)
│       └── Program.cs           # Configuración de la aplicación
│
├── Frontend/
│   ├── src/
│   │   ├── api/                 # Servicios API
│   │   ├── components/          # Componentes React
│   │   ├── contexts/            # Context API
│   │   ├── hooks/               # Custom hooks
│   │   ├── routes/              # Páginas y rutas
│   │   └── assets/              # Recursos estáticos
│   ├── public/                  # Archivos públicos
│   └── package.json            # Dependencias
│
├── docker/
│   └── docker-compose.yml       # Configuración Docker Compose
│
├── .devcontainer/
│   └── devcontainer.json         # Configuración VS Code Dev Container
│
└── README.md                    # Este archivo
```

---

## 🛠️ Tecnologías Utilizadas

### Backend
- **.NET Core 9.0**
- **Entity Framework Core 9.0**
- **PostgreSQL 16**
- **AutoMapper**
- **JWT Authentication**
- **Swagger/OpenAPI**

### Frontend
- **React 19**
- **Vite 7**
- **TypeScript**
- **Tailwind CSS 3**
- **React Router DOM 7**
- **Axios**
- **Recharts**

### DevOps
- **Docker**
- **Docker Compose**
- **Nginx** (para frontend en producción)

---

## 📝 Notas Adicionales

### Credenciales por Defecto

**Usuario de Prueba:**
- Email: `admin@test.com`
- Password: `Admin123!`

**Base de Datos:**
- Host: `localhost` (local) / `billing_postgres` (Docker)
- Puerto: `5432` (local) / `5434` (Docker)
- Database: `billing_system`
- Usuario: `postgres`
- Password: `postgres`

### Migraciones

Las migraciones se aplican automáticamente en Docker. Para desarrollo local:

```bash
cd Backend/InvoicesSystem.API
dotnet ef database update
```

### Alimentar Base de Datos

```bash
psql -U postgres -d billing_system -f Backend/InvoicesSystem.API/Scripts/SeedDatabase.sql
```

### Solución de Problemas

**Error de conexión a la base de datos:**
- Verificar que PostgreSQL esté ejecutándose
- Verificar la cadena de conexión en `appsettings.json`
- En Docker, verificar que los servicios estén en la misma red

**Error de CORS:**
- Verificar que el backend tenga CORS configurado
- Verificar que la URL del frontend esté permitida

**Error de autenticación:**
- Verificar que el token JWT sea válido
- Verificar que el token no haya expirado
- Verificar que el header `Authorization` esté presente

---

## 👨‍💻 Autor

**Jorge Andres Cristancho Olarte**

- GitHub: [@jcristancho2](https://github.com/jcristancho2)

---

## 📄 Licencia

Este proyecto es parte de una prueba técnica para Conexus IT.

---

## 🎯 Próximos Pasos

- [ ] Implementar pruebas unitarias
- [ ] Implementar pruebas de integración
- [ ] Agregar más gráficos al dashboard
- [ ] Implementar exportación de facturas (PDF)
- [ ] Agregar notificaciones push
- [ ] Implementar caché en Redis
- [ ] Agregar logging estructurado

---

**¡Gracias por usar el Sistema de Facturación Conexus IT! 🚀**
