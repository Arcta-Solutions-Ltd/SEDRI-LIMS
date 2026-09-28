-- Realign editspecimenform with the add received specimen page structure.
-- Page definitions are supplied by C# page configs; this migration updates stored form and query configs.

UPDATE configs
SET contents = jsonb_set(
    contents,
    '{pages}',
    '["specimenpatientdetails", "specimenattributes", "specimentimingsreceived", "ackreceiptpageforedit"]'::jsonb
)
WHERE configname = 'editspecimenform'
  AND contents IS NOT NULL
  AND contents != '{}'::jsonb
  AND contents ? 'pages';

UPDATE configs
SET contents = jsonb_set(
    contents,
    '{Pages}',
    '["specimenpatientdetails", "specimenattributes", "specimentimingsreceived", "ackreceiptpageforedit"]'::jsonb
)
WHERE configname = 'editspecimenform'
  AND contents IS NOT NULL
  AND contents != '{}'::jsonb
  AND contents ? 'Pages';

-- Ensure SpecimenByIdForEdit query loads fields required by the realigned edit form.
WITH
    fields_to_add (name, type, knownas) AS (
        VALUES
            ('AntibioticsInLast24hrsId', 'int', NULL),
            ('TempInLast24hrsId', 'int', NULL),
            ('AdditionalClinicalInformation', 'string', NULL),
            ('FurtherInformation', 'string', NULL),
            ('TestCategoryId', 'string', NULL),
            ('CultureTypeCategoryId', 'string', NULL)
    ),
    missing_fields AS (
        SELECT
            c.configname,
            jsonb_agg(
                CASE
                    WHEN f.knownas IS NULL THEN jsonb_build_object('Name', f.name, 'Type', f.type)
                    ELSE jsonb_build_object('Name', f.name, 'Type', f.type, 'KnownAs', f.knownas)
                END
            ) AS fields_to_append
        FROM
            configs c
            CROSS JOIN fields_to_add f
        WHERE
            c.configname = 'specimenbyidforedit'
            AND c.contents IS NOT NULL
            AND c.contents != '{}'::jsonb
            AND c.contents ? 'Fields'
            AND NOT EXISTS (
                SELECT 1
                FROM jsonb_array_elements(c.contents -> 'Fields') AS existing
                WHERE lower(existing ->> 'Name') = lower(f.name)
            )
        GROUP BY
            c.configname
    )
UPDATE configs c
SET contents = jsonb_set(
    c.contents,
    '{Fields}',
    (c.contents -> 'Fields') || mf.fields_to_append
)
FROM missing_fields mf
WHERE c.configname = mf.configname;
