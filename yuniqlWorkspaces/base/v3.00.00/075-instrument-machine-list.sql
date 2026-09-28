-- Instrument machine catalog for instrument profile wizard step 1 (List InstrumentMachine).
-- ListItem ids 1543-1547 follow 072-dashboard-timerange-unit-list (1539-1542).

INSERT INTO list (id, name, grouping, parentid, common, description, lastmodifieddate, deleted)
SELECT 144, 'InstrumentMachine', 'Instrument', null, true, 'External instrument machine / platform', now(), false
WHERE NOT EXISTS (SELECT 1 FROM list WHERE id = 144 OR lower(name) = lower('InstrumentMachine'));

INSERT INTO listitem (id, listid, value, fixed, enabled, displayorder, deleted, lastmodifieddate)
SELECT 1543, 144, 'Vitek 2', true, true, 1, false, now()
WHERE NOT EXISTS (SELECT 1 FROM listitem WHERE id = 1543);

INSERT INTO listitem (id, listid, value, fixed, enabled, displayorder, deleted, lastmodifieddate)
SELECT 1544, 144, 'Bactec', true, true, 2, false, now()
WHERE NOT EXISTS (SELECT 1 FROM listitem WHERE id = 1544);

INSERT INTO listitem (id, listid, value, fixed, enabled, displayorder, deleted, lastmodifieddate)
SELECT 1545, 144, 'Phoenix', true, true, 3, false, now()
WHERE NOT EXISTS (SELECT 1 FROM listitem WHERE id = 1545);

INSERT INTO listitem (id, listid, value, fixed, enabled, displayorder, deleted, lastmodifieddate)
SELECT 1546, 144, 'GeneXpert', true, true, 4, false, now()
WHERE NOT EXISTS (SELECT 1 FROM listitem WHERE id = 1546);

INSERT INTO listitem (id, listid, value, fixed, enabled, displayorder, deleted, lastmodifieddate)
SELECT 1547, 144, 'Vitek MS', true, true, 5, false, now()
WHERE NOT EXISTS (SELECT 1 FROM listitem WHERE id = 1547);
