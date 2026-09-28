-- Column picker language keys for list-view column selection UI (DB-backed English pack, translationid 669).
UPDATE language
SET pack = COALESCE(pack, '[]'::jsonb) || '[
    {"Key": "@GenCol@", "Value": "Columns"},
    {"Key": "@GenColB@", "Value": "Apply"},
    {"Key": "@GenColC@", "Value": "Reset to default"},
    {"Key": "@GenSelCol@", "Value": "Select which columns to display:"}
]'::jsonb,
    lastmodifieddate = now()
WHERE translationid = 669
  AND NOT EXISTS (
    SELECT 1
    FROM jsonb_array_elements(COALESCE(pack, '[]'::jsonb)) AS elem
    WHERE elem->>'Key' = '@GenCol@'
  );

-- @GenClo@ (Close) — backfill only if missing from the DB pack (already in EnglishLanguage.cs).
UPDATE language
SET pack = COALESCE(pack, '[]'::jsonb) || '[
    {"Key": "@GenClo@", "Value": "Close"}
]'::jsonb,
    lastmodifieddate = now()
WHERE translationid = 669
  AND NOT EXISTS (
    SELECT 1
    FROM jsonb_array_elements(COALESCE(pack, '[]'::jsonb)) AS elem
    WHERE elem->>'Key' = '@GenClo@'
  );
