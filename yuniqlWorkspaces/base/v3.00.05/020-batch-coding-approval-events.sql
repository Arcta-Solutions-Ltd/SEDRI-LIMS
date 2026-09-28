-- Batch breakpoint and expert rule approval events (list 74) and role permissions.
-- Run as a new migration; does not amend existing scripts.
-- Ids 1887-1890 (last used in v3.00.02 is 1886).

-- Topic: Breakpoints (721)
INSERT INTO listitem (id, listid, value, fixed, enabled, displayorder, deleted, lastmodifieddate)
SELECT 1887, 74, 'batchapprovebreakpoint', false, true, 1, false, now()
WHERE NOT EXISTS (SELECT 1 FROM listitem WHERE listid = 74 AND LOWER(TRIM(value)) = LOWER(TRIM('batchapprovebreakpoint')));

INSERT INTO listitemparentchild (parentid, childid, lastmodifieddate)
SELECT 721, li.id, now()
FROM listitem li
WHERE li.listid = 74 AND LOWER(li.value) = LOWER('batchapprovebreakpoint')
  AND NOT EXISTS (SELECT 1 FROM listitemparentchild WHERE parentid = 721 AND childid = li.id);

INSERT INTO listitem (id, listid, value, fixed, enabled, displayorder, deleted, lastmodifieddate)
SELECT 1888, 74, 'batchrejectbreakpoint', false, true, 1, false, now()
WHERE NOT EXISTS (SELECT 1 FROM listitem WHERE listid = 74 AND LOWER(TRIM(value)) = LOWER(TRIM('batchrejectbreakpoint')));

INSERT INTO listitemparentchild (parentid, childid, lastmodifieddate)
SELECT 721, li.id, now()
FROM listitem li
WHERE li.listid = 74 AND LOWER(li.value) = LOWER('batchrejectbreakpoint')
  AND NOT EXISTS (SELECT 1 FROM listitemparentchild WHERE parentid = 721 AND childid = li.id);

-- Topic: ExpertRules (147)
INSERT INTO listitem (id, listid, value, fixed, enabled, displayorder, deleted, lastmodifieddate)
SELECT 1889, 74, 'batchapproveexpertrule', false, true, 1, false, now()
WHERE NOT EXISTS (SELECT 1 FROM listitem WHERE listid = 74 AND LOWER(TRIM(value)) = LOWER(TRIM('batchapproveexpertrule')));

INSERT INTO listitemparentchild (parentid, childid, lastmodifieddate)
SELECT 147, li.id, now()
FROM listitem li
WHERE li.listid = 74 AND LOWER(li.value) = LOWER('batchapproveexpertrule')
  AND NOT EXISTS (SELECT 1 FROM listitemparentchild WHERE parentid = 147 AND childid = li.id);

INSERT INTO listitem (id, listid, value, fixed, enabled, displayorder, deleted, lastmodifieddate)
SELECT 1890, 74, 'batchrejectexpertrule', false, true, 1, false, now()
WHERE NOT EXISTS (SELECT 1 FROM listitem WHERE listid = 74 AND LOWER(TRIM(value)) = LOWER(TRIM('batchrejectexpertrule')));

INSERT INTO listitemparentchild (parentid, childid, lastmodifieddate)
SELECT 147, li.id, now()
FROM listitem li
WHERE li.listid = 74 AND LOWER(li.value) = LOWER('batchrejectexpertrule')
  AND NOT EXISTS (SELECT 1 FROM listitemparentchild WHERE parentid = 147 AND childid = li.id);

-- Grant batch breakpoint approval events to roles that have addbreakpointapproval
UPDATE role
SET eventpermission = jsonb_set(
    eventpermission::jsonb,
    '{AllowedEvents}',
    (eventpermission->'AllowedEvents')::jsonb || '["batchapprovebreakpoint"]'::jsonb
)
WHERE (eventpermission->'AllowedEvents') @> '["addbreakpointapproval"]'::jsonb
  AND NOT (eventpermission->'AllowedEvents') @> '["batchapprovebreakpoint"]'::jsonb;

UPDATE role
SET eventpermission = jsonb_set(
    eventpermission::jsonb,
    '{AllowedEvents}',
    (eventpermission->'AllowedEvents')::jsonb || '["batchrejectbreakpoint"]'::jsonb
)
WHERE (eventpermission->'AllowedEvents') @> '["addbreakpointapproval"]'::jsonb
  AND NOT (eventpermission->'AllowedEvents') @> '["batchrejectbreakpoint"]'::jsonb;

-- Grant batch expert rule approval events to roles that have addexpertruleapproval
UPDATE role
SET eventpermission = jsonb_set(
    eventpermission::jsonb,
    '{AllowedEvents}',
    (eventpermission->'AllowedEvents')::jsonb || '["batchapproveexpertrule"]'::jsonb
)
WHERE (eventpermission->'AllowedEvents') @> '["addexpertruleapproval"]'::jsonb
  AND NOT (eventpermission->'AllowedEvents') @> '["batchapproveexpertrule"]'::jsonb;

UPDATE role
SET eventpermission = jsonb_set(
    eventpermission::jsonb,
    '{AllowedEvents}',
    (eventpermission->'AllowedEvents')::jsonb || '["batchrejectexpertrule"]'::jsonb
)
WHERE (eventpermission->'AllowedEvents') @> '["addexpertruleapproval"]'::jsonb
  AND NOT (eventpermission->'AllowedEvents') @> '["batchrejectexpertrule"]'::jsonb;
