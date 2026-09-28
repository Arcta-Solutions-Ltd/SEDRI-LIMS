-- FrontEnd tags for Run Export hierarchy picker placeholders (@ExpSelOrg@, @ExpSelLoc@, @ExpFilOrg@, @ExpFilLoc@)
-- when using DB-backed translations (translationid 669 = default English per DefaultLanguageEnglish.sql).
UPDATE language
SET pack = COALESCE(pack, '[]'::jsonb) || '[
    {"Key": "@ExpSelOrg@", "Value": "Select client organisation(s)"},
    {"Key": "@ExpSelLoc@", "Value": "Select patient location(s)"},
    {"Key": "@ExpFilOrg@", "Value": "Filter organisations..."},
    {"Key": "@ExpFilLoc@", "Value": "Filter locations..."}
]'::jsonb,
    lastmodifieddate = now()
WHERE translationid = 669
  AND NOT EXISTS (
    SELECT 1
    FROM jsonb_array_elements(COALESCE(pack, '[]'::jsonb)) AS elem
    WHERE elem->>'Key' = '@ExpSelOrg@'
  );
