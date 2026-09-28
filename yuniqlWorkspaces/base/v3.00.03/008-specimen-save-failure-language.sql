-- Specimen save failure language key (@SpeSavF@) for DB-backed English pack (translationid 669).
UPDATE language
SET pack = COALESCE(pack, '[]'::jsonb) || '[
    {"Key": "@SpeSavF@", "Value": "The specimen could not be saved. Please try again or contact your administrator."}
]'::jsonb,
    lastmodifieddate = now()
WHERE translationid = 669
  AND NOT EXISTS (
    SELECT 1
    FROM jsonb_array_elements(COALESCE(pack, '[]'::jsonb)) AS elem
    WHERE elem->>'Key' = '@SpeSavF@'
  );
