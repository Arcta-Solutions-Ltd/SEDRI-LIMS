-- Re-asserts the Specimen Growth (list 133) self referencing hierarchy used by Table Maintenance.
--
-- The original migration lives in v3.00.02/014-specimen-growth-internal-hierarchy.sql, but it was authored
-- into a version directory that had already been released. yuniql only runs version directories newer than
-- the database's current version, so any installation that had already passed v3.00.02 never applied it and
-- genuinely lost the self reference: list.internalhierarchy defaults to false and listitem 427 / 428 stop
-- being system rows, which removes the Growth / No Growth parent option from the table entry form.
--
-- Everything below is idempotent and keyed on ids only, never on the values 'Growth' / 'No Growth', which are
-- translated per language.

ALTER TABLE list ADD COLUMN IF NOT EXISTS internalhierarchy BOOLEAN NOT NULL DEFAULT false;

UPDATE list SET internalhierarchy = true, lastmodifieddate = now()
WHERE id = 133 AND internalhierarchy = false;

UPDATE listitem SET fixed = true, lastmodifieddate = now()
WHERE id IN (427, 428) AND fixed = false;

-- Growth (427) children: Significant growth, Growth, 1+-4+, Mixed flora
INSERT INTO listitemparentchild (parentid, childid, lastmodifieddate)
SELECT 427, t.childid, now()
FROM (VALUES (126), (1087), (178), (179), (180), (181), (182)) AS t(childid)
WHERE EXISTS (SELECT 1 FROM listitem WHERE id = t.childid AND listid = 133)
  AND NOT EXISTS (SELECT 1 FROM listitemparentchild WHERE parentid = 427 AND childid = t.childid);

-- No Growth (428) children: No growth, Contaminant, Insignificant growth, Overgrown
INSERT INTO listitemparentchild (parentid, childid, lastmodifieddate)
SELECT 428, t.childid, now()
FROM (VALUES (177), (1050), (1086), (125)) AS t(childid)
WHERE EXISTS (SELECT 1 FROM listitem WHERE id = t.childid AND listid = 133)
  AND NOT EXISTS (SELECT 1 FROM listitemparentchild WHERE parentid = 428 AND childid = t.childid);

-- Keep this script even though it duplicates v3.00.02/014. It is the only version of these statements that
-- reaches a database installed before that file was moved into v3.00.02.
