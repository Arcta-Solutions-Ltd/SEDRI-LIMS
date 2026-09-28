-- Grant savehomedashboardevent to roles that already have savecolumnlayoutsevent (home / preferences users).

UPDATE role
SET eventpermission = jsonb_set(
    eventpermission::jsonb,
    '{AllowedEvents}',
    (eventpermission->'AllowedEvents')::jsonb || '["savehomedashboardevent"]'::jsonb
)
WHERE (eventpermission->'AllowedEvents') @> '["savecolumnlayoutsevent"]'::jsonb
  AND NOT (eventpermission->'AllowedEvents') @> '["savehomedashboardevent"]'::jsonb;
