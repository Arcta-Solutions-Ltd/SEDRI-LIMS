-- GraphType (list 14): Polar Area (id 484) and Radar (id 487).
-- English base seed skipped 484; some locale scripts included Polar with id 484 — insert only if missing.

INSERT INTO listitem (id, listid, value, fixed, enabled, displayorder, deleted, lastmodifieddate)
SELECT 484, 14, 'Polar Area Chart', false, true, 6, false, now()
WHERE NOT EXISTS (SELECT 1 FROM listitem WHERE id = 484);

INSERT INTO listitem (id, listid, value, fixed, enabled, displayorder, deleted, lastmodifieddate)
SELECT 487, 14, 'Radar Chart', false, true, 7, false, now()
WHERE NOT EXISTS (SELECT 1 FROM listitem WHERE id = 487);
