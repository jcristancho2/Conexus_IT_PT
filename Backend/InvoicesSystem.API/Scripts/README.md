# Scripts de Base de Datos

## SeedDatabase.sql

Script SQL para poblar la base de datos con datos de prueba.

### Uso

#### Opción 1: Ejecutar con psql
```bash
psql -U postgres -d InvoicesSystem -f Scripts/SeedDatabase.sql
```

#### Opción 2: Ejecutar con pgAdmin
1. Abre pgAdmin
2. Conéctate a tu base de datos `InvoicesSystem`
3. Click derecho en la base de datos > Query Tool
4. Abre el archivo `SeedDatabase.sql`
5. Ejecuta el script (F5)

#### Opción 3: Ejecutar desde la línea de comandos de PostgreSQL
```bash
PGPASSWORD=tu_password psql -h localhost -U postgres -d InvoicesSystem -f Scripts/SeedDatabase.sql
```

### Datos que se insertan

- **1 País** (Colombia)
- **3 Departamentos** (Antioquia, Cundinamarca, Valle del Cauca)
- **3 Ciudades** (Medellín, Bogotá, Cali)
- **6 Direcciones**
- **1 Emisor** (Conexus IT SAS)
- **4 Clientes** (2 naturales, 2 jurídicos)
- **8 Contactos de clientes**
- **5 Productos/Servicios**
- **4 Facturas** (3 finalizadas, 1 en borrador)
- **7 Detalles de facturas**
- **3 Pagos de facturas**

### Credenciales de prueba

El script NO crea usuarios. Para crear un usuario de prueba, usa el endpoint de registro:

```bash
POST /api/Auth/register
{
  "email": "admin@test.com",
  "password": "Admin123!",
  "firstName": "Admin",
  "lastName": "Test",
  "role": "Admin"
}
```

O usa el endpoint de login con credenciales existentes.

### Notas

- El script usa `ON CONFLICT DO NOTHING` para evitar errores si los datos ya existen
- Puedes ejecutar el script múltiples veces de forma segura
- Para limpiar todos los datos, descomenta las líneas TRUNCATE al inicio del script

