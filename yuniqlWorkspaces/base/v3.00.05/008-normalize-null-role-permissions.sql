-- Normalize legacy role permission rows where menu or event permissions were left NULL or empty object.
-- Zero-access roles must store explicit empty arrays so sidebar filtering treats them as having no grants.

UPDATE role
SET menupermission = '{"AllowedSidebarItems":[]}'::jsonb
WHERE menupermission IS NULL
   OR menupermission = '{}'::jsonb;

UPDATE role
SET eventpermission = '{"AllowedEvents":[]}'::jsonb
WHERE eventpermission IS NULL
   OR eventpermission = '{}'::jsonb;
