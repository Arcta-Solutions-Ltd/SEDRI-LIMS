-- Home dashboard rolling period: Period unit list (Years, Months, Days, Hours).
-- ListItem IDs 1539-1542 must match TIMERANGE_UNIT_ID_TO_API in arcportal homeDashboardUtils.js.
INSERT INTO list (id, name, grouping, parentid, common, description, lastmodifieddate, deleted)
SELECT 143, 'dashboardtimerangeunit', 'System', null, true, 'Dashboard time range period unit', now(), false
WHERE NOT EXISTS (SELECT 1 FROM list WHERE id = 143 OR lower(name) = lower('dashboardtimerangeunit'));

INSERT INTO listitem (id, listid, value, fixed, enabled, displayorder, deleted, lastmodifieddate)
SELECT 1539, 143, 'Years', true, true, 1, false, now()
WHERE NOT EXISTS (SELECT 1 FROM listitem WHERE id = 1539);

INSERT INTO listitem (id, listid, value, fixed, enabled, displayorder, deleted, lastmodifieddate)
SELECT 1540, 143, 'Months', true, true, 2, false, now()
WHERE NOT EXISTS (SELECT 1 FROM listitem WHERE id = 1540);

INSERT INTO listitem (id, listid, value, fixed, enabled, displayorder, deleted, lastmodifieddate)
SELECT 1541, 143, 'Days', true, true, 3, false, now()
WHERE NOT EXISTS (SELECT 1 FROM listitem WHERE id = 1541);

INSERT INTO listitem (id, listid, value, fixed, enabled, displayorder, deleted, lastmodifieddate)
SELECT 1542, 143, 'Hours', true, true, 4, false, now()
WHERE NOT EXISTS (SELECT 1 FROM listitem WHERE id = 1542);
