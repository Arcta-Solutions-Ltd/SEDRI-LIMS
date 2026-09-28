-- Standardise PageConfig.TableName for specimen-related pages and remove redundant
-- Patient / Admission / Request TableExceptions from specimen create events.
-- Page TableName is now the sole configurable routing mechanism for the four tables.

-- ---------------------------------------------------------------------------
-- Page configs (configtypeid = 9): set tablename where missing or incorrect
-- ---------------------------------------------------------------------------

WITH page_targets (configname, target) AS (
    VALUES
        ('patientdetailspage', 'patient'),
        ('patientaddresspage', 'patient'),
        ('editpatientdetailspage', 'patient'),
        ('neoshieldpatientidentificationpage', 'patient'),
        ('neoshieldbirthdetailspage', 'patient'),
        ('neoshieldadmissionpage', 'admission'),
        ('neoshieldrequestheaderpage', 'request'),
        ('neoshieldclinicalstatepage', 'request')
)
UPDATE configs c
SET contents = (
        CASE
            WHEN c.contents ? 'TableName' THEN c.contents - 'TableName'
            ELSE c.contents
        END
    ) || jsonb_build_object('tablename', pt.target),
    lastmodifieddate = now()
FROM page_targets pt
WHERE c.configtypeid = 9
  AND lower(c.configname) = pt.configname
  AND NOT (
      (c.contents ? 'tablename' AND lower(c.contents ->> 'tablename') = pt.target)
      OR (c.contents ? 'TableName' AND lower(c.contents ->> 'TableName') = pt.target)
  );

-- ---------------------------------------------------------------------------
-- Event configs (configtypeid = 7): strip four-table TableExceptions
-- Culture-only exceptions (e.g. ManufacturersBarcode -> Culture) are retained.
-- ---------------------------------------------------------------------------

UPDATE configs c
SET contents = jsonb_set(
        c.contents,
        '{TableExceptions}',
        COALESCE(
            (
                SELECT jsonb_agg(elem ORDER BY ord)
                FROM jsonb_array_elements(COALESCE(c.contents -> 'TableExceptions', '[]'::jsonb))
                    WITH ORDINALITY AS t(elem, ord)
                WHERE lower(elem ->> 'table') NOT IN ('patient', 'admission', 'request', 'specimen')
            ),
            '[]'::jsonb
        )
    ),
    lastmodifieddate = now()
WHERE c.configtypeid = 7
  AND lower(c.configname) IN ('neoshieldspecimen', 'remotespecimen', 'newreceivedspecimen')
  AND c.contents ? 'TableExceptions'
  AND EXISTS (
      SELECT 1
      FROM jsonb_array_elements(COALESCE(c.contents -> 'TableExceptions', '[]'::jsonb)) AS elem
      WHERE lower(elem ->> 'table') IN ('patient', 'admission', 'request', 'specimen')
  );
