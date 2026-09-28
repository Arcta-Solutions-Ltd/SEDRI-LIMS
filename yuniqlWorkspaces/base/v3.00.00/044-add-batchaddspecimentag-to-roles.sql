-- Add batchaddspecimentag event to roles that have addspecimentag
UPDATE role
SET eventpermission = jsonb_set(
    eventpermission::jsonb,
    '{AllowedEvents}',
    (eventpermission->'AllowedEvents')::jsonb || '["batchaddspecimentag"]'::jsonb
)
WHERE (eventpermission->'AllowedEvents') @> '["addspecimentag"]'::jsonb
  AND NOT (eventpermission->'AllowedEvents') @> '["batchaddspecimentag"]'::jsonb;
