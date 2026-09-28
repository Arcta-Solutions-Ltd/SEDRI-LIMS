-- Add organism scope isolate test option events to roles that have culture type culture test option events
UPDATE role
SET eventpermission = jsonb_set(
    eventpermission::jsonb,
    '{AllowedEvents}',
    (eventpermission->'AllowedEvents')::jsonb || '["addorganismscopeculturetestoptionevent", "editorganismscopeculturetestoptionevent", "deleteorganismscopeculturetestoptionevent"]'::jsonb
)
WHERE (eventpermission->'AllowedEvents') @> '["addculturetypeculturetestoptionevent"]'::jsonb
  AND NOT (eventpermission->'AllowedEvents') @> '["addorganismscopeculturetestoptionevent"]'::jsonb;
