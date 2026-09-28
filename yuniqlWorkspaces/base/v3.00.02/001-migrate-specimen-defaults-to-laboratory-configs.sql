-- =============================================================================
-- Move the specimen default mappings out of the global specimens view config
-- and into laboratoryconfigs rows for laboratory 1.
--
-- Installs that have customised the specimens config (configname = 'specimens',
-- configtypeid = 5) hold their specimen type -> culture type and specimen type
-- -> direct test mappings inside that config's DefaultCultures and DefaultTests
-- arrays. The application reads those mappings from laboratoryconfigs instead,
-- so this migration copies them across and then removes them from the source
-- config.
--
-- The steps below are a single logical operation and are kept in one file
-- because the states in between are not valid: between a delete and its insert
-- the laboratory has no default mappings, and the final step destroys the data
-- the inserts read from.
--
--   1. specimentypeculturetypedefault, rebuilt from DefaultCultures.
--   2. specimentypedirecttestdefault, rebuilt from DefaultTests.
--   3. Drop the consumed arrays from the specimens config. Everything else it
--      holds -- GridColumns, filters and so on -- is left alone, so nothing
--      that was not migrated is lost, and an array whose seeded defaults were
--      kept in step 1 or 2 is left in place too.
--
-- Steps 1 and 2 each drop the seeded defaults for their own configname (id <
-- 10000, from v3.00.00/018-defaults.sql) before inserting, so the migrated
-- mappings do not sit alongside and conflict with the out-of-the-box ones. Each
-- delete is guarded on its own array holding at least one usable entry, so a
-- config that populates only one of the two arrays keeps the seeded defaults
-- for the other, and an install still on the shipped '{}' config keeps both.
-- The guard mirrors the filter on the matching insert: the seeded rows are only
-- removed when there is something to replace them with.
--
-- Rows are written in the shape the application expects, matching
-- v3.00.00/018-defaults.sql and arcportal/src/Classes/Laboratory/Laboratory.js:
--
--   {"Id": 1, "GroupId": "810", "AssociatedListId": "978,979"}
--
-- Id continues from the highest existing Id held in contents for that
-- configname, or starts at 1 when there is none -- which is the case once the
-- seeded rows have been removed and the laboratory has no custom rows. Entries
-- with no values are skipped, because GetRuleListQuery splits AssociatedListId
-- without a null check.
--
-- No explicit BEGIN/COMMIT here -- yuniql manages the transaction around the
-- migration run.
-- =============================================================================


-- Step 1a -- drop the seeded specimentypeculturetypedefault rows, but only if
-- DefaultCultures has an entry to replace them with.

DELETE FROM laboratoryconfigs
WHERE id < 10000
  AND configname = 'specimentypeculturetypedefault'
  AND EXISTS (
      SELECT 1
      FROM configs c
      CROSS JOIN LATERAL jsonb_array_elements(
          CASE
              WHEN jsonb_typeof(COALESCE(c.contents -> 'DefaultCultures', c.contents -> 'defaultcultures')) = 'array'
              THEN COALESCE(c.contents -> 'DefaultCultures', c.contents -> 'defaultcultures')
          END
      ) AS item
      WHERE c.configname = 'specimens'
        AND c.configtypeid = 5
        AND c.contents IS NOT NULL
        AND jsonb_typeof(c.contents) = 'object'
        AND COALESCE(
                item ->> 'CultureType', item ->> 'culturetype',
                item ->> 'SpecimenType', item ->> 'specimentype'
            ) IS NOT NULL
        AND jsonb_typeof(COALESCE(item -> 'Values', item -> 'values')) = 'array'
        AND jsonb_array_length(COALESCE(item -> 'Values', item -> 'values')) > 0
  );


-- Step 1b -- specimentypeculturetypedefault rows from DefaultCultures.

WITH source AS (
    SELECT
        c.id AS config_id,
        COALESCE(c.contents -> 'DefaultCultures', c.contents -> 'defaultcultures') AS default_cultures
    FROM configs c
    WHERE c.configname = 'specimens'
      AND c.configtypeid = 5
      AND c.contents IS NOT NULL
      AND jsonb_typeof(c.contents) = 'object'
),
cultures AS (
    SELECT
        row_number() OVER (ORDER BY s.config_id, t.ordinality) AS seq,
        COALESCE(
            t.item ->> 'CultureType', t.item ->> 'culturetype',
            t.item ->> 'SpecimenType', t.item ->> 'specimentype'
        ) AS group_id,
        (
            SELECT string_agg(v.value #>> '{}', ',' ORDER BY v.ordinality)
            FROM jsonb_array_elements(
                CASE
                    WHEN jsonb_typeof(COALESCE(t.item -> 'Values', t.item -> 'values')) = 'array'
                    THEN COALESCE(t.item -> 'Values', t.item -> 'values')
                END
            ) WITH ORDINALITY AS v (value, ordinality)
        ) AS associated_list_id
    FROM source s
    CROSS JOIN LATERAL jsonb_array_elements(s.default_cultures) WITH ORDINALITY AS t (item, ordinality)
    WHERE jsonb_typeof(s.default_cultures) = 'array'
),
start_id AS (
    SELECT COALESCE(MAX((lc.contents ->> 'Id')::int), 0) AS value
    FROM laboratoryconfigs lc
    WHERE lc.configname = 'specimentypeculturetypedefault'
      AND lc.laboratoryid = 1
      AND lc.contents ->> 'Id' ~ '^\d+$'
)
INSERT INTO laboratoryconfigs (laboratoryid, configname, contents, lastmodifieddate)
SELECT
    1,
    'specimentypeculturetypedefault',
    jsonb_build_object(
        'Id', start_id.value + row_number() OVER (ORDER BY t.seq),
        'GroupId', t.group_id,
        'AssociatedListId', t.associated_list_id
    ),
    now()
FROM cultures t
CROSS JOIN start_id
WHERE t.group_id IS NOT NULL
  AND t.associated_list_id IS NOT NULL
  AND t.associated_list_id <> '';


-- Step 2a -- drop the seeded specimentypedirecttestdefault rows, but only if
-- DefaultTests has an entry to replace them with.

DELETE FROM laboratoryconfigs
WHERE id < 10000
  AND configname = 'specimentypedirecttestdefault'
  AND EXISTS (
      SELECT 1
      FROM configs c
      CROSS JOIN LATERAL jsonb_array_elements(
          CASE
              WHEN jsonb_typeof(COALESCE(c.contents -> 'DefaultTests', c.contents -> 'defaulttests')) = 'array'
              THEN COALESCE(c.contents -> 'DefaultTests', c.contents -> 'defaulttests')
          END
      ) AS item
      WHERE c.configname = 'specimens'
        AND c.configtypeid = 5
        AND c.contents IS NOT NULL
        AND jsonb_typeof(c.contents) = 'object'
        AND COALESCE(item ->> 'SpecimenType', item ->> 'specimentype') IS NOT NULL
        AND jsonb_typeof(COALESCE(item -> 'Values', item -> 'values')) = 'array'
        AND jsonb_array_length(COALESCE(item -> 'Values', item -> 'values')) > 0
  );


-- Step 2b -- specimentypedirecttestdefault rows from DefaultTests.

WITH source AS (
    SELECT
        c.id AS config_id,
        COALESCE(c.contents -> 'DefaultTests', c.contents -> 'defaulttests') AS default_tests
    FROM configs c
    WHERE c.configname = 'specimens'
      AND c.configtypeid = 5
      AND c.contents IS NOT NULL
      AND jsonb_typeof(c.contents) = 'object'
),
tests AS (
    SELECT
        row_number() OVER (ORDER BY s.config_id, t.ordinality) AS seq,
        COALESCE(t.item ->> 'SpecimenType', t.item ->> 'specimentype') AS group_id,
        (
            SELECT string_agg(v.value #>> '{}', ',' ORDER BY v.ordinality)
            FROM jsonb_array_elements(
                CASE
                    WHEN jsonb_typeof(COALESCE(t.item -> 'Values', t.item -> 'values')) = 'array'
                    THEN COALESCE(t.item -> 'Values', t.item -> 'values')
                END
            ) WITH ORDINALITY AS v (value, ordinality)
        ) AS associated_list_id
    FROM source s
    CROSS JOIN LATERAL jsonb_array_elements(s.default_tests) WITH ORDINALITY AS t (item, ordinality)
    WHERE jsonb_typeof(s.default_tests) = 'array'
),
start_id AS (
    SELECT COALESCE(MAX((lc.contents ->> 'Id')::int), 0) AS value
    FROM laboratoryconfigs lc
    WHERE lc.configname = 'specimentypedirecttestdefault'
      AND lc.laboratoryid = 1
      AND lc.contents ->> 'Id' ~ '^\d+$'
)
INSERT INTO laboratoryconfigs (laboratoryid, configname, contents, lastmodifieddate)
SELECT
    1,
    'specimentypedirecttestdefault',
    jsonb_build_object(
        'Id', start_id.value + row_number() OVER (ORDER BY t.seq),
        'GroupId', t.group_id,
        'AssociatedListId', t.associated_list_id
    ),
    now()
FROM tests t
CROSS JOIN start_id
WHERE t.group_id IS NOT NULL
  AND t.associated_list_id IS NOT NULL
  AND t.associated_list_id <> '';


-- Step 3a -- drop DefaultCultures, but only if step 1 migrated it. The guard is
-- the same one step 1a used, so the array survives in the config whenever the
-- seeded defaults were kept.

UPDATE configs c
SET contents = c.contents - 'DefaultCultures' - 'defaultcultures',
    lastmodifieddate = now()
WHERE c.configname = 'specimens'
  AND c.configtypeid = 5
  AND c.contents IS NOT NULL
  AND jsonb_typeof(c.contents) = 'object'
  AND EXISTS (
      SELECT 1
      FROM jsonb_array_elements(
          CASE
              WHEN jsonb_typeof(COALESCE(c.contents -> 'DefaultCultures', c.contents -> 'defaultcultures')) = 'array'
              THEN COALESCE(c.contents -> 'DefaultCultures', c.contents -> 'defaultcultures')
          END
      ) AS item
      WHERE COALESCE(
                item ->> 'CultureType', item ->> 'culturetype',
                item ->> 'SpecimenType', item ->> 'specimentype'
            ) IS NOT NULL
        AND jsonb_typeof(COALESCE(item -> 'Values', item -> 'values')) = 'array'
        AND jsonb_array_length(COALESCE(item -> 'Values', item -> 'values')) > 0
  );


-- Step 3b -- drop DefaultTests, but only if step 2 migrated it.

UPDATE configs c
SET contents = c.contents - 'DefaultTests' - 'defaulttests',
    lastmodifieddate = now()
WHERE c.configname = 'specimens'
  AND c.configtypeid = 5
  AND c.contents IS NOT NULL
  AND jsonb_typeof(c.contents) = 'object'
  AND EXISTS (
      SELECT 1
      FROM jsonb_array_elements(
          CASE
              WHEN jsonb_typeof(COALESCE(c.contents -> 'DefaultTests', c.contents -> 'defaulttests')) = 'array'
              THEN COALESCE(c.contents -> 'DefaultTests', c.contents -> 'defaulttests')
          END
      ) AS item
      WHERE COALESCE(item ->> 'SpecimenType', item ->> 'specimentype') IS NOT NULL
        AND jsonb_typeof(COALESCE(item -> 'Values', item -> 'values')) = 'array'
        AND jsonb_array_length(COALESCE(item -> 'Values', item -> 'values')) > 0
  );
