-- Add ExportScheduleChangesToInclude list for schedule "Changes to include" radio (new only, new and modified)
INSERT INTO list (id, name, grouping, parentid, common, description, lastmodifieddate, deleted)
SELECT 140, 'ExportScheduleChangesToInclude', 'Export', null, false, 'Export Schedule Changes To Include', now(), false
WHERE NOT EXISTS (SELECT 1 FROM list WHERE id = 140 OR name = 'ExportScheduleChangesToInclude');

-- Add changes-to-include options (IDs 1529-1530; 064 uses up to 1528)
-- Value used for display; backend maps to newonly/newandmodified
INSERT INTO listitem (id, listid, value, fixed, enabled, displayorder, deleted, lastmodifieddate)
SELECT 1529, 140, 'New only', false, true, 1, false, now()
WHERE NOT EXISTS (SELECT 1 FROM listitem WHERE listid = 140 AND value = 'New only');

INSERT INTO listitem (id, listid, value, fixed, enabled, displayorder, deleted, lastmodifieddate)
SELECT 1530, 140, 'New and modified', false, true, 2, false, now()
WHERE NOT EXISTS (SELECT 1 FROM listitem WHERE listid = 140 AND value = 'New and modified');
