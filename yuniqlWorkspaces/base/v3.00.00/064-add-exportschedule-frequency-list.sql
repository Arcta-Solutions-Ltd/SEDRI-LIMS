-- Add ExportScheduleFrequency list for schedule frequency combobox (hourly, daily, monthly)
INSERT INTO list (id, name, grouping, parentid, common, description, lastmodifieddate, deleted)
SELECT 139, 'ExportScheduleFrequency', 'Export', null, false, 'Export Schedule Frequency', now(), false
WHERE NOT EXISTS (SELECT 1 FROM list WHERE id = 139 OR name = 'ExportScheduleFrequency');

-- Add frequency options (IDs 1526-1528; 063 uses up to 1525)
INSERT INTO listitem (id, listid, value, fixed, enabled, displayorder, deleted, lastmodifieddate)
SELECT 1526, 139, 'hourly', false, true, 1, false, now()
WHERE NOT EXISTS (SELECT 1 FROM listitem WHERE listid = 139 AND LOWER(value) = 'hourly');

INSERT INTO listitem (id, listid, value, fixed, enabled, displayorder, deleted, lastmodifieddate)
SELECT 1527, 139, 'daily', false, true, 2, false, now()
WHERE NOT EXISTS (SELECT 1 FROM listitem WHERE listid = 139 AND LOWER(value) = 'daily');

INSERT INTO listitem (id, listid, value, fixed, enabled, displayorder, deleted, lastmodifieddate)
SELECT 1528, 139, 'monthly', false, true, 3, false, now()
WHERE NOT EXISTS (SELECT 1 FROM listitem WHERE listid = 139 AND LOWER(value) = 'monthly');
