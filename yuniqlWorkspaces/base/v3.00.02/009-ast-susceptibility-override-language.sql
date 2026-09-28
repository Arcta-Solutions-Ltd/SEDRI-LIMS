-- Language keys for AST manual susceptibility override audit UI.
-- Run as a new migration; does not amend existing scripts.
UPDATE language
SET pack = COALESCE(pack, '[]'::jsonb) || '[
    {"Key": "@LabSusAud@", "Value": "Record susceptibility change audit"},
    {"Key": "@AstSusPan@", "Value": "Susceptibility override reason"},
    {"Key": "@AstSusPanDesc@", "Value": "Enter the reason for changing susceptibility from the calculated result."},
    {"Key": "@AstSusCan@", "Value": "Canned reason"},
    {"Key": "@AstSusFree@", "Value": "Free text reason"},
    {"Key": "@AstSusRev@", "Value": "Revert to calculated"},
    {"Key": "@AstSusRevConf@", "Value": "This will restore the calculated susceptibility and remove the override reason."},
    {"Key": "@AstSusOvrReq@", "Value": "A reason is required when manually setting susceptibility"},
    {"Key": "@AstSusMan@", "Value": "Manually set susceptibility"},
    {"Key": "@AstSusSetBy@", "Value": "Set by"},
    {"Key": "@AstSusSetAt@", "Value": "Set at"},
    {"Key": "@AstSusOverFrom@", "Value": "Overridden from"},
    {"Key": "@AstSusNew@", "Value": "Susceptibility"}
]'::jsonb,
    lastmodifieddate = now()
WHERE translationid = 669
  AND NOT EXISTS (
    SELECT 1
    FROM jsonb_array_elements(COALESCE(pack, '[]'::jsonb)) AS elem
    WHERE elem->>'Key' = '@AstSusPan@'
  );
