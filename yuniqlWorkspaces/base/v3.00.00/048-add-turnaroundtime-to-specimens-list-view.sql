-- Add Turn Around Time column to specimens list view config when it exists in configs
-- but does not yet have the turnaroundtime column. The column displays TAT colour from
-- SpecimenListTatEnricher and is available in the column picker for customisation.
-- Insert after the alert column (position 1) to match SpecimenListViewConfig.

-- Handle gridColumns (camelCase)
WITH
    tat_column AS (
        SELECT '{"key": "turnaroundtime", "name": "", "fieldName": "TurnAroundTimeColour", "minWidth": 28, "maxWidth": 28, "isResizable": false}'::jsonb AS col
    ),
    configs_needing_tat AS (
        SELECT c.id, c.contents
        FROM configs c
        WHERE c.configname = 'specimens'
          AND c.contents IS NOT NULL
          AND c.contents != '{}'::jsonb
          AND c.contents->'gridColumns' IS NOT NULL
          AND NOT EXISTS (
              SELECT 1
              FROM jsonb_array_elements(c.contents->'gridColumns') AS elem
              WHERE lower(COALESCE(elem->>'key', elem->>'Key')) = 'turnaroundtime'
                 OR lower(COALESCE(elem->>'fieldName', elem->>'FieldName')) = 'turnaroundtimecolour'
          )
    )
UPDATE configs c
SET contents = jsonb_set(
    c.contents,
    '{gridColumns}',
    jsonb_insert(
        cnt.contents->'gridColumns',
        '{1}',
        (SELECT col FROM tat_column),
        false
    )
)
FROM configs_needing_tat cnt
WHERE c.id = cnt.id;

-- Handle GridColumns (PascalCase)
WITH
    tat_column AS (
        SELECT '{"key": "turnaroundtime", "name": "", "fieldName": "TurnAroundTimeColour", "minWidth": 28, "maxWidth": 28, "isResizable": false}'::jsonb AS col
    ),
    configs_needing_tat AS (
        SELECT c.id, c.contents
        FROM configs c
        WHERE c.configname = 'specimens'
          AND c.contents IS NOT NULL
          AND c.contents != '{}'::jsonb
          AND c.contents->'GridColumns' IS NOT NULL
          AND NOT EXISTS (
              SELECT 1
              FROM jsonb_array_elements(c.contents->'GridColumns') AS elem
              WHERE lower(COALESCE(elem->>'key', elem->>'Key')) = 'turnaroundtime'
                 OR lower(COALESCE(elem->>'fieldName', elem->>'FieldName')) = 'turnaroundtimecolour'
          )
    )
UPDATE configs c
SET contents = jsonb_set(
    c.contents,
    '{GridColumns}',
    jsonb_insert(
        cnt.contents->'GridColumns',
        '{1}',
        (SELECT col FROM tat_column),
        false
    )
)
FROM configs_needing_tat cnt
WHERE c.id = cnt.id;
