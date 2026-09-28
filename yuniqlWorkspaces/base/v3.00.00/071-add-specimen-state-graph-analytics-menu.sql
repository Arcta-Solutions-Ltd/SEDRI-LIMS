-- Specimen state graph: UI event for Analytics menu, and nested button on graphs view when stored in configs.
-- Code defaults come from GraphViewConfig / SpecimenStateGraphUIEventConfig; this script updates existing DB rows.

INSERT INTO configs (configname, configtypeid, contents, lastmodifieddate)
SELECT 'specimenstategraphuievent',
       6,
       '{"Name": "specimenstategraphuievent", "Type": "graph", "Action": "specimenstategraph", "Description": "Specimen State Graph"}'::jsonb,
       now()
WHERE NOT EXISTS (SELECT 1 FROM configs WHERE configname = 'specimenstategraphuievent');


-- Append specimen state item to specimengraphs submenu (lowercase buttons)
WITH
    new_sub_btn AS (
        SELECT '{"key": "specimenstate", "text": "@SpeStaA@", "icon": "PieSingle", "uievent": "specimenstategraphuievent"}'::jsonb AS btn
    ),
    configs_needing AS (
        SELECT c.id,
               c.contents->'buttons' AS top_buttons
        FROM configs c
        WHERE c.configname = 'graphs'
          AND c.contents IS NOT NULL
          AND c.contents != '{}'::jsonb
          AND c.contents->'buttons' IS NOT NULL
          AND EXISTS (
              SELECT 1
              FROM jsonb_array_elements(c.contents->'buttons') AS b
              WHERE lower(COALESCE(b->>'key', b->>'Key')) = 'specimengraphs'
          )
          AND NOT EXISTS (
              SELECT 1
              FROM jsonb_array_elements(c.contents->'buttons') AS b
              CROSS JOIN LATERAL jsonb_array_elements(COALESCE(b->'buttons', b->'Buttons', '[]'::jsonb)) AS nb(elem)
              WHERE lower(COALESCE(b->>'key', b->>'Key')) = 'specimengraphs'
                AND lower(COALESCE(elem->>'key', elem->>'Key')) = 'specimenstate'
          )
    ),
    new_top AS (
        SELECT cn.id,
               jsonb_agg(
                   CASE
                       WHEN lower(COALESCE(tb.val->>'key', tb.val->>'Key')) = 'specimengraphs' THEN
                           CASE
                               WHEN tb.val->'buttons' IS NOT NULL THEN
                                   jsonb_set(tb.val, '{buttons}', (tb.val->'buttons') || (SELECT btn FROM new_sub_btn))
                               ELSE
                                   jsonb_set(tb.val, '{Buttons}', COALESCE(tb.val->'Buttons', '[]'::jsonb) || (SELECT btn FROM new_sub_btn))
                           END
                       ELSE tb.val
                   END
                   ORDER BY tb.ord
               ) AS updated_top
        FROM configs_needing cn
        CROSS JOIN LATERAL jsonb_array_elements(cn.top_buttons) WITH ORDINALITY AS tb(val, ord)
        GROUP BY cn.id
    )
UPDATE configs c
SET contents = jsonb_set(c.contents, '{buttons}', nt.updated_top),
    lastmodifieddate = now()
FROM new_top nt
WHERE c.id = nt.id;


-- PascalCase Buttons on graphs view
WITH
    new_sub_btn AS (
        SELECT '{"key": "specimenstate", "text": "@SpeStaA@", "icon": "PieSingle", "uievent": "specimenstategraphuievent"}'::jsonb AS btn
    ),
    configs_needing AS (
        SELECT c.id,
               c.contents->'Buttons' AS top_buttons
        FROM configs c
        WHERE c.configname = 'graphs'
          AND c.contents IS NOT NULL
          AND c.contents != '{}'::jsonb
          AND c.contents->'Buttons' IS NOT NULL
          AND EXISTS (
              SELECT 1
              FROM jsonb_array_elements(c.contents->'Buttons') AS b
              WHERE lower(COALESCE(b->>'key', b->>'Key')) = 'specimengraphs'
          )
          AND NOT EXISTS (
              SELECT 1
              FROM jsonb_array_elements(c.contents->'Buttons') AS b
              CROSS JOIN LATERAL jsonb_array_elements(COALESCE(b->'buttons', b->'Buttons', '[]'::jsonb)) AS nb(elem)
              WHERE lower(COALESCE(b->>'key', b->>'Key')) = 'specimengraphs'
                AND lower(COALESCE(elem->>'key', elem->>'Key')) = 'specimenstate'
          )
    ),
    new_top AS (
        SELECT cn.id,
               jsonb_agg(
                   CASE
                       WHEN lower(COALESCE(tb.val->>'key', tb.val->>'Key')) = 'specimengraphs' THEN
                           CASE
                               WHEN tb.val->'buttons' IS NOT NULL THEN
                                   jsonb_set(tb.val, '{buttons}', (tb.val->'buttons') || (SELECT btn FROM new_sub_btn))
                               ELSE
                                   jsonb_set(tb.val, '{Buttons}', COALESCE(tb.val->'Buttons', '[]'::jsonb) || (SELECT btn FROM new_sub_btn))
                           END
                       ELSE tb.val
                   END
                   ORDER BY tb.ord
               ) AS updated_top
        FROM configs_needing cn
        CROSS JOIN LATERAL jsonb_array_elements(cn.top_buttons) WITH ORDINALITY AS tb(val, ord)
        GROUP BY cn.id
    )
UPDATE configs c
SET contents = jsonb_set(c.contents, '{Buttons}', nt.updated_top),
    lastmodifieddate = now()
FROM new_top nt
WHERE c.id = nt.id;
