UPDATE role
SET eventpermission = jsonb_set(
  eventpermission::jsonb,
  array['AllowedEvents'],
  (eventpermission->'AllowedEvents')::jsonb || '["addaccessionnumbertext", "deletesetting"]'::jsonb) 
WHERE id = 2;
