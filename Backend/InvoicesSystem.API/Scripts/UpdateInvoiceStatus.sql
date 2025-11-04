
UPDATE invoice
SET
    status = 1, -- Final
    updated_at = NOW()
WHERE
    status = 0;
-- Draft (Borrador)

-- Verificar el resultado
SELECT
    status,
    CASE
        WHEN status = 0 THEN 'Borrador'
        WHEN status = 1 THEN 'Finalizada'
        WHEN status = 2 THEN 'Cancelada'
        ELSE 'Desconocido'
    END as estado_nombre,
    COUNT(*) as cantidad
FROM invoice
GROUP BY
    status
ORDER BY status;