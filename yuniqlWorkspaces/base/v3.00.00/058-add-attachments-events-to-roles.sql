-- Add managespecimenattachments event to roles that have addspecimentag
UPDATE role
SET eventpermission = jsonb_set(
    eventpermission::jsonb,
    '{AllowedEvents}',
    (eventpermission->'AllowedEvents')::jsonb || '["managespecimenattachments"]'::jsonb
)
WHERE (eventpermission->'AllowedEvents') @> '["addspecimentag"]'::jsonb
  AND NOT (eventpermission->'AllowedEvents') @> '["managespecimenattachments"]'::jsonb;

-- Add managecultureattachments event to roles that have culturecommentevent
UPDATE role
SET eventpermission = jsonb_set(
    eventpermission::jsonb,
    '{AllowedEvents}',
    (eventpermission->'AllowedEvents')::jsonb || '["managecultureattachments"]'::jsonb
)
WHERE (eventpermission->'AllowedEvents') @> '["culturecommentevent"]'::jsonb
  AND NOT (eventpermission->'AllowedEvents') @> '["managecultureattachments"]'::jsonb;
