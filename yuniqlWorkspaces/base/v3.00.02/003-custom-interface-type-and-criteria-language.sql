-- Rename the "Direct Test" interface type (InstrumentEvent list id 10, list item id 10) to "Custom".
-- The list item id is preserved so existing interface profiles keep working and direct-test processor
-- routing is unchanged. Matching everywhere is by id, so only the display value changes here.
UPDATE listitem
SET value = 'Custom', lastmodifieddate = now()
WHERE id = 10 AND listid = 10;

-- FrontEnd/validation tags for the Custom interface type export profile picker, interface criteria grid,
-- interface combination rule and the "export profile required" validation message. Must exist in the
-- Language pack when using DB-backed translations (translationid 669 = default English per
-- DefaultLanguageEnglish.sql). Run as a new migration; does not amend existing scripts.
UPDATE language
SET pack = COALESCE(pack, '[]'::jsonb) || '[
    {"Key": "@InsCusExp@", "Value": "Export Profile"},
    {"Key": "@InsCriG@", "Value": "Interface Criteria (optional)"},
    {"Key": "@InsCriR@", "Value": "Interface combination rule"},
    {"Key": "@InsValExp@", "Value": "Export profile must be selected for Custom interface type"}
]'::jsonb,
    lastmodifieddate = now()
WHERE translationid = 669
  AND NOT EXISTS (
    SELECT 1
    FROM jsonb_array_elements(COALESCE(pack, '[]'::jsonb)) AS elem
    WHERE elem->>'Key' = '@InsCusExp@'
  );
