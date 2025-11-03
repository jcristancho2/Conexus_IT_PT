
-- 1. País, Departamento, Ciudad
INSERT INTO
    country (id_country, country_name)
VALUES (1, 'Colombia') ON CONFLICT (id_country) DO NOTHING;

INSERT INTO
    department (
        id_department,
        department_name,
        id_country
    )
VALUES (1, 'Antioquia', 1),
    (2, 'Cundinamarca', 1),
    (3, 'Valle del Cauca', 1) ON CONFLICT (id_department) DO NOTHING;

INSERT INTO
    city (
        id_city,
        city_name,
        id_department
    )
VALUES (1, 'Medellín', 1),
    (2, 'Bogotá', 2),
    (3, 'Cali', 3) ON CONFLICT (id_city) DO NOTHING;

-- 2. Direcciones
INSERT INTO
    address (
        id_address,
        full_address,
        id_city
    )
VALUES (
        1,
        'Calle 50 # 46-30, Oficina 301',
        1
    ),
    (
        2,
        'Carrera 15 # 93-70, Piso 5',
        2
    ),
    (
        3,
        'Avenida 6N # 28-09, Torre Empresarial',
        3
    ),
    (4, 'Calle 123 # 45-67', 1),
    (5, 'Carrera 7 # 32-98', 2),
    (6, 'Avenida 3N # 12-45', 3),
    (
        7,
        'Calle 72 # 10-45, Edificio Plaza',
        2
    ),
    (
        8,
        'Avenida El Poblado # 15-20',
        1
    ),
    (9, 'Carrera 30 # 5-78', 3),
    (10, 'Calle 100 # 50-30', 2),
    (
        11,
        'Avenida Circunvalar # 45-67',
        1
    ),
    (12, 'Carrera 43A # 1-50', 1),
    (13, 'Calle 33 # 65-89', 2),
    (14, 'Avenida 7N # 45-12', 3),
    (15, 'Carrera 9 # 75-41', 2),
    (16, 'Calle 80 # 20-30', 2),
    (
        17,
        'Avenida Oriental # 48-90',
        1
    ),
    (18, 'Carrera 50 # 10-25', 3),
    (19, 'Calle 19 # 52-15', 2),
    (20, 'Avenida 6A # 26-50', 3) ON CONFLICT (id_address) DO NOTHING;

-- 3. Tipos de Identificación
INSERT INTO
    type_identification (
        id_type_identification,
        description
    )
VALUES (1, 'CC'),
    (2, 'NIT'),
    (3, 'CE') ON CONFLICT (id_type_identification) DO NOTHING;

-- 4. Régimen Tributario
INSERT INTO
    tax_regime (id_tax_regime, regime_name)
VALUES (1, 'Régimen Común'),
    (2, 'Régimen Simplificado'),
    (3, 'Gran Contribuyente') ON CONFLICT (id_tax_regime) DO NOTHING;

-- 5. Responsabilidad Tributaria
INSERT INTO
    tax_responsibility (
        id_tax_responsibility,
        responsibility_name
    )
VALUES (1, 'Responsable de IVA'),
    (2, 'No Responsable de IVA'),
    (3, 'Régimen Simple') ON CONFLICT (id_tax_responsibility) DO NOTHING;

-- 6. Métodos de Pago
INSERT INTO
    payment_method (
        id_payment_method,
        method_name
    )
VALUES (1, 'Efectivo'),
    (2, 'Transferencia Bancaria'),
    (3, 'Tarjeta de Crédito'),
    (4, 'Tarjeta de Débito'),
    (5, 'Cheque') ON CONFLICT (id_payment_method) DO NOTHING;

-- 7. Emisor
INSERT INTO
    issuer (
        id_issuer,
        id_address,
        id_tax_regime,
        id_tax_responsibility,
        identification_number,
        business_name,
        commercial_name,
        email,
        phone,
        website,
        created_at
    )
VALUES (
        1,
        1,
        1,
        1,
        '900123456-1',
        'Conexus IT SAS',
        'Conexus IT',
        'info@conexusit.com',
        '6041234567',
        'https://conexusit.com',
        NOW()
    ) ON CONFLICT (id_issuer) DO NOTHING;

-- 8. Clientes (10+ clientes)
INSERT INTO
    customer (
        id_customer,
        id_address,
        id_type_identification,
        identification_number,
        person_type,
        first_name,
        last_name,
        business_name,
        commercial_name,
        id_tax_regime,
        id_tax_responsibility,
        created_at
    )
VALUES (
        1,
        4,
        1,
        '1012345678',
        0,
        'Juan',
        'Pérez',
        NULL,
        NULL,
        1,
        1,
        NOW()
    ),
    (
        2,
        5,
        1,
        '1023456789',
        0,
        'María',
        'González',
        NULL,
        NULL,
        1,
        1,
        NOW()
    ),
    (
        3,
        6,
        2,
        '800123456-1',
        1,
        NULL,
        NULL,
        'Empresa ABC SAS',
        'ABC SAS',
        1,
        1,
        NOW()
    ),
    (
        4,
        4,
        1,
        '1034567890',
        0,
        'Carlos',
        'Rodríguez',
        NULL,
        NULL,
        2,
        2,
        NOW()
    ),
    (
        5,
        7,
        1,
        '1045678901',
        0,
        'Ana',
        'Martínez',
        NULL,
        NULL,
        1,
        1,
        NOW()
    ),
    (
        6,
        8,
        2,
        '800234567-2',
        1,
        NULL,
        NULL,
        'Tech Solutions SAS',
        'TechSol',
        1,
        1,
        NOW()
    ),
    (
        7,
        9,
        1,
        '1056789012',
        0,
        'Luis',
        'Fernández',
        NULL,
        NULL,
        1,
        1,
        NOW()
    ),
    (
        8,
        10,
        2,
        '800345678-3',
        1,
        NULL,
        NULL,
        'Comercial XYZ Ltda',
        'Comercial XYZ',
        2,
        1,
        NOW()
    ),
    (
        9,
        11,
        1,
        '1067890123',
        0,
        'Laura',
        'Sánchez',
        NULL,
        NULL,
        1,
        1,
        NOW()
    ),
    (
        10,
        12,
        2,
        '800456789-4',
        1,
        NULL,
        NULL,
        'Servicios Digitales SAS',
        'DigitalServ',
        1,
        1,
        NOW()
    ),
    (
        11,
        13,
        1,
        '1078901234',
        0,
        'Pedro',
        'López',
        NULL,
        NULL,
        2,
        2,
        NOW()
    ),
    (
        12,
        14,
        1,
        '1089012345',
        0,
        'Sofía',
        'Torres',
        NULL,
        NULL,
        1,
        1,
        NOW()
    ),
    (
        13,
        15,
        2,
        '800567890-5',
        1,
        NULL,
        NULL,
        'Importaciones Global SA',
        'ImportGlobal',
        1,
        1,
        NOW()
    ),
    (
        14,
        16,
        1,
        '1090123456',
        0,
        'Diego',
        'Ramírez',
        NULL,
        NULL,
        1,
        1,
        NOW()
    ),
    (
        15,
        17,
        2,
        '800678901-6',
        1,
        NULL,
        NULL,
        'Consultoría Empresarial SAS',
        'ConsultEmp',
        1,
        1,
        NOW()
    ) ON CONFLICT (id_customer) DO NOTHING;

-- 9. Contactos de Clientes
INSERT INTO
    customer_contact (
        id_customer,
        contact_type,
        contact_value
    )
VALUES (1, 0, 'juan.perez@email.com'),
    (1, 1, '3001234567'),
    (
        2,
        0,
        'maria.gonzalez@email.com'
    ),
    (2, 1, '3002345678'),
    (
        3,
        0,
        'contacto@empresaabc.com'
    ),
    (3, 1, '6049876543'),
    (
        4,
        0,
        'carlos.rodriguez@email.com'
    ),
    (4, 1, '3003456789'),
    (
        5,
        0,
        'ana.martinez@email.com'
    ),
    (5, 1, '3004567890'),
    (6, 0, 'info@techsol.com'),
    (6, 1, '6041234567'),
    (
        7,
        0,
        'luis.fernandez@email.com'
    ),
    (7, 1, '3005678901'),
    (
        8,
        0,
        'ventas@comercialxyz.com'
    ),
    (8, 1, '6042345678'),
    (
        9,
        0,
        'laura.sanchez@email.com'
    ),
    (9, 1, '3006789012'),
    (
        10,
        0,
        'contacto@digitalserv.com'
    ),
    (10, 1, '6043456789'),
    (
        11,
        0,
        'pedro.lopez@email.com'
    ),
    (11, 1, '3007890123'),
    (
        12,
        0,
        'sofia.torres@email.com'
    ),
    (12, 1, '3008901234'),
    (
        13,
        0,
        'info@importglobal.com'
    ),
    (13, 1, '6044567890'),
    (
        14,
        0,
        'diego.ramirez@email.com'
    ),
    (14, 1, '3009012345'),
    (
        15,
        0,
        'contacto@consultemp.com'
    ),
    (15, 1, '6045678901') ON CONFLICT DO NOTHING;

-- 10. Productos (15+ productos)
INSERT INTO
    product (
        id_product,
        code_product,
        product_name,
        description,
        unit_price,
        unit_measure,
        is_active,
        created_at
    )
VALUES (
        1,
        'P-001',
        'Laptop Dell XPS 15',
        'Laptop Dell XPS 15, 16GB RAM, 512GB SSD',
        3500000.00,
        'UND',
        true,
        NOW()
    ),
    (
        2,
        'P-002',
        'Mouse Logitech MX Master',
        'Mouse inalámbrico Logitech MX Master 3',
        350000.00,
        'UND',
        true,
        NOW()
    ),
    (
        3,
        'P-003',
        'Teclado Mecánico RGB',
        'Teclado mecánico con retroiluminación RGB',
        250000.00,
        'UND',
        true,
        NOW()
    ),
    (
        4,
        'P-004',
        'Monitor LG 27 pulgadas',
        'Monitor LG UltraWide 27 pulgadas 4K',
        1200000.00,
        'UND',
        true,
        NOW()
    ),
    (
        5,
        'P-005',
        'Servicio de Desarrollo Web',
        'Desarrollo de sitio web completo',
        5000000.00,
        'HRS',
        true,
        NOW()
    ),
    (
        6,
        'P-006',
        'Servicio de Consultoría IT',
        'Consultoría en infraestructura IT',
        350000.00,
        'HRS',
        true,
        NOW()
    ),
    (
        7,
        'P-007',
        'Tablet Samsung Galaxy',
        'Tablet Samsung Galaxy Tab S8',
        2800000.00,
        'UND',
        true,
        NOW()
    ),
    (
        8,
        'P-008',
        'Auriculares Sony WH-1000XM4',
        'Auriculares inalámbricos con cancelación de ruido',
        1800000.00,
        'UND',
        true,
        NOW()
    ),
    (
        9,
        'P-009',
        'Impresora HP LaserJet',
        'Impresora láser HP LaserJet Pro',
        1500000.00,
        'UND',
        true,
        NOW()
    ),
    (
        10,
        'P-010',
        'Servicio de Soporte Técnico',
        'Soporte técnico remoto mensual',
        800000.00,
        'HRS',
        true,
        NOW()
    ),
    (
        11,
        'P-011',
        'Webcam Logitech C920',
        'Webcam HD 1080p Logitech',
        450000.00,
        'UND',
        true,
        NOW()
    ),
    (
        12,
        'P-012',
        'Disco Duro Externo 2TB',
        'Disco duro externo Seagate 2TB',
        350000.00,
        'UND',
        true,
        NOW()
    ),
    (
        13,
        'P-013',
        'Servicio de Mantenimiento',
        'Mantenimiento preventivo de equipos',
        200000.00,
        'HRS',
        true,
        NOW()
    ),
    (
        14,
        'P-014',
        'Router WiFi 6',
        'Router WiFi 6 ASUS AX3000',
        800000.00,
        'UND',
        true,
        NOW()
    ),
    (
        15,
        'P-015',
        'Servicio de Cloud Migration',
        'Migración de servicios a la nube',
        8000000.00,
        'HRS',
        true,
        NOW()
    ),
    (
        16,
        'P-016',
        'Licencia Microsoft Office 365',
        'Licencia anual Office 365 Business',
        600000.00,
        'UND',
        true,
        NOW()
    ),
    (
        17,
        'P-017',
        'Backup en la Nube',
        'Servicio de backup mensual 500GB',
        150000.00,
        'HRS',
        true,
        NOW()
    ),
    (
        18,
        'P-018',
        'Switch de Red 24 Puertos',
        'Switch gestionable 24 puertos',
        1200000.00,
        'UND',
        true,
        NOW()
    ) ON CONFLICT (id_product) DO NOTHING;

-- 11. Facturas (20+ facturas)
INSERT INTO
    invoice (
        id_invoice,
        id_customer,
        id_issuer,
        invoice_number,
        invoice_date,
        due_date,
        status,
        subtotal,
        total_tax,
        total,
        notes,
        created_at
    )
VALUES (
        1,
        1,
        1,
        'INV-2025-00001',
        NOW() - INTERVAL '30 days',
        NOW() - INTERVAL '10 days',
        1,
        3500000.00,
        665000.00,
        4165000.00,
        'Laptop Dell XPS 15',
        NOW() - INTERVAL '30 days'
    ),
    (
        2,
        2,
        1,
        'INV-2025-00002',
        NOW() - INTERVAL '25 days',
        NOW() - INTERVAL '5 days',
        1,
        350000.00,
        66500.00,
        416500.00,
        'Mouse Logitech MX Master',
        NOW() - INTERVAL '25 days'
    ),
    (
        3,
        3,
        1,
        'INV-2025-00003',
        NOW() - INTERVAL '20 days',
        NOW() + INTERVAL '10 days',
        1,
        5000000.00,
        950000.00,
        5950000.00,
        'Servicio de Desarrollo Web',
        NOW() - INTERVAL '20 days'
    ),
    (
        4,
        4,
        1,
        'INV-2025-00004',
        NOW() - INTERVAL '18 days',
        NOW() + INTERVAL '12 days',
        1,
        250000.00,
        47500.00,
        297500.00,
        'Teclado Mecánico RGB',
        NOW() - INTERVAL '18 days'
    ),
    (
        5,
        5,
        1,
        'INV-2025-00005',
        NOW() - INTERVAL '15 days',
        NOW() + INTERVAL '15 days',
        1,
        1200000.00,
        228000.00,
        1428000.00,
        'Monitor LG 27 pulgadas',
        NOW() - INTERVAL '15 days'
    ),
    (
        6,
        6,
        1,
        'INV-2025-00006',
        NOW() - INTERVAL '12 days',
        NOW() + INTERVAL '18 days',
        1,
        700000.00,
        133000.00,
        833000.00,
        'Servicio de Consultoría IT',
        NOW() - INTERVAL '12 days'
    ),
    (
        7,
        7,
        1,
        'INV-2025-00007',
        NOW() - INTERVAL '10 days',
        NOW() + INTERVAL '20 days',
        1,
        2800000.00,
        532000.00,
        3332000.00,
        'Tablet Samsung Galaxy',
        NOW() - INTERVAL '10 days'
    ),
    (
        8,
        8,
        1,
        'INV-2025-00008',
        NOW() - INTERVAL '8 days',
        NOW() + INTERVAL '22 days',
        1,
        1800000.00,
        342000.00,
        2142000.00,
        'Auriculares Sony WH-1000XM4',
        NOW() - INTERVAL '8 days'
    ),
    (
        9,
        9,
        1,
        'INV-2025-00009',
        NOW() - INTERVAL '6 days',
        NOW() + INTERVAL '24 days',
        1,
        1500000.00,
        285000.00,
        1785000.00,
        'Impresora HP LaserJet',
        NOW() - INTERVAL '6 days'
    ),
    (
        10,
        10,
        1,
        'INV-2025-00010',
        NOW() - INTERVAL '5 days',
        NOW() + INTERVAL '25 days',
        1,
        800000.00,
        152000.00,
        952000.00,
        'Servicio de Soporte Técnico',
        NOW() - INTERVAL '5 days'
    ),
    (
        11,
        11,
        1,
        'INV-2025-00011',
        NOW() - INTERVAL '4 days',
        NOW() + INTERVAL '26 days',
        1,
        450000.00,
        85500.00,
        535500.00,
        'Webcam Logitech C920',
        NOW() - INTERVAL '4 days'
    ),
    (
        12,
        12,
        1,
        'INV-2025-00012',
        NOW() - INTERVAL '3 days',
        NOW() + INTERVAL '27 days',
        1,
        350000.00,
        66500.00,
        416500.00,
        'Disco Duro Externo 2TB',
        NOW() - INTERVAL '3 days'
    ),
    (
        13,
        13,
        1,
        'INV-2025-00013',
        NOW() - INTERVAL '2 days',
        NOW() + INTERVAL '28 days',
        1,
        16000000.00,
        3040000.00,
        19040000.00,
        'Servicio de Cloud Migration',
        NOW() - INTERVAL '2 days'
    ),
    (
        14,
        14,
        1,
        'INV-2025-00014',
        NOW() - INTERVAL '1 day',
        NOW() + INTERVAL '29 days',
        1,
        800000.00,
        152000.00,
        952000.00,
        'Router WiFi 6',
        NOW() - INTERVAL '1 day'
    ),
    (
        15,
        15,
        1,
        'INV-2025-00015',
        NOW(),
        NOW() + INTERVAL '30 days',
        1,
        600000.00,
        114000.00,
        714000.00,
        'Licencia Microsoft Office 365',
        NOW()
    ),
    (
        16,
        1,
        1,
        'INV-2025-00016',
        NOW() - INTERVAL '28 days',
        NOW() - INTERVAL '8 days',
        1,
        1200000.00,
        228000.00,
        1428000.00,
        'Switch de Red 24 Puertos',
        NOW() - INTERVAL '28 days'
    ),
    (
        17,
        2,
        1,
        'INV-2025-00017',
        NOW() - INTERVAL '22 days',
        NOW() - INTERVAL '2 days',
        1,
        300000.00,
        57000.00,
        357000.00,
        'Servicio de Mantenimiento',
        NOW() - INTERVAL '22 days'
    ),
    (
        18,
        3,
        1,
        'INV-2025-00018',
        NOW() - INTERVAL '14 days',
        NOW() + INTERVAL '16 days',
        0,
        5000000.00,
        950000.00,
        5950000.00,
        'Factura en borrador',
        NOW() - INTERVAL '14 days'
    ),
    (
        19,
        4,
        1,
        'INV-2025-00019',
        NOW() - INTERVAL '7 days',
        NOW() + INTERVAL '23 days',
        1,
        150000.00,
        28500.00,
        178500.00,
        'Backup en la Nube',
        NOW() - INTERVAL '7 days'
    ),
    (
        20,
        5,
        1,
        'INV-2025-00020',
        NOW() - INTERVAL '9 days',
        NOW() + INTERVAL '21 days',
        1,
        1050000.00,
        199500.00,
        1249500.00,
        'Laptop + Mouse + Teclado',
        NOW() - INTERVAL '9 days'
    ),
    (
        21,
        6,
        1,
        'INV-2025-00021',
        NOW() - INTERVAL '11 days',
        NOW() + INTERVAL '19 days',
        1,
        2400000.00,
        456000.00,
        2856000.00,
        'Monitor + Tablet',
        NOW() - INTERVAL '11 days'
    ),
    (
        22,
        7,
        1,
        'INV-2025-00022',
        NOW() - INTERVAL '13 days',
        NOW() + INTERVAL '17 days',
        1,
        900000.00,
        171000.00,
        1071000.00,
        'Router + Impresora',
        NOW() - INTERVAL '13 days'
    ),
    (
        23,
        8,
        1,
        'INV-2025-00023',
        NOW() - INTERVAL '16 days',
        NOW() + INTERVAL '14 days',
        1,
        2250000.00,
        427500.00,
        2677500.00,
        'Auriculares + Webcam',
        NOW() - INTERVAL '16 days'
    ),
    (
        24,
        9,
        1,
        'INV-2025-00024',
        NOW() - INTERVAL '19 days',
        NOW() + INTERVAL '11 days',
        1,
        700000.00,
        133000.00,
        833000.00,
        'Disco Duro + Licencia Office',
        NOW() - INTERVAL '19 days'
    ),
    (
        25,
        10,
        1,
        'INV-2025-00025',
        NOW() - INTERVAL '21 days',
        NOW() + INTERVAL '9 days',
        1,
        950000.00,
        180500.00,
        1130500.00,
        'Servicio Soporte + Mantenimiento',
        NOW() - INTERVAL '21 days'
    ) ON CONFLICT (id_invoice) DO NOTHING;

-- 12. Detalles de Facturas (ampliado para 25+ facturas)
INSERT INTO
    invoice_detail (
        id_invoice,
        id_product,
        quantity,
        unit_price,
        discount,
        subtotal,
        description,
        created_at
    )
VALUES (
        1,
        1,
        1,
        3500000.00,
        0,
        3500000.00,
        NULL,
        NOW() - INTERVAL '30 days'
    ),
    (
        2,
        2,
        1,
        350000.00,
        0,
        350000.00,
        NULL,
        NOW() - INTERVAL '25 days'
    ),
    (
        3,
        5,
        1,
        5000000.00,
        0,
        5000000.00,
        NULL,
        NOW() - INTERVAL '20 days'
    ),
    (
        4,
        3,
        1,
        250000.00,
        0,
        250000.00,
        NULL,
        NOW() - INTERVAL '18 days'
    ),
    (
        5,
        4,
        1,
        1200000.00,
        0,
        1200000.00,
        NULL,
        NOW() - INTERVAL '15 days'
    ),
    (
        6,
        6,
        2,
        350000.00,
        0,
        700000.00,
        NULL,
        NOW() - INTERVAL '12 days'
    ),
    (
        7,
        7,
        1,
        2800000.00,
        0,
        2800000.00,
        NULL,
        NOW() - INTERVAL '10 days'
    ),
    (
        8,
        8,
        1,
        1800000.00,
        0,
        1800000.00,
        NULL,
        NOW() - INTERVAL '8 days'
    ),
    (
        9,
        9,
        1,
        1500000.00,
        0,
        1500000.00,
        NULL,
        NOW() - INTERVAL '6 days'
    ),
    (
        10,
        10,
        1,
        800000.00,
        0,
        800000.00,
        NULL,
        NOW() - INTERVAL '5 days'
    ),
    (
        11,
        11,
        1,
        450000.00,
        0,
        450000.00,
        NULL,
        NOW() - INTERVAL '4 days'
    ),
    (
        12,
        12,
        1,
        350000.00,
        0,
        350000.00,
        NULL,
        NOW() - INTERVAL '3 days'
    ),
    (
        13,
        15,
        2,
        8000000.00,
        0,
        16000000.00,
        NULL,
        NOW() - INTERVAL '2 days'
    ),
    (
        14,
        14,
        1,
        800000.00,
        0,
        800000.00,
        NULL,
        NOW() - INTERVAL '1 day'
    ),
    (
        15,
        16,
        1,
        600000.00,
        0,
        600000.00,
        NULL,
        NOW()
    ),
    (
        16,
        18,
        1,
        1200000.00,
        0,
        1200000.00,
        NULL,
        NOW() - INTERVAL '28 days'
    ),
    (
        17,
        13,
        1,
        200000.00,
        0,
        200000.00,
        NULL,
        NOW() - INTERVAL '22 days'
    ),
    (
        17,
        13,
        1,
        200000.00,
        100000.00,
        100000.00,
        'Descuento por volumen',
        NOW() - INTERVAL '22 days'
    ),
    (
        18,
        5,
        1,
        5000000.00,
        0,
        5000000.00,
        NULL,
        NOW() - INTERVAL '14 days'
    ),
    (
        19,
        17,
        1,
        150000.00,
        0,
        150000.00,
        NULL,
        NOW() - INTERVAL '7 days'
    ),
    (
        20,
        1,
        1,
        3500000.00,
        0,
        3500000.00,
        NULL,
        NOW() - INTERVAL '9 days'
    ),
    (
        20,
        2,
        1,
        350000.00,
        0,
        350000.00,
        NULL,
        NOW() - INTERVAL '9 days'
    ),
    (
        20,
        3,
        1,
        250000.00,
        0,
        250000.00,
        NULL,
        NOW() - INTERVAL '9 days'
    ),
    (
        20,
        3,
        1,
        250000.00,
        0,
        250000.00,
        NULL,
        NOW() - INTERVAL '9 days'
    ),
    (
        21,
        4,
        1,
        1200000.00,
        0,
        1200000.00,
        NULL,
        NOW() - INTERVAL '11 days'
    ),
    (
        21,
        7,
        1,
        2800000.00,
        0,
        2800000.00,
        NULL,
        NOW() - INTERVAL '11 days'
    ),
    (
        22,
        14,
        1,
        800000.00,
        0,
        800000.00,
        NULL,
        NOW() - INTERVAL '13 days'
    ),
    (
        22,
        9,
        1,
        1500000.00,
        0,
        1500000.00,
        NULL,
        NOW() - INTERVAL '13 days'
    ),
    (
        23,
        8,
        1,
        1800000.00,
        0,
        1800000.00,
        NULL,
        NOW() - INTERVAL '16 days'
    ),
    (
        23,
        11,
        1,
        450000.00,
        0,
        450000.00,
        NULL,
        NOW() - INTERVAL '16 days'
    ),
    (
        24,
        12,
        1,
        350000.00,
        0,
        350000.00,
        NULL,
        NOW() - INTERVAL '19 days'
    ),
    (
        24,
        16,
        1,
        600000.00,
        0,
        600000.00,
        NULL,
        NOW() - INTERVAL '19 days'
    ),
    (
        24,
        16,
        1,
        600000.00,
        250000.00,
        350000.00,
        'Descuento por paquete',
        NOW() - INTERVAL '19 days'
    ),
    (
        25,
        10,
        1,
        800000.00,
        0,
        800000.00,
        NULL,
        NOW() - INTERVAL '21 days'
    ),
    (
        25,
        13,
        1,
        200000.00,
        0,
        200000.00,
        NULL,
        NOW() - INTERVAL '21 days'
    ),
    (
        25,
        13,
        1,
        200000.00,
        250000.00,
        0,
        'Descuento especial',
        NOW() - INTERVAL '21 days'
    ) ON CONFLICT DO NOTHING;

-- 13. Usuarios de prueba
INSERT INTO
    users (
        email,
        password_hash,
        first_name,
        last_name,
        role,
        is_active,
        created_at
    )
VALUES (
        'admin@test.com',
        'Admin123!',
        'Admin',
        'Test',
        'Admin',
        true,
        NOW()
    ) ON CONFLICT (email) DO NOTHING;

-- 14. Pagos de Facturas
INSERT INTO
    invoice_payment (
        id_invoice,
        id_payment_method,
        amount,
        payment_date,
        created_at
    )
VALUES (
        1,
        2,
        59500.00,
        NOW() - INTERVAL '8 days',
        NOW() - INTERVAL '8 days'
    ),
    (
        2,
        3,
        154700.00,
        NOW() - INTERVAL '3 days',
        NOW() - INTERVAL '3 days'
    ),
    (
        4,
        2,
        200000.00,
        NOW() - INTERVAL '1 day',
        NOW() - INTERVAL '1 day'
    ) ON CONFLICT DO NOTHING;

-- Verificar datos insertados
SELECT 'Paises' as tabla, COUNT(*) as total
FROM country
UNION ALL
SELECT 'Departamentos', COUNT(*)
FROM department
UNION ALL
SELECT 'Ciudades', COUNT(*)
FROM city
UNION ALL
SELECT 'Direcciones', COUNT(*)
FROM address
UNION ALL
SELECT 'Clientes', COUNT(*)
FROM customer
UNION ALL
SELECT 'Productos', COUNT(*)
FROM product
UNION ALL
SELECT 'Facturas', COUNT(*)
FROM invoice
UNION ALL
SELECT 'Detalles de Facturas', COUNT(*)
FROM invoice_detail
UNION ALL
SELECT 'Pagos de Facturas', COUNT(*)
FROM invoice_payment
UNION ALL
SELECT 'Usuarios', COUNT(*)
FROM users;