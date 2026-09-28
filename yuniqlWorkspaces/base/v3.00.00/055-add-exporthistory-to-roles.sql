-- Add exportmenu and exporthistory to AllowedSidebarItems for roles that have exportprofile
-- exportmenu: parent menu key so Exports sub-menu displays
-- exporthistory: Export History list view

-- 1. Add exportmenu to roles that have exportprofile
UPDATE role
SET menupermission = jsonb_set(
    menupermission,
    '{AllowedSidebarItems}',
    (menupermission->'AllowedSidebarItems') || '["exportmenu"]'::jsonb
)
WHERE (menupermission->'AllowedSidebarItems') @> '["exportprofile"]'::jsonb
  AND NOT (menupermission->'AllowedSidebarItems') @> '["exportmenu"]'::jsonb;

-- 2. Add exporthistory to roles that have exportprofile
UPDATE role
SET menupermission = jsonb_set(
    menupermission,
    '{AllowedSidebarItems}',
    (menupermission->'AllowedSidebarItems') || '["exporthistory"]'::jsonb
)
WHERE (menupermission->'AllowedSidebarItems') @> '["exportprofile"]'::jsonb
  AND NOT (menupermission->'AllowedSidebarItems') @> '["exporthistory"]'::jsonb;
