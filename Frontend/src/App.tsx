import * as React from "react";
import Button from "@mui/material/Button";
import { Card, CardContent, Typography } from "@mui/material";

export default function App() {
  return (
    <div className="flex flex-col items-center justify-center min-h-screen bg-gray-100 gap-6">
      <h1 className="text-3xl font-bold text-blue-600">Tailwind + MUI + React</h1>

      <Card className="shadow-lg rounded-xl w-80">
        <CardContent>
          <Typography variant="h6" className="text-center mb-2">
            Componente de ejemplo
          </Typography>
          <Typography variant="body2" className="text-gray-600 text-center">
            Puedes combinar clases de Tailwind con componentes de MUI sin
            problema.
          </Typography>
        </CardContent>
      </Card>

      <Button variant="contained" color="primary">
        Botón MUI
      </Button>
      <button className="bg-blue-500 text-white px-4 py-2 rounded hover:bg-blue-600">
        Botón Tailwind
      </button>
    </div>
  );
}
