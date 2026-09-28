-- Lock crafted organism workflow pages in Define Page Contents and align page group anchors.
-- Patches stored page overrides in place (ConfigTypeId 9).
--
-- Organism group anchors (forms that include cultureorganismpage):
--   cultureorganismpage (1), selectorganismpage (2), organismlistpage (3)
-- Forms with only select + list use anchors 2 and 3 present on the form.

-- ---------------------------------------------------------------------------
-- 1. configureActions: nofields on organism workflow pages
-- ---------------------------------------------------------------------------

UPDATE configs c
SET contents = (
        CASE
            WHEN c.contents ? 'ConfigureActions' THEN c.contents - 'ConfigureActions'
            ELSE c.contents
        END
    ) || jsonb_build_object('configureActions', 'nofields'),
    lastmodifieddate = now()
WHERE c.configtypeid = 9
  AND LOWER(c.configname) IN (
        'cultureorganismpage',
        'selectorganismpage',
        'organismlistpage'
      )
  AND c.contents IS NOT NULL
  AND jsonb_typeof(c.contents) = 'object'
  AND c.contents != '{}'::jsonb
  AND LOWER(TRIM(COALESCE(c.contents->>'configureActions', c.contents->>'ConfigureActions', '')))
      IS DISTINCT FROM 'nofields';

-- ---------------------------------------------------------------------------
-- 2. pageGroup and groupAnchor on organism pages
-- ---------------------------------------------------------------------------

UPDATE configs c
SET contents = (
        (c.contents - 'PageGroup' - 'pageGroup' - 'GroupAnchor' - 'groupAnchor')
        || jsonb_build_object('pageGroup', 'organism', 'groupAnchor', 1)
    ),
    lastmodifieddate = now()
WHERE c.configtypeid = 9
  AND LOWER(c.configname) = 'cultureorganismpage'
  AND c.contents IS NOT NULL
  AND jsonb_typeof(c.contents) = 'object'
  AND c.contents != '{}'::jsonb
  AND (
        LOWER(COALESCE(c.contents->>'pageGroup', c.contents->>'PageGroup', '')) IS DISTINCT FROM 'organism'
        OR COALESCE(
            NULLIF(c.contents->>'groupAnchor', '')::int,
            NULLIF(c.contents->>'GroupAnchor', '')::int,
            -1
        ) IS DISTINCT FROM 1
      );

UPDATE configs c
SET contents = (
        (c.contents - 'PageGroup' - 'pageGroup' - 'GroupAnchor' - 'groupAnchor')
        || jsonb_build_object('pageGroup', 'organism', 'groupAnchor', 2)
    ),
    lastmodifieddate = now()
WHERE c.configtypeid = 9
  AND LOWER(c.configname) = 'selectorganismpage'
  AND c.contents IS NOT NULL
  AND jsonb_typeof(c.contents) = 'object'
  AND c.contents != '{}'::jsonb
  AND (
        LOWER(COALESCE(c.contents->>'pageGroup', c.contents->>'PageGroup', '')) IS DISTINCT FROM 'organism'
        OR COALESCE(
            NULLIF(c.contents->>'groupAnchor', '')::int,
            NULLIF(c.contents->>'GroupAnchor', '')::int,
            -1
        ) IS DISTINCT FROM 2
      );

UPDATE configs c
SET contents = (
        (c.contents - 'PageGroup' - 'pageGroup' - 'GroupAnchor' - 'groupAnchor')
        || jsonb_build_object('pageGroup', 'organism', 'groupAnchor', 3)
    ),
    lastmodifieddate = now()
WHERE c.configtypeid = 9
  AND LOWER(c.configname) = 'organismlistpage'
  AND c.contents IS NOT NULL
  AND jsonb_typeof(c.contents) = 'object'
  AND c.contents != '{}'::jsonb
  AND (
        LOWER(COALESCE(c.contents->>'pageGroup', c.contents->>'PageGroup', '')) IS DISTINCT FROM 'organism'
        OR COALESCE(
            NULLIF(c.contents->>'groupAnchor', '')::int,
            NULLIF(c.contents->>'GroupAnchor', '')::int,
            -1
        ) IS DISTINCT FROM 3
      );

-- ---------------------------------------------------------------------------
-- 3. Lock organism identity field ids on cultureorganismpage
-- ---------------------------------------------------------------------------

WITH
    pages_with_locked_fields AS (
        SELECT
            c.id,
            c.configname,
            COALESCE(c.contents -> 'columns', c.contents -> 'Columns') AS columns_arr,
            CASE
                WHEN c.contents ? 'columns' THEN 'columns'
                ELSE 'Columns'
            END AS columns_key
        FROM configs c
        WHERE c.configtypeid = 9
          AND LOWER(c.configname) = 'cultureorganismpage'
          AND c.contents IS NOT NULL
          AND c.contents != '{}'::jsonb
          AND jsonb_typeof(COALESCE(c.contents -> 'columns', c.contents -> 'Columns')) = 'array'
    ),
    lock_field_ids AS (
        SELECT *
        FROM (VALUES
            ('cultureorganismpage', 'organismid'),
            ('cultureorganismpage', 'organismcodeid')
        ) AS t(page_name, field_id)
    ),
    updated_columns AS (
        SELECT
            p.id,
            p.columns_key,
            jsonb_agg(
                CASE
                    WHEN jsonb_typeof(col) = 'object' THEN
                        jsonb_set(
                            col,
                            CASE
                                WHEN col ? 'formGroups' THEN ARRAY['formGroups']
                                ELSE ARRAY['FormGroups']
                            END,
                            (
                                SELECT jsonb_agg(
                                    CASE
                                        WHEN jsonb_typeof(fg) = 'object' THEN
                                            jsonb_set(
                                                fg,
                                                CASE
                                                    WHEN fg ? 'fields' THEN ARRAY['fields']
                                                    ELSE ARRAY['Fields']
                                                END,
                                                (
                                                    SELECT COALESCE(
                                                        jsonb_agg(
                                                            CASE
                                                                WHEN EXISTS (
                                                                    SELECT 1
                                                                    FROM lock_field_ids l
                                                                    WHERE l.page_name = LOWER(p.configname)
                                                                      AND l.field_id = lower(COALESCE(f ->> 'id', f ->> 'Id'))
                                                                      AND lower(COALESCE(f ->> 'Configurable', f ->> 'configurable', '')) IS DISTINCT FROM 'no'
                                                                )
                                                                THEN (f - 'Configurable' - 'configurable') || '{"Configurable": "No"}'::jsonb
                                                                ELSE f
                                                            END
                                                        ),
                                                        '[]'::jsonb
                                                    )
                                                    FROM jsonb_array_elements(
                                                        COALESCE(fg -> 'fields', fg -> 'Fields', '[]'::jsonb)
                                                    ) AS f
                                                )
                                            )
                                        ELSE fg
                                    END
                                )
                                FROM jsonb_array_elements(
                                    COALESCE(col -> 'formGroups', col -> 'FormGroups', '[]'::jsonb)
                                ) AS fg
                            )
                        )
                    ELSE col
                END
            ) AS new_columns
        FROM pages_with_locked_fields p,
             jsonb_array_elements(p.columns_arr) AS col
        GROUP BY p.id, p.columns_key
    )
UPDATE configs c
SET contents = (
        CASE
            WHEN u.columns_key = 'columns' THEN c.contents - 'Columns'
            ELSE c.contents - 'columns'
        END
    ) || jsonb_build_object(u.columns_key, u.new_columns),
    lastmodifieddate = now()
FROM updated_columns u
WHERE c.id = u.id;
