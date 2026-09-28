-- Re-assert Specimen Growth (list 133) parent links for Growth (427) and No Growth (428).
-- Idempotent safety net for deployments missing listitemparentchild rows, which prevents
-- CultureListBySpecimenIdQuery from returning GrowthTypeParentId and can hide the AST menu.
-- Match on ids only — never on translated listitem values.

INSERT INTO listitemparentchild (parentid, childid, lastmodifieddate)
SELECT 427, t.childid, now()
FROM (VALUES (126), (1087), (178), (179), (180), (181), (182)) AS t(childid)
WHERE EXISTS (SELECT 1 FROM listitem WHERE id = t.childid AND listid = 133)
  AND NOT EXISTS (SELECT 1 FROM listitemparentchild WHERE parentid = 427 AND childid = t.childid);

INSERT INTO listitemparentchild (parentid, childid, lastmodifieddate)
SELECT 428, t.childid, now()
FROM (VALUES (177), (1050), (1086), (125)) AS t(childid)
WHERE EXISTS (SELECT 1 FROM listitem WHERE id = t.childid AND listid = 133)
  AND NOT EXISTS (SELECT 1 FROM listitemparentchild WHERE parentid = 428 AND childid = t.childid);
