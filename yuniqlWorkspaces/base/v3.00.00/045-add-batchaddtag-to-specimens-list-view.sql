-- Add batch add tag button to specimens list view config when it exists in ConfigList
-- but the batchspecimen button does not yet have the batchaddtag nested button.
-- This ensures batchaddspecimentaguievent appears in the role permission editor under Tags.
WITH
    batch_add_tag_btn AS (
        SELECT '{"key": "batchaddtag", "text": "@SpeBatI@", "onBatch": true, "icon": "Tag", "uievent": "batchaddspecimentaguievent", "onFinish": "refresh"}'::jsonb AS btn
    ),
    configs_needing_batchaddtag AS (
        SELECT c.id,
               c.contents,
               COALESCE(c.contents->'buttons', c.contents->'Buttons') AS buttons,
               CASE
                   WHEN c.contents->'buttons' IS NOT NULL THEN 'buttons'
                   ELSE 'Buttons'
               END AS btn_key
        FROM configs c
        WHERE c.configname = 'specimens'
          AND c.contents IS NOT NULL
          AND c.contents != '{}'::jsonb
          AND (c.contents->'buttons' IS NOT NULL OR c.contents->'Buttons' IS NOT NULL)
          AND EXISTS (
              SELECT 1
              FROM jsonb_array_elements(COALESCE(c.contents->'buttons', c.contents->'Buttons')) AS b
              WHERE lower(COALESCE(b->>'key', b->>'Key')) = 'batchspecimen'
          )
          AND NOT EXISTS (
              SELECT 1
              FROM jsonb_array_elements(COALESCE(c.contents->'buttons', c.contents->'Buttons')) AS b
              CROSS JOIN LATERAL jsonb_array_elements(COALESCE(b->'buttons', b->'Buttons', '[]'::jsonb)) AS nb(elem)
              WHERE lower(COALESCE(b->>'key', b->>'Key')) = 'batchspecimen'
                AND lower(COALESCE(elem->>'key', elem->>'Key')) = 'batchaddtag'
          )
    ),
    new_buttons AS (
        SELECT cnt.id,
               cnt.btn_key,
               jsonb_agg(
                   CASE
                       WHEN lower(COALESCE(b->>'key', b->>'Key')) = 'batchspecimen' THEN
                           CASE
                               WHEN b->'buttons' IS NOT NULL THEN
                                   jsonb_set(b, '{buttons}', (b->'buttons') || (SELECT btn FROM batch_add_tag_btn))
                               ELSE
                                   jsonb_set(b, '{Buttons}', COALESCE(b->'Buttons', '[]'::jsonb) || (SELECT btn FROM batch_add_tag_btn))
                               END
                       ELSE b
                   END
               ) AS updated_buttons
        FROM configs_needing_batchaddtag cnt
        CROSS JOIN LATERAL jsonb_array_elements(cnt.buttons) AS b
        GROUP BY cnt.id, cnt.btn_key
    )
UPDATE configs c
SET contents = jsonb_set(c.contents, ARRAY['buttons'], nb.updated_buttons)
FROM new_buttons nb
WHERE c.id = nb.id
  AND nb.btn_key = 'buttons';

-- Handle PascalCase Buttons
WITH
    batch_add_tag_btn AS (
        SELECT '{"key": "batchaddtag", "text": "@SpeBatI@", "onBatch": true, "icon": "Tag", "uievent": "batchaddspecimentaguievent", "onFinish": "refresh"}'::jsonb AS btn
    ),
    configs_needing_batchaddtag AS (
        SELECT c.id,
               c.contents->'Buttons' AS buttons
        FROM configs c
        WHERE c.configname = 'specimens'
          AND c.contents IS NOT NULL
          AND c.contents != '{}'::jsonb
          AND c.contents->'Buttons' IS NOT NULL
          AND EXISTS (
              SELECT 1
              FROM jsonb_array_elements(c.contents->'Buttons') AS b
              WHERE lower(COALESCE(b->>'key', b->>'Key')) = 'batchspecimen'
          )
          AND NOT EXISTS (
              SELECT 1
              FROM jsonb_array_elements(c.contents->'Buttons') AS b
              CROSS JOIN LATERAL jsonb_array_elements(COALESCE(b->'buttons', b->'Buttons', '[]'::jsonb)) AS nb(elem)
              WHERE lower(COALESCE(b->>'key', b->>'Key')) = 'batchspecimen'
                AND lower(COALESCE(elem->>'key', elem->>'Key')) = 'batchaddtag'
          )
    ),
    new_buttons AS (
        SELECT cnt.id,
               jsonb_agg(
                   CASE
                       WHEN lower(COALESCE(b->>'key', b->>'Key')) = 'batchspecimen' THEN
                           CASE
                               WHEN b->'buttons' IS NOT NULL THEN
                                   jsonb_set(b, '{buttons}', (b->'buttons') || (SELECT btn FROM batch_add_tag_btn))
                               ELSE
                                   jsonb_set(b, '{Buttons}', COALESCE(b->'Buttons', '[]'::jsonb) || (SELECT btn FROM batch_add_tag_btn))
                               END
                       ELSE b
                   END
               ) AS updated_buttons
        FROM configs_needing_batchaddtag cnt
        CROSS JOIN LATERAL jsonb_array_elements(cnt.buttons) AS b
        GROUP BY cnt.id
    )
UPDATE configs c
SET contents = jsonb_set(c.contents, ARRAY['Buttons'], nb.updated_buttons)
FROM new_buttons nb
WHERE c.id = nb.id;
