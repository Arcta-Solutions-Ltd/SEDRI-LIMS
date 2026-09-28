-- Capitalise the initial letter of the ExportScheduleFrequency list item display values
-- (hourly/daily/monthly -> Hourly/Daily/Monthly). The schedule frequency is stored and matched
-- by list item id (1526/1527/1528) everywhere (ExportScheduleBackgroundService.IsDue, page
-- visibility rules, Cypress helpers), so changing the display text only affects what the user
-- sees in the Frequency combobox and the schedules list column. Ids are left unchanged.
UPDATE listitem SET value = 'Hourly', lastmodifieddate = now() WHERE id = 1526 AND listid = 139;
UPDATE listitem SET value = 'Daily', lastmodifieddate = now() WHERE id = 1527 AND listid = 139;
UPDATE listitem SET value = 'Monthly', lastmodifieddate = now() WHERE id = 1528 AND listid = 139;
