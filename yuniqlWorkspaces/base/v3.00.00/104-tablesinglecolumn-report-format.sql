INSERT INTO
    configs (
        id,
        configname,
        configtypeid,
        contents,
        lastmodifieddate
    )
SELECT
    124,
    'tablesinglecolumn',
    21,
    '{
    "Name": "TableSingleColumn",
    "Type": "Table",
    "Grids": [
        {
            "left": 20,
            "width": "550"
        }
    ]
}',
    NOW ()
WHERE
    NOT EXISTS (
        SELECT 1 FROM configs WHERE configname = 'tablesinglecolumn'
    );

UPDATE configs
SET
    contents = REPLACE(contents::text, '"Format": "TableOne"', '"Format": "TableSingleColumn"')::jsonb,
    lastmodifieddate = NOW()
WHERE
    configname IN ('specimencommentssection', 'culturecommentssection')
    AND contents::text LIKE '%"Format": "TableOne"%';
