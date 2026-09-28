-- Report headers and footers had no ConfigType of their own. They are read by name alone through
-- ReportHeaderAdapter and ReportFooterAdapter, so nothing forced the issue until the report designer
-- began writing section edits back: every section it saved went to ConfigTypeId 14, which put headers
-- and footers into the specimen report section type that SectionAdapter.GetAllSectionsAsync reads.
-- Give them their own types and move any record the designer already mistyped.
-- Idempotent: safe to re-run.

INSERT INTO ConfigType (Id, Name, Type)
OVERRIDING SYSTEM VALUE
SELECT 24, 'ReportHeader', 'ReportSection'
WHERE NOT EXISTS (SELECT 1 FROM ConfigType WHERE Id = 24);

INSERT INTO ConfigType (Id, Name, Type)
OVERRIDING SYSTEM VALUE
SELECT 25, 'ReportFooter', 'ReportSection'
WHERE NOT EXISTS (SELECT 1 FROM ConfigType WHERE Id = 25);

-- Keep the identity sequence ahead of the explicitly inserted ids.
SELECT setval(
    pg_get_serial_sequence('configtype', 'id'),
    GREATEST((SELECT MAX(Id) FROM ConfigType), 1),
    true
);

-- Move any header record the designer stored as a report section onto the header type. The reports
-- themselves name their header, so the report records are the only reliable way to tell a header
-- apart from an ordinary section.
UPDATE configs
SET configtypeid = 24,
    lastmodifieddate = now()
WHERE configtypeid = 14
  AND lower(configname) IN (
      SELECT lower(contents ->> 'Header')
      FROM configs
      WHERE configtypeid = 13
        AND jsonb_typeof(contents::jsonb) = 'object'
        AND contents ->> 'Header' IS NOT NULL
  );

UPDATE configs
SET configtypeid = 25,
    lastmodifieddate = now()
WHERE configtypeid = 14
  AND lower(configname) IN (
      SELECT lower(contents ->> 'Footer')
      FROM configs
      WHERE configtypeid = 13
        AND jsonb_typeof(contents::jsonb) = 'object'
        AND contents ->> 'Footer' IS NOT NULL
  );

-- Suffixed duplicates such as 'astsectiona' are deliberately left alone. The save path created them
-- by failing to find the type 15 original, then repointed the report at the duplicate, so the
-- duplicate now holds the edits and the original holds the pre-edit definition. Which one to keep is
-- a judgement about the report, not something a migration can decide. Run this to find them:
--
--   SELECT c.id, c.configname, c.configtypeid
--   FROM configs c
--   WHERE c.configtypeid = 14
--     AND EXISTS (
--         SELECT 1 FROM configs o
--         WHERE o.configtypeid = 15
--           AND c.configname ~ ('^' || o.configname || '[a-z]+$')
--     );
