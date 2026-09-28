

INSERT INTO listitem (id, listid, value, fixed, enabled, displayorder, deleted, lastmodifieddate)
SELECT 1553, 144, 'BacT/ALERT', true, true, 1, false, now()
WHERE NOT EXISTS (SELECT 1 FROM listitem WHERE id = 1553);

INSERT INTO listitem (id, listid, value, fixed, enabled, displayorder, deleted, lastmodifieddate)
SELECT 1549, 144, 'BioTyper', true, true, 2, false, now()
WHERE NOT EXISTS (SELECT 1 FROM listitem WHERE id = 1549);

INSERT INTO listitem (id, listid, value, fixed, enabled, displayorder, deleted, lastmodifieddate)
SELECT 1550, 144, 'MIGIT', true, true, 3, false, now()
WHERE NOT EXISTS (SELECT 1 FROM listitem WHERE id = 1550);

INSERT INTO listitem (id, listid, value, fixed, enabled, displayorder, deleted, lastmodifieddate)
SELECT 1551, 144, 'MAX', true, true, 4, false, now()
WHERE NOT EXISTS (SELECT 1 FROM listitem WHERE id = 1551);

INSERT INTO listitem (id, listid, value, fixed, enabled, displayorder, deleted, lastmodifieddate)
SELECT 1552, 144, 'Sysmex', true, true, 5, false, now()
WHERE NOT EXISTS (SELECT 1 FROM listitem WHERE id = 1552);