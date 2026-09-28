-- Add Tags filter to patients list view config when it exists in ConfigList
-- but does not yet have the tag filter in the filters array.
WITH
    tag_filter AS (
        SELECT '{"key": "tag", "placeholder": "@GenTagA@", "multiSelect": true, "width": 250, "optionsName": "Tag", "fieldName": "tagid", "dropdownwidth": 850, "type": "hierarchicalpicker"}'::jsonb AS filter_elem
    ),
    configs_needing_tag_filter AS (
        SELECT c.id,
               c.contents,
               COALESCE(c.contents->'filters', c.contents->'Filters') AS filters_arr,
               CASE
                   WHEN c.contents->'filters' IS NOT NULL THEN 'filters'
                   ELSE 'Filters'
               END AS filter_key
        FROM configs c
        WHERE c.configname = 'patients'
          AND c.contents IS NOT NULL
          AND c.contents != '{}'::jsonb
          AND (c.contents->'filters' IS NOT NULL OR c.contents->'Filters' IS NOT NULL)
          AND NOT EXISTS (
              SELECT 1
              FROM jsonb_array_elements(COALESCE(c.contents->'filters', c.contents->'Filters')) AS elem
              WHERE lower(COALESCE(elem->>'fieldName', elem->>'FieldName')) = 'tagid'
          )
    )
UPDATE configs c
SET contents = jsonb_set(c.contents, ARRAY['filters'], (cnt.filters_arr || (SELECT filter_elem FROM tag_filter)))
FROM configs_needing_tag_filter cnt
WHERE c.id = cnt.id
  AND cnt.filter_key = 'filters';

-- Handle PascalCase Filters
WITH
    tag_filter AS (
        SELECT '{"key": "tag", "placeholder": "@GenTagA@", "multiSelect": true, "width": 250, "optionsName": "Tag", "fieldName": "tagid", "dropdownwidth": 850, "type": "hierarchicalpicker"}'::jsonb AS filter_elem
    ),
    configs_needing_tag_filter AS (
        SELECT c.id,
               c.contents->'Filters' AS filters_arr
        FROM configs c
        WHERE c.configname = 'patients'
          AND c.contents IS NOT NULL
          AND c.contents != '{}'::jsonb
          AND c.contents->'Filters' IS NOT NULL
          AND NOT EXISTS (
              SELECT 1
              FROM jsonb_array_elements(c.contents->'Filters') AS elem
              WHERE lower(COALESCE(elem->>'fieldName', elem->>'FieldName')) = 'tagid'
          )
    )
UPDATE configs c
SET contents = jsonb_set(c.contents, ARRAY['Filters'], (cnt.filters_arr || (SELECT filter_elem FROM tag_filter)))
FROM configs_needing_tag_filter cnt
WHERE c.id = cnt.id;
