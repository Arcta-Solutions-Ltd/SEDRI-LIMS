-- Event listitem 74: requestinstrumenttest (manual request from embedded instrument lists).
-- Topic link: Instruments (1080) in list 73 (see 003-list-items.sql).
-- ListItem id 1548 follows 075-instrument-machine-list.sql (1543-1547).

INSERT INTO listitem (id, listid, value, fixed, enabled, displayorder, deleted, lastmodifieddate)
SELECT 1548, 74, 'requestinstrumenttest', false, true, 1, false, now()
WHERE NOT EXISTS (SELECT 1 FROM listitem WHERE listid = 74 AND LOWER(value) = 'requestinstrumenttest');

INSERT INTO listitemparentchild (parentid, childid, lastmodifieddate)
SELECT 1080, li.id, now()
FROM listitem li
WHERE li.listid = 74 AND LOWER(li.value) = 'requestinstrumenttest'
  AND NOT EXISTS (SELECT 1 FROM listitemparentchild WHERE parentid = 1080 AND childid = li.id);

-- Optional: grant to roles that may accept instrument results (same operational scope).
UPDATE role
SET eventpermission = jsonb_set(
    eventpermission,
    '{AllowedEvents}',
    (eventpermission->'AllowedEvents')::jsonb || '["requestinstrumenttest"]'::jsonb
)
WHERE (eventpermission->'AllowedEvents') @> '["acceptinstrumentresults"]'::jsonb
  AND NOT (eventpermission->'AllowedEvents') @> '["requestinstrumenttest"]'::jsonb;
