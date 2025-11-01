# PRUEBA TECNICA CONEXUS-IT

![Lenguaje](https://img.shields.io/badge/Lenguaje-ASP.NET%20Core%209.0+-512BD4?style=for-the-badge&logo=dotnet)
![Arquitectura](https://img.shields.io/badge/Arquitectura-MVC%20-%23007ACC?style=for-the-badge)
![Base de Datos](https://img.shields.io/badge/Base%20de%20Datos-Postgres%20/%20EF%20Core-4479A1?style=for-the-badge&logo=postgres)
![Estado](https://img.shields.io/badge/Estado-En%20Desarrollo-%2328A745?style=for-the-badge)
![Docker](https://img.shields.io/badge/Docker-Disponible-%232496ED?style=for-the-badge&logo=docker)



## 1. Prueba de conocimiento

-   **A ¿Cuál es el objetivo o funcionalidad del leguaje XML?**
    ``` bash
    Extensible Markup Language (XML)
    
    Objetivo: permite de forma jerarquica y flexible presentar datos 
    estucturados y facilita el intercambio de informacion en la web 
    independiente del lenguaje de programacion o sistema operativo, 
    deribado de SGML y es comlementario de HTML

    Funcionalidad: 
    - etiquetas personalizadas
    - estructura jerarquica
    - intercambio de datos entre aplicaciones
    ```
-   **B ¿Cuál es la diferencia entre un servicio Api/REST y uno WCF??**
    ``` bash

    la principal diferencia es WCF se utiliza para entornos microsoft 
    complejos, es mas antiguo y pesado. Se utiliza en entornos 
    empresariales Internos; REST es universal, moderno y lijero. 
    Compatible con cualquier dispositivo.
    ```

-   **C ¿Para qué casos sería recomendable usar una vista y no una tabla de la base de datos?**
    ``` bash 
    - simplifica consultas complejas 
    - permite otorgar permisos de acceso a informacion sensible
    - presentacion de datos de forma mas coherente, extrayendo la informacion necesaria
    - reutilizacion de logica para no repetir consultas complejas
    - optimizacion donde se reduce el tiempo de consultas repetidas
    - analisis o informes 
    ```
-   **D ¿Cuál es el Objetivo o funcionalidad de una petición Json?**
    ``` bash
    intercambio bidireccional y seguro de datos entre servidores

    - no envia ni recibe coockies ni autenticacion HTTP
    -solo trabaja con JSON 
    -especifica errores y retardos
    ```

## 2. Script de base de datos

``` SQL
CREATE TABLE IF NOT EXISTS country(
id_country SERIAL PRIMARY KEY,
cod_country CHAR(2) NOT NUL UNIQUE,
name_country VARCHAR(100) NOT NULL,
),
CREATE TABLE IF NOT EXISTS departament(
id_departament SERIAL PRIMARY KEY,
name_departament VARCHAR(100) NOT NULL,
id_pais INT NOT NULL REFERENCES country(id_country)
),
CREATE TABLE IF NOT EXISTS city(
id_city
name_city
id_departament INT NOT NULL REFERENCES departament(id_departament)
),


CREATE TABLE IF NOT EXISTS payment_method(

),

CREATE TABLE IF NOT EXISTS invoice_payments(

),
CREATE TABLE IF NOT EXISTS customer(

),

CREATE TABLE IF NOT EXISTS customer_contact(

),
CREATE TABLE IF NOT EXISTS issuer(

),
CREATE TABLE IF NOT EXISTS product(

),
CREATE TABLE IF NOT EXISTS taxes(

),
CREATE TABLE IF NOT EXISTS product_taxes(

),
CREATE TABLE IF NOT EXISTS invoice(

),
CREATE TABLE IF NOT EXISTS invoice_detail(

),

```


## Autor
[Jorge Andres Cristancho Olarte](https://github.com/jcristancho2)

