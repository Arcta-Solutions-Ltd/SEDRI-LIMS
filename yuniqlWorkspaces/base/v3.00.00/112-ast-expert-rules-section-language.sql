-- FrontEnd tags for the AST Expert Rules section heading (@AstExpRul@), the inline trigger icon
-- accessibility label (@AstExpTrg@) and the organism-name guidance icon label (@AstOrgGui@) — must exist
-- in the Language pack when using DB-backed translations (translationid 669 = default English per
-- DefaultLanguageEnglish.sql). Run as a new migration; does not amend existing scripts.
UPDATE language
SET pack = COALESCE(pack, '[]'::jsonb) || '[
    {"Key": "@AstExpRul@", "Value": "Expert Rules"},
    {"Key": "@AstExpTrg@", "Value": "Expert rules triggered"},
    {"Key": "@AstOrgGui@", "Value": "Guidance expert rules for this organism"}
]'::jsonb,
    lastmodifieddate = now()
WHERE translationid = 669
  AND NOT EXISTS (
    SELECT 1
    FROM jsonb_array_elements(COALESCE(pack, '[]'::jsonb)) AS elem
    WHERE elem->>'Key' = '@AstExpRul@'
  );
