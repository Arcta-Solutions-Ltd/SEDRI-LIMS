-- Add Last Modified (lastmodifieddate) column to archive list view config in DB when missing,
-- and ensure only column9 uses default sort (isSorted / isSortedDescending), matching ArchiveListViewConfig.

-- Append column9 to gridColumns (camelCase) when not already present
WITH
    lm_column AS (
        SELECT '{"key": "column9", "name": "@SpeMod@", "fieldName": "lastmodifieddate", "minWidth": 110, "maxWidth": 120, "isResizable": true, "isCollapsible": true, "isSorted": true, "isSortedDescending": true}'::jsonb AS col
    ),
    configs_needing_lm AS (
        SELECT c.id, c.contents->'gridColumns' AS grid_cols
        FROM configs c
        WHERE c.configname = 'archive'
          AND c.contents IS NOT NULL
          AND c.contents != '{}'::jsonb
          AND c.contents->'gridColumns' IS NOT NULL
          AND NOT EXISTS (
              SELECT 1
              FROM jsonb_array_elements(c.contents->'gridColumns') AS elem
              WHERE lower(COALESCE(elem->>'key', elem->>'Key')) = 'column9'
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

-- Append column9 to GridColumns (PascalCase root) when not already present
WITH
    lm_column AS (
        SELECT '{"key": "column9", "name": "@SpeMod@", "fieldName": "lastmodifieddate", "minWidth": 110, "maxWidth": 120, "isResizable": true, "isCollapsible": true, "isSorted": true, "isSortedDescending": true}'::jsonb AS col
    ),
    configs_needing_lm AS (
        SELECT c.id, c.contents->'GridColumns' AS grid_cols
        FROM configs c
        WHERE c.configname = 'archive'
          AND c.contents IS NOT NULL
          AND c.contents != '{}'::jsonb
          AND c.contents->'GridColumns' IS NOT NULL
          AND NOT EXISTS (
              SELECT 1
              FROM jsonb_array_elements(c.contents->'GridColumns') AS elem
              WHERE lower(COALESCE(elem->>'key', elem->>'Key')) = 'column9'
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

-- Normalize sort flags: only column9 + lastmodifieddate keeps default sort; strip from all other columns
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
                    WHEN lower(COALESCE(elem->>'key', elem->>'Key')) = 'column9'
                         AND lower(COALESCE(elem->>'fieldName', elem->>'FieldName')) = 'lastmodifieddate'
                        THEN (elem - 'IsSorted' - 'isSorted' - 'IsSortedDescending' - 'isSortedDescending')
                             || '{"isSorted": true, "isSortedDescending": true}'::jsonb
                    ELSE (elem - 'IsSorted' - 'isSorted' - 'IsSortedDescending' - 'isSortedDescending')
                END
            )
            FROM jsonb_array_elements(c2.contents->'gridColumns') elem
        ) AS new_cols
    FROM configs c2
    WHERE c2.configname = 'archive'
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
                    WHEN lower(COALESCE(elem->>'key', elem->>'Key')) = 'column9'
                         AND lower(COALESCE(elem->>'fieldName', elem->>'FieldName')) = 'lastmodifieddate'
                        THEN (elem - 'IsSorted' - 'isSorted' - 'IsSortedDescending' - 'isSortedDescending')
                             || '{"isSorted": true, "isSortedDescending": true}'::jsonb
                    ELSE (elem - 'IsSorted' - 'isSorted' - 'IsSortedDescending' - 'isSortedDescending')
                END
            )
            FROM jsonb_array_elements(c2.contents->'GridColumns') elem
        ) AS new_cols
    FROM configs c2
    WHERE c2.configname = 'archive'
      AND c2.contents->'GridColumns' IS NOT NULL
) sub
WHERE c.id = sub.id
  AND sub.new_cols IS NOT NULL;
