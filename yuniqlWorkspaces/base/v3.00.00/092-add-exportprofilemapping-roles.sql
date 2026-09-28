-- Register the saveexportprofilemapping event in the Event list (74) and link it to the Export topic (969).
-- Then grant the event to any role that already has editexportprofile, mirroring the schedule events pattern in 063-add-exportschedule-events-to-roles.sql.
INSERT INTO listitem (id, listid, value, fixed, enabled, displayorder, deleted, lastmodifieddate)
SELECT 24, 74, 'saveexportprofilemapping', false, true, 1, false, now()
WHERE NOT EXISTS (SELECT 1 FROM listitem WHERE listid = 74 AND LOWER(value) = 'saveexportprofilemapping');

INSERT INTO listitemparentchild (parentid, childid, lastmodifieddate)
SELECT 969, li.id, now()
FROM listitem li
WHERE li.listid = 74 AND LOWER(li.value) = 'saveexportprofilemapping'
  AND NOT EXISTS (SELECT 1 FROM listitemparentchild WHERE parentid = 969 AND childid = li.id);

UPDATE role
SET eventpermission = jsonb_set(
    eventpermission,
    '{AllowedEvents}',
    (eventpermission->'AllowedEvents')::jsonb || '["saveexportprofilemapping"]'::jsonb
)
WHERE (eventpermission->'AllowedEvents') @> '["editexportprofile"]'::jsonb
  AND NOT (eventpermission->'AllowedEvents') @> '["saveexportprofilemapping"]'::jsonb;
