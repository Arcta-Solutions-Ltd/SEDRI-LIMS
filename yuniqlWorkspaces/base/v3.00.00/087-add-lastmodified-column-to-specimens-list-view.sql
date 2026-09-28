-- Add Last Modified (lastmodifieddate) column to specimens list view config in DB when missing,
-- and ensure only column10 uses default sort (isSorted / isSortedDescending), matching SpecimenListViewConfig.

-- Append column10 to gridColumns (camelCase) when not already present
WITH
    lm_column AS (
        SELECT '{"key": "column10", "name": "@SpeMod@", "fieldName": "lastmodifieddate", "minWidth": 110, "maxWidth": 120, "isResizable": true, "isCollapsible": true, "isSorted": true, "isSortedDescending": true}'::jsonb AS col
    ),
    configs_needing_lm AS (
        SELECT c.id, c.contents->'gridColumns' AS grid_cols
        FROM configs c
        WHERE c.configname = 'specimens'
          AND c.contents IS NOT NULL
          AND c.contents != '{}'::jsonb
          AND c.contents->'gridColumns' IS NOT NULL
          AND NOT EXISTS (
              SELECT 1
              FROM jsonb_array_elements(c.contents->'gridColumns') AS elem
              WHERE lower(COALESCE(elem->>'key', elem->>'Key')) = 'column10'
                 OR lower(COALESCE(elem->>'fieldName', elem->>'FieldName')) = 'lastmodifieddate'
          )
    )
UPDATE configs c
SET contents = jsonb_set(
    c.contents,
    '{gridColumns}',
    (cnt.grid_cols || (SELECT col FROM lm_column))
)
FROM configs_needing_lm cnt
WHERE c.id = cnt.id;

-- Append column10 to GridColumns (PascalCase root) when not already present
WITH
    lm_column AS (
        SELECT '{"key": "column10", "name": "@SpeMod@", "fieldName": "lastmodifieddate", "minWidth": 110, "maxWidth": 120, "isResizable": true, "isCollapsible": true, "isSorted": true, "isSortedDescending": true}'::jsonb AS col
    ),
    configs_needing_lm AS (
        SELECT c.id, c.contents->'GridColumns' AS grid_cols
        FROM configs c
        WHERE c.configname = 'specimens'
          AND c.contents IS NOT NULL
          AND c.contents != '{}'::jsonb
          AND c.contents->'GridColumns' IS NOT NULL
          AND NOT EXISTS (
              SELECT 1
              FROM jsonb_array_elements(c.contents->'GridColumns') AS elem
              WHERE lower(COALESCE(elem->>'key', elem->>'Key')) = 'column10'
                 OR lower(COALESCE(elem->>'fieldName', elem->>'FieldName')) = 'lastmodifieddate'
          )
    )
UPDATE configs c
SET contents = jsonb_set(
    c.contents,
    '{GridColumns}',
    (cnt.grid_cols || (SELECT col FROM lm_column))
)
FROM configs_needing_lm cnt
WHERE c.id = cnt.id;

-- Normalize sort flags: only column10 + lastmodifieddate keeps default sort; strip from all other columns
UPDATE configs c
SET contents = jsonb_set(
    c.contents,
    '{gridColumns}',
    sub.new_cols
)
FROM (
    SELECT c2.id,
        (
            SELECT jsonb_agg(
                CASE
                    WHEN lower(COALESCE(elem->>'key', elem->>'Key')) = 'column10'
                         AND lower(COALESCE(elem->>'fieldName', elem->>'FieldName')) = 'lastmodifieddate'
                        THEN (elem - 'IsSorted' - 'isSorted' - 'IsSortedDescending' - 'isSortedDescending')
                             || '{"isSorted": true, "isSortedDescending": true}'::jsonb
                    ELSE (elem - 'IsSorted' - 'isSorted' - 'IsSortedDescending' - 'isSortedDescending')
                END
            )
            FROM jsonb_array_elements(c2.contents->'gridColumns') elem
        ) AS new_cols
    FROM configs c2
    WHERE c2.configname = 'specimens'
      AND c2.contents->'gridColumns' IS NOT NULL
) sub
WHERE c.id = sub.id
  AND sub.new_cols IS NOT NULL;

UPDATE configs c
SET contents = jsonb_set(
    c.contents,
    '{GridColumns}',
    sub.new_cols
)
FROM (
    SELECT c2.id,
        (
            SELECT jsonb_agg(
                CASE
                    WHEN lower(COALESCE(elem->>'key', elem->>'Key')) = 'column10'
                         AND lower(COALESCE(elem->>'fieldName', elem->>'FieldName')) = 'lastmodifieddate'
                        THEN (elem - 'IsSorted' - 'isSorted' - 'IsSortedDescending' - 'isSortedDescending')
                             || '{"isSorted": true, "isSortedDescending": true}'::jsonb
                    ELSE (elem - 'IsSorted' - 'isSorted' - 'IsSortedDescending' - 'isSortedDescending')
                END
            )
            FROM jsonb_array_elements(c2.contents->'GridColumns') elem
        ) AS new_cols
    FROM configs c2
    WHERE c2.configname = 'specimens'
      AND c2.contents->'GridColumns' IS NOT NULL
) sub
WHERE c.id = sub.id
  AND sub.new_cols IS NOT NULL;
