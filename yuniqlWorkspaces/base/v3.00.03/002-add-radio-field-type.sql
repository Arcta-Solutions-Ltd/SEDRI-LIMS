-- Add Radio (151) to FieldTypeList so edit-field queries can resolve radio controls
INSERT INTO listitem (id, listid, value, lastmodifieddate, fixed, enabled, displayorder, deleted)
SELECT 151, 109, 'Radio', now(), false, true, 1, false
WHERE NOT EXISTS (SELECT 1 FROM listitem WHERE id = 151);
