-- Add addspecimentag and addpatienttag events to roles that have specimencomment/patientcomment
UPDATE role
SET eventpermission = jsonb_set(
    eventpermission::jsonb,
    '{AllowedEvents}',
    (eventpermission->'AllowedEvents')::jsonb || '["addspecimentag"]'::jsonb
)
WHERE (eventpermission->'AllowedEvents') @> '["specimencomment"]'::jsonb
  AND NOT (eventpermission->'AllowedEvents') @> '["addspecimentag"]'::jsonb;

UPDATE role
SET eventpermission = jsonb_set(
    eventpermission::jsonb,
    '{AllowedEvents}',
    (eventpermission->'AllowedEvents')::jsonb || '["addpatienttag"]'::jsonb
)
WHERE (eventpermission->'AllowedEvents') @> '["patientcomment"]'::jsonb
  AND NOT (eventpermission->'AllowedEvents') @> '["addpatienttag"]'::jsonb;
