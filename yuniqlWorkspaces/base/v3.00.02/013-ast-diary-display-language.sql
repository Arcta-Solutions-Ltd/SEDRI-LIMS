-- Language keys for the AST diary entry contents.
--
-- These label the AST fields, grid column headings and nested blocks named by the updateast Display
-- configuration. Existing AST and general keys (@AstCom1@, @AstCom2@, @AstTesA@, @AstExpRul@, @GenAnt@,
-- @GenDos@, @GenGui@, @GenSus@, @GenCat@, @GenInc@, @GenTesA@, @AstSusCan@, @AstSusFree@, @AstSusSetBy@,
-- @AstSusSetAt@, @AstSusOverFrom@) are reused and are not repeated here.
--
-- Run as a new migration; does not amend existing scripts.
UPDATE language
SET pack = COALESCE(pack, '[]'::jsonb) || '[
    {"Key": "@AstDiaLin@", "Value": "AST Lines"},
    {"Key": "@AstDiaSpe@", "Value": "Special Considerations"},
    {"Key": "@AstDiaSpeT@", "Value": "Special Consideration"},
    {"Key": "@AstDiaOvr@", "Value": "Susceptibility Override"},
    {"Key": "@AstDiaAct@", "Value": "Expert Rule Actions"},
    {"Key": "@AstDiaRulN@", "Value": "Expert Rule"},
    {"Key": "@AstDiaRulT@", "Value": "Expert Rule Text"},
    {"Key": "@AstDiaApp@", "Value": "Rule Applied"},
    {"Key": "@AstDiaMea@", "Value": "Measurement"},
    {"Key": "@AstDiaBrk@", "Value": "Applied Breakpoint"},
    {"Key": "@AstDiaNot@", "Value": "Additional Notes"},
    {"Key": "@AstDiaDel@", "Value": "Remove Incomplete Rows"},
    {"Key": "@AstDiaRecD@", "Value": "Recorded Date"},
    {"Key": "@AstDiaRecT@", "Value": "Recorded Time"}
]'::jsonb,
    lastmodifieddate = now()
WHERE translationid = 669
  AND NOT EXISTS (
    SELECT 1
    FROM jsonb_array_elements(COALESCE(pack, '[]'::jsonb)) AS elem
    WHERE elem->>'Key' = '@AstDiaLin@'
  );
