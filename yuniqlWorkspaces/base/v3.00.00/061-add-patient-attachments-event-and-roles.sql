-- Add managepatientattachments event to list 74 (Event)
-- so MessageQueue.AddAsync can resolve event ids for the queue (fixes "Missing event id" warning).
-- Links event to Patient topic (623) via listitemparentchild.
INSERT INTO listitem (id, listid, value, fixed, enabled, displayorder, deleted, lastmodifieddate)
SELECT 1522, 74, 'managepatientattachments', false, true, 1, false, now()
WHERE NOT EXISTS (SELECT 1 FROM listitem WHERE listid = 74 AND LOWER(value) = 'managepatientattachments');

-- Link managepatientattachments to Patient topic (623)
INSERT INTO listitemparentchild (parentid, childid, lastmodifieddate)
SELECT 623, li.id, now()
FROM listitem li
WHERE li.listid = 74 AND LOWER(li.value) = 'managepatientattachments'
  AND NOT EXISTS (SELECT 1 FROM listitemparentchild WHERE parentid = 623 AND childid = li.id);

-- Add managepatientattachments event to roles that have addpatienttag
UPDATE role
SET eventpermission = jsonb_set(
    eventpermission::jsonb,
    '{AllowedEvents}',
    (eventpermission->'AllowedEvents')::jsonb || '["managepatientattachments"]'::jsonb
)
WHERE (eventpermission->'AllowedEvents') @> '["addpatienttag"]'::jsonb
  AND NOT (eventpermission->'AllowedEvents') @> '["managepatientattachments"]'::jsonb;
