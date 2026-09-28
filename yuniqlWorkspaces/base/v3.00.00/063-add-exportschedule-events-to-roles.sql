-- Add addexportschedule, editexportschedule, deleteexportschedule events to list 74 (Event)
-- Links events to Export topic (969) via listitemparentchild
INSERT INTO listitem (id, listid, value, fixed, enabled, displayorder, deleted, lastmodifieddate)
SELECT 1523, 74, 'addexportschedule', false, true, 1, false, now()
WHERE NOT EXISTS (SELECT 1 FROM listitem WHERE listid = 74 AND LOWER(value) = 'addexportschedule');

INSERT INTO listitem (id, listid, value, fixed, enabled, displayorder, deleted, lastmodifieddate)
SELECT 1524, 74, 'editexportschedule', false, true, 1, false, now()
WHERE NOT EXISTS (SELECT 1 FROM listitem WHERE listid = 74 AND LOWER(value) = 'editexportschedule');

INSERT INTO listitem (id, listid, value, fixed, enabled, displayorder, deleted, lastmodifieddate)
SELECT 1525, 74, 'deleteexportschedule', false, true, 1, false, now()
WHERE NOT EXISTS (SELECT 1 FROM listitem WHERE listid = 74 AND LOWER(value) = 'deleteexportschedule');

-- Link export schedule events to Export topic (969)
INSERT INTO listitemparentchild (parentid, childid, lastmodifieddate)
SELECT 969, li.id, now()
FROM listitem li
WHERE li.listid = 74 AND LOWER(li.value) = 'addexportschedule'
  AND NOT EXISTS (SELECT 1 FROM listitemparentchild WHERE parentid = 969 AND childid = li.id);

INSERT INTO listitemparentchild (parentid, childid, lastmodifieddate)
SELECT 969, li.id, now()
FROM listitem li
WHERE li.listid = 74 AND LOWER(li.value) = 'editexportschedule'
  AND NOT EXISTS (SELECT 1 FROM listitemparentchild WHERE parentid = 969 AND childid = li.id);

INSERT INTO listitemparentchild (parentid, childid, lastmodifieddate)
SELECT 969, li.id, now()
FROM listitem li
WHERE li.listid = 74 AND LOWER(li.value) = 'deleteexportschedule'
  AND NOT EXISTS (SELECT 1 FROM listitemparentchild WHERE parentid = 969 AND childid = li.id);

-- Add export schedule events to roles that have editexportprofile (can edit profiles, can manage schedules)
UPDATE role
SET eventpermission = jsonb_set(
    eventpermission,
    '{AllowedEvents}',
    (eventpermission->'AllowedEvents')::jsonb || '["addexportschedule", "editexportschedule", "deleteexportschedule"]'::jsonb
)
WHERE (eventpermission->'AllowedEvents') @> '["editexportprofile"]'::jsonb
  AND NOT (eventpermission->'AllowedEvents') @> '["addexportschedule"]'::jsonb;
