-- Ensures any new fields are added to the specimen list query if it exists in the DB
WITH
    fields_to_add (name, type) AS (
        VALUES
            ('LaboratoryId', 'int'),
            ('SpecimenTypeId', 'int'),
            ('TestCategoryId', 'string'),
            ('CultureTypeCategoryId', 'string'),
            ('LastModifiedDate', 'datetime')
    ),
    missing_fields AS (
        SELECT
            c.configname,
            jsonb_agg (
                jsonb_build_object ('Name', f.name, 'Type', f.type)
            ) AS fields_to_append
        FROM
            configs c
            CROSS JOIN fields_to_add f
        WHERE
            c.configname = 'specimenlist'
            AND NOT EXISTS (
                SELECT
                    1
                FROM
                    jsonb_array_elements (c.contents -> 'Fields') AS existing
                WHERE
                    lower(existing ->> 'Name') = lower(f.name)
            )
        GROUP BY
            c.configname
    )
UPDATE configs c
SET
    contents = jsonb_set (
        c.contents,
        '{Fields}',
        (c.contents -> 'Fields') || mf.fields_to_append
    )
FROM
    missing_fields mf
WHERE
    c.configname = mf.configname;

-- Add SP security tag
UPDATE configs 
SET contents = contents || jsonb_build_object( 
    'Tags',  
    CASE  
        WHEN (contents ->> 'Tags') IS NULL OR (contents ->> 'Tags') = '' THEN 'SP' 
        WHEN (contents ->> 'Tags') LIKE '%SP%' THEN (contents ->> 'Tags')
        ELSE (contents ->> 'Tags') || ',SP' 
    END 
) 
WHERE jsonb_typeof(contents) = 'object' AND configname = 'specimenlist'; 

-- Order by LastModifiedDate instead of AccessionNumber
UPDATE configs
SET contents = contents || '{"Orderby": "LastModifiedDate", "Descending": true}'::jsonb
WHERE contents ->> 'Orderby' = 'AccessionNumber'
  AND jsonb_typeof(contents) = 'object'
  AND configname = 'specimenlist';
