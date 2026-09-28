-- AST susceptibility override canned reasons as maintainable list items (replaces astsusceptibilityoverridecannedcomment).
-- ListItem ids 1815-1818 follow 122-configuration-formgroup-events-list74 (1814).

INSERT INTO list (id, name, grouping, parentid, common, description, lastmodifieddate, deleted)
SELECT 145, 'ASTSusOverrideCanned', 'Specimen', null, true, 'AST Susceptibility Override Canned Comments', now(), false
WHERE NOT EXISTS (SELECT 1 FROM list WHERE id = 145 OR lower(name) = lower('ASTSusOverrideCanned'));

INSERT INTO listitem (id, listid, value, fixed, enabled, displayorder, deleted, lastmodifieddate)
SELECT 1815, 145, 'Clinical correlation', true, true, 1, false, now()
WHERE NOT EXISTS (SELECT 1 FROM listitem WHERE id = 1815);

INSERT INTO listitem (id, listid, value, fixed, enabled, displayorder, deleted, lastmodifieddate)
SELECT 1816, 145, 'Repeat test discrepancy', true, true, 2, false, now()
WHERE NOT EXISTS (SELECT 1 FROM listitem WHERE id = 1816);

INSERT INTO listitem (id, listid, value, fixed, enabled, displayorder, deleted, lastmodifieddate)
SELECT 1817, 145, 'Expert review override', true, true, 3, false, now()
WHERE NOT EXISTS (SELECT 1 FROM listitem WHERE id = 1817);

INSERT INTO listitem (id, listid, value, fixed, enabled, displayorder, deleted, lastmodifieddate)
SELECT 1818, 145, 'Other — see free text', true, true, 4, false, now()
WHERE NOT EXISTS (SELECT 1 FROM listitem WHERE id = 1818);

-- Remap override rows that reference the legacy dedicated table (ids 1-4) to listitem ids.
DO $$
BEGIN
  IF EXISTS (
    SELECT 1 FROM information_schema.tables
    WHERE table_schema = 'public' AND table_name = 'astsusceptibilityoverridecannedcomment'
  ) THEN
    UPDATE astsusceptibilityoverride SET cannedcommentid = 1815 WHERE cannedcommentid = 1;
    UPDATE astsusceptibilityoverride SET cannedcommentid = 1816 WHERE cannedcommentid = 2;
    UPDATE astsusceptibilityoverride SET cannedcommentid = 1817 WHERE cannedcommentid = 3;
    UPDATE astsusceptibilityoverride SET cannedcommentid = 1818 WHERE cannedcommentid = 4;
    DROP TABLE astsusceptibilityoverridecannedcomment;
  END IF;
END $$;

-- Additional language keys (for DBs that already applied 124).
UPDATE language
SET pack = COALESCE(pack, '[]'::jsonb) || '[
    {"Key": "@AstSusPanDesc@", "Value": "Enter the reason for changing susceptibility from the calculated result."},
    {"Key": "@AstSusRevConf@", "Value": "This will restore the calculated susceptibility and remove the override reason."}
]'::jsonb,
    lastmodifieddate = now()
WHERE translationid = 669
  AND NOT EXISTS (
    SELECT 1
    FROM jsonb_array_elements(COALESCE(pack, '[]'::jsonb)) AS elem
    WHERE elem->>'Key' = '@AstSusPanDesc@'
  );
