-- Add delete events for expert rule child rows (conditions, actions, test conditions)
-- Topic: Expert Rules (147)

INSERT INTO listitem (id, listid, value, fixed, enabled, displayorder, deleted, lastmodifieddate)
SELECT 1801, 74, 'deleteexpertrulecondition', false, true, 1, false, now()
WHERE NOT EXISTS (SELECT 1 FROM listitem WHERE listid = 74 AND LOWER(TRIM(value)) = LOWER(TRIM('deleteexpertrulecondition')));

INSERT INTO listitemparentchild (parentid, childid, lastmodifieddate)
SELECT 147, li.id, now()
FROM listitem li
WHERE li.listid = 74 AND LOWER(li.value) = LOWER('deleteexpertrulecondition')
  AND NOT EXISTS (SELECT 1 FROM listitemparentchild WHERE parentid = 147 AND childid = li.id);

INSERT INTO listitem (id, listid, value, fixed, enabled, displayorder, deleted, lastmodifieddate)
SELECT 1802, 74, 'deleteexpertruleaction', false, true, 1, false, now()
WHERE NOT EXISTS (SELECT 1 FROM listitem WHERE listid = 74 AND LOWER(TRIM(value)) = LOWER(TRIM('deleteexpertruleaction')));

INSERT INTO listitemparentchild (parentid, childid, lastmodifieddate)
SELECT 147, li.id, now()
FROM listitem li
WHERE li.listid = 74 AND LOWER(li.value) = LOWER('deleteexpertruleaction')
  AND NOT EXISTS (SELECT 1 FROM listitemparentchild WHERE parentid = 147 AND childid = li.id);

INSERT INTO listitem (id, listid, value, fixed, enabled, displayorder, deleted, lastmodifieddate)
SELECT 1803, 74, 'deleteexpertruletestcondition', false, true, 1, false, now()
WHERE NOT EXISTS (SELECT 1 FROM listitem WHERE listid = 74 AND LOWER(TRIM(value)) = LOWER(TRIM('deleteexpertruletestcondition')));

INSERT INTO listitemparentchild (parentid, childid, lastmodifieddate)
SELECT 147, li.id, now()
FROM listitem li
WHERE li.listid = 74 AND LOWER(li.value) = LOWER('deleteexpertruletestcondition')
  AND NOT EXISTS (SELECT 1 FROM listitemparentchild WHERE parentid = 147 AND childid = li.id);

-- Grant edit/delete child events to roles that can edit expert rules
UPDATE role
SET eventpermission = jsonb_set(
    eventpermission::jsonb,
    '{AllowedEvents}',
    (eventpermission->'AllowedEvents')::jsonb || '[
        "editexpertruletestcondition",
        "deleteexpertrulecondition",
        "deleteexpertruleaction",
        "deleteexpertruletestcondition"
    ]'::jsonb
)
WHERE (eventpermission->'AllowedEvents') @> '["editexpertrule"]'::jsonb
  AND NOT (eventpermission->'AllowedEvents') @> '["deleteexpertrulecondition"]'::jsonb;

-- Organisation Admin role (id 2) explicit grant (050 omitted editexpertruletestcondition)
UPDATE role
SET eventpermission = jsonb_set(
    eventpermission::jsonb,
    '{AllowedEvents}',
    (eventpermission->'AllowedEvents')::jsonb || '[
        "editexpertruletestcondition",
        "deleteexpertrulecondition",
        "deleteexpertruleaction",
        "deleteexpertruletestcondition"
    ]'::jsonb
)
WHERE id = 2
  AND NOT (eventpermission->'AllowedEvents') @> '["editexpertruletestcondition"]'::jsonb;
