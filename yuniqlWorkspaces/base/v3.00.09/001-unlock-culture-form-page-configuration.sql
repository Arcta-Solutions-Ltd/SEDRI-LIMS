-- Unlock culture form workflow pages for Define Page Contents configuration.
-- Matches C# factory defaults in arc.app/Config/Pages/Specimen. Config-only: no application code changes.
--
-- Updates stored page overrides in configs (ConfigTypeId 9) in place — systems that already
-- customised these pages via the configuration UI are patched, not replaced wholesale.
--
-- Culture pages affected:
--   specimengrowthdetails, specimenadditionalguidance, specimenotherinformationpage
--
-- Locked fields (Configurable: 'No') on specimengrowthdetails:
--   CultureType, growthid
--
-- Also idempotently re-applies create-specimen page unlock for deployments that missed 001.

-- ---------------------------------------------------------------------------
-- 1. configureActions: normalise to add,edit,delete on culture pages
-- ---------------------------------------------------------------------------

UPDATE configs c
SET contents = (
        CASE
            WHEN c.contents ? 'ConfigureActions' THEN c.contents - 'ConfigureActions'
            ELSE c.contents
        END
    ) || jsonb_build_object('configureActions', 'add,edit,delete'),
    lastmodifieddate = now()
WHERE c.configtypeid = 9
  AND LOWER(c.configname) IN (
        'specimengrowthdetails',
        'specimenadditionalguidance',
        'specimenotherinformationpage'
      )
  AND c.contents IS NOT NULL
  AND jsonb_typeof(c.contents) = 'object'
  AND c.contents != '{}'::jsonb
  AND LOWER(TRIM(COALESCE(c.contents->>'configureActions', c.contents->>'ConfigureActions', '')))
      IS DISTINCT FROM 'add,edit,delete';

-- ---------------------------------------------------------------------------
-- 2. tablename: culture on Growth Details page
-- ---------------------------------------------------------------------------

UPDATE configs c
SET contents = (
        CASE
            WHEN c.contents ? 'TableName' THEN c.contents - 'TableName'
            ELSE c.contents
        END
    ) || jsonb_build_object('tablename', 'culture'),
    lastmodifieddate = now()
WHERE c.configtypeid = 9
  AND LOWER(c.configname) = 'specimengrowthdetails'
  AND c.contents IS NOT NULL
  AND jsonb_typeof(c.contents) = 'object'
  AND c.contents != '{}'::jsonb
  AND LOWER(COALESCE(c.contents->>'tablename', c.contents->>'TableName', '')) IS DISTINCT FROM 'culture';

-- ---------------------------------------------------------------------------
-- 3. Remove Configurable: 'No' from non-locked fields on Growth Details
--    (repairs systems that locked fields that should remain editable)
-- ---------------------------------------------------------------------------

WITH
    pages_to_repair AS (
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
          AND LOWER(c.configname) = 'specimengrowthdetails'
          AND c.contents IS NOT NULL
          AND c.contents != '{}'::jsonb
          AND jsonb_typeof(COALESCE(c.contents -> 'columns', c.contents -> 'Columns')) = 'array'
    ),
    locked_field_ids AS (
        SELECT *
        FROM (VALUES
            ('specimengrowthdetails', 'culturetype'),
            ('specimengrowthdetails', 'growthid')
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
                                                                WHEN lower(COALESCE(f ->> 'Configurable', f ->> 'configurable', '')) = 'no'
                                                                     AND NOT EXISTS (
                                                                         SELECT 1
                                                                         FROM locked_field_ids l
                                                                         WHERE l.page_name = LOWER(p.configname)
                                                                           AND l.field_id = lower(COALESCE(f ->> 'id', f ->> 'Id'))
                                                                     )
                                                                THEN f - 'Configurable' - 'configurable'
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
        FROM pages_to_repair p,
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

-- ---------------------------------------------------------------------------
-- 4. Ensure Configurable: 'No' on locked culture fields (by field id)
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
          AND LOWER(c.configname) = 'specimengrowthdetails'
          AND c.contents IS NOT NULL
          AND c.contents != '{}'::jsonb
          AND jsonb_typeof(COALESCE(c.contents -> 'columns', c.contents -> 'Columns')) = 'array'
    ),
    lock_field_ids AS (
        SELECT *
        FROM (VALUES
            ('specimengrowthdetails', 'culturetype'),
            ('specimengrowthdetails', 'growthid')
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

-- ---------------------------------------------------------------------------
-- 5. Remove Configurable: 'No' from all fields on supplementary culture pages
--    (no locked field ids on these pages)
-- ---------------------------------------------------------------------------

WITH
    pages_to_repair AS (
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
          AND LOWER(c.configname) IN (
                'specimenadditionalguidance',
                'specimenotherinformationpage'
              )
          AND c.contents IS NOT NULL
          AND c.contents != '{}'::jsonb
          AND jsonb_typeof(COALESCE(c.contents -> 'columns', c.contents -> 'Columns')) = 'array'
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
                                                                WHEN lower(COALESCE(f ->> 'Configurable', f ->> 'configurable', '')) = 'no'
                                                                THEN f - 'Configurable' - 'configurable'
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
        FROM pages_to_repair p,
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

-- ---------------------------------------------------------------------------
-- 6. Idempotent create-specimen page unlock (companion to v3.00.05/001)
-- ---------------------------------------------------------------------------

UPDATE configs c
SET contents = (
        CASE
            WHEN c.contents ? 'ConfigureActions' THEN c.contents - 'ConfigureActions'
            ELSE c.contents
        END
    ) || jsonb_build_object('configureActions', 'add,edit,delete'),
    lastmodifieddate = now()
WHERE c.configtypeid = 9
  AND LOWER(c.configname) IN (
        'advancespecimendetailspage',
        'specimentimings',
        'specimentimingsreceived'
      )
  AND c.contents IS NOT NULL
  AND jsonb_typeof(c.contents) = 'object'
  AND c.contents != '{}'::jsonb
  AND LOWER(TRIM(COALESCE(c.contents->>'configureActions', c.contents->>'ConfigureActions', '')))
      IS DISTINCT FROM 'add,edit,delete';

UPDATE configs c
SET contents = (
        CASE
            WHEN c.contents ? 'TableName' THEN c.contents - 'TableName'
            ELSE c.contents
        END
    ) || jsonb_build_object('tablename', 'specimen'),
    lastmodifieddate = now()
WHERE c.configtypeid = 9
  AND LOWER(c.configname) IN (
        'advancespecimendetailspage',
        'specimentimings',
        'specimentimingsreceived'
      )
  AND c.contents IS NOT NULL
  AND jsonb_typeof(c.contents) = 'object'
  AND c.contents != '{}'::jsonb
  AND LOWER(COALESCE(c.contents->>'tablename', c.contents->>'TableName', '')) IS DISTINCT FROM 'specimen';

WITH
    pages_to_unlock AS (
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
          AND LOWER(c.configname) IN (
                'advancespecimendetailspage',
                'specimentimings',
                'specimentimingsreceived'
              )
          AND c.contents IS NOT NULL
          AND c.contents != '{}'::jsonb
          AND jsonb_typeof(COALESCE(c.contents -> 'columns', c.contents -> 'Columns')) = 'array'
    ),
    unlock_field_ids AS (
        SELECT *
        FROM (VALUES
            ('advancespecimendetailspage', 'admissiondate'),
            ('advancespecimendetailspage', 'clinicalcontactno'),
            ('specimentimings', 'collectiondate'),
            ('specimentimings', 'collectiontime'),
            ('specimentimings', 'existingbarcode'),
            ('specimentimings', 'furtherinformation'),
            ('specimentimingsreceived', 'collectiondate'),
            ('specimentimingsreceived', 'collectiontime'),
            ('specimentimingsreceived', 'existingbarcode'),
            ('specimentimingsreceived', 'furtherinformation')
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
                                                                    FROM unlock_field_ids u
                                                                    WHERE u.page_name = LOWER(p.configname)
                                                                      AND u.field_id = lower(COALESCE(f ->> 'id', f ->> 'Id'))
                                                                      AND lower(COALESCE(f ->> 'Configurable', f ->> 'configurable', '')) = 'no'
                                                                )
                                                                THEN f - 'Configurable' - 'configurable'
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
        FROM pages_to_unlock p,
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
          AND LOWER(c.configname) IN (
                'advancespecimendetailspage',
                'specimentimings',
                'specimentimingsreceived'
              )
          AND c.contents IS NOT NULL
          AND c.contents != '{}'::jsonb
          AND jsonb_typeof(COALESCE(c.contents -> 'columns', c.contents -> 'Columns')) = 'array'
    ),
    lock_field_ids AS (
        SELECT *
        FROM (VALUES
            ('advancespecimendetailspage', 'patientref'),
            ('advancespecimendetailspage', 'organisationid'),
            ('advancespecimendetailspage', 'laboratoryid'),
            ('specimentimings', 'specimentypeid'),
            ('specimentimings', 'specimensiteid'),
            ('specimentimings', 'age'),
            ('specimentimingsreceived', 'specimentypeid'),
            ('specimentimingsreceived', 'specimensiteid'),
            ('specimentimingsreceived', 'age')
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
