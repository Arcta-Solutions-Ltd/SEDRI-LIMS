-- FrontEnd tags for AST Expert Finding (@AstExpFin@, @AstRmFam@, @AstRmPhe@) — must exist in Language pack
-- when using DB-backed translations (translationid 669 = default English per DefaultLanguageEnglish.sql).
UPDATE language
SET pack = COALESCE(pack, '[]'::jsonb) || '[
    {"Key": "@AstExpFin@", "Value": "Expert Finding"},
    {"Key": "@AstRmFam@", "Value": "Drug family"},
    {"Key": "@AstRmPhe@", "Value": "Phenotype"}
]'::jsonb,
    lastmodifieddate = now()
WHERE translationid = 669
  AND NOT EXISTS (
    SELECT 1
    FROM jsonb_array_elements(COALESCE(pack, '[]'::jsonb)) AS elem
    WHERE elem->>'Key' = '@AstExpFin@'
  );
