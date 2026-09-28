-- Language keys for edit direct test definition page heading and description.
UPDATE language
SET pack = COALESCE(pack, '[]'::jsonb) || '[
    {"Key": "@ConEdiDT@", "Value": "Edit Direct Test Definition"},
    {"Key": "@ConEdiDTA@", "Value": "Edit an existing direct test definition"}
]'::jsonb,
    lastmodifieddate = now()
WHERE translationid = 669
  AND NOT EXISTS (
    SELECT 1
    FROM jsonb_array_elements(COALESCE(pack, '[]'::jsonb)) AS elem
    WHERE elem->>'Key' = '@ConEdiDT@'
  );
