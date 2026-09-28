-- Page groups: drop the short-lived fixedOrder flag and backfill pageGroup on user-added
-- page overrides from their table name. Built-in pages get group metadata from the factory
-- via PageConfigAdapter, so they do not need a contents rewrite.
--
-- Language keys for the Page Order editor messages are added to the DB-backed English pack
-- (translationid 669) so installed systems pick them up without relying on the factory pack.

-- Remove leftover fixedOrder / FixedOrder keys from page configs (configtypeid 9).
UPDATE configs
SET contents = contents - 'fixedOrder',
    lastmodifieddate = now()
WHERE configtypeid = 9
  AND contents IS NOT NULL
  AND contents ? 'fixedOrder';

UPDATE configs
SET contents = contents - 'FixedOrder',
    lastmodifieddate = now()
WHERE configtypeid = 9
  AND contents IS NOT NULL
  AND contents ? 'FixedOrder';

-- Backfill pageGroup from tablename for pages that do not already declare a group.
-- Specimen pages stay ungrouped. Matching uses the table name id, not a translated label.
UPDATE configs
SET contents = jsonb_set(
        contents,
        '{pageGroup}',
        to_jsonb(lower(coalesce(contents->>'tablename', contents->>'TableName'))),
        true
    ),
    lastmodifieddate = now()
WHERE configtypeid = 9
  AND contents IS NOT NULL
  AND contents != '{}'::jsonb
  AND NOT (contents ? 'pageGroup')
  AND NOT (contents ? 'PageGroup')
  AND lower(coalesce(contents->>'tablename', contents->>'TableName', '')) IN ('patient', 'admission', 'request');

-- Refresh @ConPagL@ when the previous fixed-order wording is already in the English pack.
UPDATE language
SET pack = (
        SELECT jsonb_agg(
            CASE
                WHEN elem->>'Key' = '@ConPagL@' THEN '{"Key": "@ConPagL@", "Value": "Pages in this group must stay together and cannot be separated by another page."}'::jsonb
                ELSE elem
            END
        )
        FROM jsonb_array_elements(COALESCE(pack, '[]'::jsonb)) AS elem
    ),
    lastmodifieddate = now()
WHERE translationid = 669
  AND EXISTS (
    SELECT 1
    FROM jsonb_array_elements(COALESCE(pack, '[]'::jsonb)) AS elem
    WHERE elem->>'Key' = '@ConPagL@'
  );

-- addpage is a configuration event and must be EventType special. specialadddata treats a
-- return id of 0 as a failed specimen save, which blocks adding a page to a form.
UPDATE configs
SET contents = jsonb_set(contents, '{EventType}', '"special"'::jsonb, true),
    lastmodifieddate = now()
WHERE lower(configname) = 'addpage'
  AND contents IS NOT NULL
  AND lower(coalesce(contents->>'EventType', contents->>'eventType', '')) = 'specialadddata';

UPDATE configs
SET contents = jsonb_set(contents, '{eventType}', '"special"'::jsonb, true),
    lastmodifieddate = now()
WHERE lower(configname) = 'addpage'
  AND contents IS NOT NULL
  AND lower(contents->>'eventType') = 'specialadddata';

-- Add @ConPagL@ when the English pack does not already have it.
UPDATE language
SET pack = COALESCE(pack, '[]'::jsonb) || '[
    {"Key": "@ConPagL@", "Value": "Pages in this group must stay together and cannot be separated by another page."}
]'::jsonb,
    lastmodifieddate = now()
WHERE translationid = 669
  AND NOT EXISTS (
    SELECT 1
    FROM jsonb_array_elements(COALESCE(pack, '[]'::jsonb)) AS elem
    WHERE elem->>'Key' = '@ConPagL@'
  );

-- Add the remaining page group language keys.
UPDATE language
SET pack = COALESCE(pack, '[]'::jsonb) || '[
    {"Key": "@ConPagM@", "Value": "This page is fixed at the start of its group and cannot be moved."},
    {"Key": "@ConPagN@", "Value": "Page groups must stay in the order Patient, Admission, Request."},
    {"Key": "@ConPagO@", "Value": "Pages in this group must stay together."},
    {"Key": "@ConPagP@", "Value": "This page is required by the workflow and cannot be deleted."}
]'::jsonb,
    lastmodifieddate = now()
WHERE translationid = 669
  AND NOT EXISTS (
    SELECT 1
    FROM jsonb_array_elements(COALESCE(pack, '[]'::jsonb)) AS elem
    WHERE elem->>'Key' = '@ConPagM@'
  );
