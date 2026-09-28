-- Move form group (subsection) between pages configuration.
-- Adds moveformgroup event to queue taxonomy and grants it to roles that can move fields.

-- ---------------------------------------------------------------------------
-- 1. Queue taxonomy: moveformgroup in list 74 under Configuration (402)
-- ---------------------------------------------------------------------------

INSERT INTO listitem (id, listid, value, fixed, enabled, displayorder, deleted, lastmodifieddate)
SELECT 2193, 74, 'moveformgroup', false, true, 1, false, now()
WHERE NOT EXISTS (SELECT 1 FROM listitem WHERE listid = 74 AND LOWER(TRIM(value)) = LOWER(TRIM('moveformgroup')));

INSERT INTO listitemparentchild (parentid, childid, lastmodifieddate)
SELECT 402, li.id, now()
FROM listitem li
WHERE li.listid = 74 AND LOWER(li.value) = LOWER('moveformgroup')
  AND NOT EXISTS (SELECT 1 FROM listitemparentchild WHERE parentid = 402 AND childid = li.id);

-- ---------------------------------------------------------------------------
-- 2. Grant moveformgroup to every role that can already move a field.
-- ---------------------------------------------------------------------------

UPDATE role
SET eventpermission = jsonb_set(
    eventpermission::jsonb,
    '{AllowedEvents}',
    (eventpermission->'AllowedEvents')::jsonb || '["moveformgroup"]'::jsonb
)
WHERE (eventpermission->'AllowedEvents') @> '["movefield"]'::jsonb
  AND NOT (eventpermission->'AllowedEvents') @> '["moveformgroup"]'::jsonb;

-- ---------------------------------------------------------------------------
-- 3. Language pack additions for move field page picker and move subsection.
-- ---------------------------------------------------------------------------

UPDATE language
SET pack = COALESCE(pack, '[]'::jsonb) || '[
    {"Key": "@ConMovFGPag@", "Value": "Page"},
    {"Key": "@ConMovFGGrp@", "Value": "Move subsection"},
    {"Key": "@ConMovFGGrpText@", "Value": "Select the page to move this subsection to."}
]'::jsonb,
    lastmodifieddate = now()
WHERE translationid = 669
  AND NOT EXISTS (
    SELECT 1
    FROM jsonb_array_elements(COALESCE(pack, '[]'::jsonb)) AS elem
    WHERE elem->>'Key' = '@ConMovFGPag@'
  );

UPDATE language
SET pack = COALESCE(pack, '[]'::jsonb) || '[
    {"Key": "@ConMovFGText@", "Value": "Select the page and subsection to move this field to."}
]'::jsonb,
    lastmodifieddate = now()
WHERE translationid = 669
  AND EXISTS (
    SELECT 1
    FROM jsonb_array_elements(COALESCE(pack, '[]'::jsonb)) AS elem
    WHERE elem->>'Key' = '@ConMovFGText@'
      AND elem->>'Value' = 'Select the subsection to move this field to.'
  );
