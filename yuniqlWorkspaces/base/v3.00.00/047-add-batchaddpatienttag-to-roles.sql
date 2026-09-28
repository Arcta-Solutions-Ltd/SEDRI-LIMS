-- Add batchaddpatienttag event to roles that have addpatienttag
UPDATE role
SET eventpermission = jsonb_set(
    eventpermission::jsonb,
    '{AllowedEvents}',
    (eventpermission->'AllowedEvents')::jsonb || '["batchaddpatienttag"]'::jsonb
)
WHERE (eventpermission->'AllowedEvents') @> '["addpatienttag"]'::jsonb
  AND NOT (eventpermission->'AllowedEvents') @> '["batchaddpatienttag"]'::jsonb;
