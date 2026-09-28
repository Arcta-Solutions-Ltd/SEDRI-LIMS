-- Ensure every role includes the mandatory Home sidebar key in menu permissions.
-- Match by stable key "home", not translated labels.

UPDATE role
SET menupermission = jsonb_set(
    menupermission,
    '{AllowedSidebarItems}',
    COALESCE(menupermission->'AllowedSidebarItems', '[]'::jsonb) || '["home"]'::jsonb
)
WHERE menupermission IS NOT NULL
  AND NOT COALESCE(menupermission->'AllowedSidebarItems', '[]'::jsonb) ? 'home';

UPDATE role
SET menupermission = '{"AllowedSidebarItems":["home"]}'::jsonb
WHERE menupermission IS NULL
   OR menupermission = '{}'::jsonb;
