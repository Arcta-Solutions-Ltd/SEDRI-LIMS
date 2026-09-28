-- Add Upload (149) to FieldTypeList for configurable file attachment controls
INSERT INTO listitem (id, listid, value, lastmodifieddate, fixed, enabled, displayorder, deleted)
SELECT 149, 109, 'Upload', now(), false, true, 1, false
WHERE NOT EXISTS (SELECT 1 FROM listitem WHERE id = 149);

-- Add ContentTypeList for selecting allowed file types (MIME types and extensions)
-- List id 138 (123 is InstrumentStatus; highest v3 list is 137)
INSERT INTO List (id, name, grouping, parentid, common, description, lastmodifieddate)
SELECT 138, 'ContentTypeList', 'System', null, false, 'Content Type List', now()
WHERE NOT EXISTS (SELECT 1 FROM List WHERE id = 138 OR name = 'ContentTypeList');

-- Add content type options (Value is used for file validation - MIME type or extension)
-- IDs 886-895 (last listitem before this script: 885 in 001-general-settings)
INSERT INTO listitem (id, listid, value, lastmodifieddate, fixed, enabled, displayorder, deleted)
SELECT v.id, 138, v.value, now(), false, true, v.ord, false
FROM (VALUES
  (886, 'image/png', 1),
  (887, 'image/jpeg', 2),
  (888, 'image/gif', 3),
  (889, 'application/pdf', 4),
  (890, '.png', 5),
  (891, '.jpg', 6),
  (892, '.jpeg', 7),
  (893, '.gif', 8),
  (894, '.pdf', 9),
  (895, 'image/*', 10)
) AS v(id, value, ord)
WHERE NOT EXISTS (
  SELECT 1 FROM listitem li
  INNER JOIN list l ON l.id = li.listid
  WHERE Lower(l.name) = 'contenttypelist' AND Lower(li.value) = Lower(v.value)
);

-- Fix existing ContentTypeList items where deleted is null (GetListValuesQuery filters li.deleted = false)
UPDATE listitem SET deleted = false
WHERE listid = 138 AND deleted IS NULL;
