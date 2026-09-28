-- Add tagsconfig to roles that have alertsconfig (OrganisationAdmin, etc.)
-- Appends tagsconfig to AllowedSidebarItems for roles that can manage alerts
UPDATE role
SET menupermission = jsonb_set(
    menupermission,
    '{AllowedSidebarItems}',
    (menupermission->'AllowedSidebarItems') || '["tagsconfig"]'::jsonb
)
WHERE (menupermission->'AllowedSidebarItems') @> '["alertsconfig"]'::jsonb
  AND NOT (menupermission->'AllowedSidebarItems') @> '["tagsconfig"]'::jsonb;
