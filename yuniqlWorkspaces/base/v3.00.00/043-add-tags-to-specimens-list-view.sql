-- Add Tags column to specimens list view config when it exists in ConfigList
-- (customized config with non-empty contents) but does not yet have the tags column.
-- The tags column is hidden by default (defaultHidden: true) and available in the column picker.
WITH
    tags_column AS (
        SELECT '{"key": "column9", "name": "@GenTagB@", "fieldName": "tags", "minWidth": 150, "maxWidth": 200, "isResizable": true, "isCollapsible": true, "defaultHidden": true}'::jsonb AS col
    ),
    configs_needing_tags AS (
        SELECT c.id, c.contents,
               COALESCE(c.contents->'gridColumns', c.contents->'GridColumns') AS grid_cols,
               CASE
                   WHEN c.contents->'gridColumns' IS NOT NULL THEN 'gridColumns'
                   ELSE 'GridColumns'
               END AS col_key
        FROM configs c
        WHERE c.configname = 'specimens'
          AND c.contents IS NOT NULL
          AND c.contents != '{}'::jsonb
          AND (c.contents->'gridColumns' IS NOT NULL OR c.contents->'GridColumns' IS NOT NULL)
          AND NOT EXISTS (
              SELECT 1
              FROM jsonb_array_elements(COALESCE(c.contents->'gridColumns', c.contents->'GridColumns')) AS elem
              WHERE lower(COALESCE(elem->>'fieldName', elem->>'FieldName')) = 'tags'
          )
    )
UPDATE configs c
SET contents = jsonb_set(
    c.contents,
    '{gridColumns}',
    (cnt.grid_cols || (SELECT col FROM tags_column))
)
FROM configs_needing_tags cnt
WHERE c.id = cnt.id
  AND cnt.col_key = 'gridColumns';

-- Handle PascalCase GridColumns (when stored from C# serialization)
WITH
    tags_column AS (
        SELECT '{"key": "column9", "name": "@GenTagB@", "fieldName": "tags", "minWidth": 150, "maxWidth": 200, "isResizable": true, "isCollapsible": true, "defaultHidden": true}'::jsonb AS col
    ),
    configs_needing_tags AS (
        SELECT c.id, c.contents,
               c.contents->'GridColumns' AS grid_cols
        FROM configs c
        WHERE c.configname = 'specimens'
          AND c.contents IS NOT NULL
          AND c.contents != '{}'::jsonb
          AND c.contents->'GridColumns' IS NOT NULL
          AND NOT EXISTS (
              SELECT 1
              FROM jsonb_array_elements(c.contents->'GridColumns') AS elem
              WHERE lower(COALESCE(elem->>'fieldName', elem->>'FieldName')) = 'tags'
          )
    )
UPDATE configs c
SET contents = jsonb_set(
    c.contents,
    '{GridColumns}',
    (cnt.grid_cols || (SELECT col FROM tags_column))
)
FROM configs_needing_tags cnt
WHERE c.id = cnt.id;
