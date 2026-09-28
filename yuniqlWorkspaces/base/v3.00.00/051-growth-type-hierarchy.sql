-- Growth Type Hierarchy: Add Growth and No Growth as parent list items in SpecimenGrowth (list 133),
-- and link all existing growth items as children via listitemparentchild.
-- Rules can then use parent type (427=Growth, 428=No Growth) instead of enumerating individual IDs.

INSERT INTO listitem (id, listid, value, fixed, enabled, displayorder, deleted, lastmodifieddate)
OVERRIDING SYSTEM VALUE
SELECT 427, 133, 'Growth', false, true, 1, false, now()
WHERE NOT EXISTS (SELECT 1 FROM listitem WHERE id = 427);

INSERT INTO listitem (id, listid, value, fixed, enabled, displayorder, deleted, lastmodifieddate)
OVERRIDING SYSTEM VALUE
SELECT 428, 133, 'No Growth', false, true, 1, false, now()
WHERE NOT EXISTS (SELECT 1 FROM listitem WHERE id = 428);

-- Growth (427) children: Significant growth, Growth, 1+–4+, Mixed flora
INSERT INTO listitemparentchild (parentid, childid, lastmodifieddate)
SELECT 427, childid, now()
FROM (VALUES (126), (1087), (178), (179), (180), (181), (182)) AS t(childid)
WHERE NOT EXISTS (SELECT 1 FROM listitemparentchild WHERE parentid = 427 AND childid = t.childid);

-- No Growth (428) children: No growth, No growth of target organism, Insignificant growth, Overgrown
INSERT INTO listitemparentchild (parentid, childid, lastmodifieddate)
SELECT 428, childid, now()
FROM (VALUES (177), (1050), (1086), (125)) AS t(childid)
WHERE NOT EXISTS (SELECT 1 FROM listitemparentchild WHERE parentid = 428 AND childid = t.childid);
