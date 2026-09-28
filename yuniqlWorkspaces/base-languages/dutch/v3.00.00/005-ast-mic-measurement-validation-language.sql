-- @AstMicMea@ — MIC measurement validation — Dutch (DB packs)
UPDATE language
SET pack = COALESCE(pack, '[]'::jsonb) || '[
    {"Key": "@AstMicMea@", "Value": "MIC-meting moet een geldig getal bevatten"}
]'::jsonb,
    lastmodifieddate = now()
WHERE translationid = 669
  AND NOT EXISTS (
    SELECT 1
    FROM jsonb_array_elements(COALESCE(pack, '[]'::jsonb)) AS elem
    WHERE elem->>'Key' = '@AstMicMea@'
  );
