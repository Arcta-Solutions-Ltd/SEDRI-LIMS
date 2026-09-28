-- AST MIC measurement validation language key (@AstMicMea@) for DB-backed English pack (translationid 669).
UPDATE language
SET pack = COALESCE(pack, '[]'::jsonb) || '[
    {"Key": "@AstMicMea@", "Value": "MIC measurement must include a valid number"}
]'::jsonb,
    lastmodifieddate = now()
WHERE translationid = 669
  AND NOT EXISTS (
    SELECT 1
    FROM jsonb_array_elements(COALESCE(pack, '[]'::jsonb)) AS elem
    WHERE elem->>'Key' = '@AstMicMea@'
  );
