# Logo de la Aplicación

## Ubicación del Logo

El logo de la aplicación está definido en el componente SVG ubicado en:

**`Frontend/src/components/Logo.tsx`**

## Cómo Cambiar el Logo

### Opción 1: Modificar el SVG existente

Puedes editar el archivo `Frontend/src/components/Logo.tsx` y modificar el SVG directamente:

```tsx
export default function Logo({ className = "w-8 h-8" }: { className?: string }) {
  return (
    <svg
      className={className}
      viewBox="0 0 100 100"
      fill="none"
      xmlns="http://www.w3.org/2000/svg"
    >
      {/* Modifica aquí el contenido del SVG */}
      <rect width="100" height="100" rx="20" className="fill-blue-600 dark:fill-blue-500" />
      {/* ... más elementos SVG ... */}
    </svg>
  );
}
```

### Opción 2: Usar una imagen externa

Puedes reemplazar el componente SVG con una imagen:

1. Coloca tu logo en `Frontend/public/logo.png` (o `.svg`, `.jpg`, etc.)
2. Modifica `Logo.tsx`:

```tsx
export default function Logo({ className = "w-8 h-8" }: { className?: string }) {
  return (
    <img 
      src="/logo.png" 
      alt="Conexus Billing" 
      className={className}
    />
  );
}
```

### Opción 3: Usar un SVG personalizado

1. Crea tu SVG en un archivo separado: `Frontend/public/logo.svg`
2. Modifica `Logo.tsx`:

```tsx
export default function Logo({ className = "w-8 h-8" }: { className?: string }) {
  return (
    <img 
      src="/logo.svg" 
      alt="Conexus Billing" 
      className={className}
    />
  );
}
```

## Dónde se Usa el Logo

El logo se utiliza en:
- **NavBar** (`Frontend/src/components/NavBar.tsx`) - Barra de navegación
- **LoginPage** (`Frontend/src/routes/auth/LoginPage.tsx`) - Página de inicio de sesión

## Iconos PWA

Para cambiar los iconos de la PWA (Progressive Web App), reemplaza los archivos en `Frontend/public/`:
- `icon-192.png` - Icono 192x192
- `icon-512.png` - Icono 512x512

Estos iconos se referencian en `Frontend/public/manifest.json`.

