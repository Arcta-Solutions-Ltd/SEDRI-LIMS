-- Set allowAdd on location/org hierarchical picker filters (false) and data-entry form fields (true).
-- Graph configs are code-only; list views and customised page configs in configs.contents are patched here.
-- Idempotent: re-running sets the same allowAdd values.

-- ---------------------------------------------------------------------------
-- 1. Filters: allowAdd false on LocationList / OrganisationList hierarchical pickers
-- ---------------------------------------------------------------------------

WITH
    configs_with_filters AS (
        SELECT
            c.id,
            COALESCE(c.contents -> 'filters', c.contents -> 'Filters') AS filters_arr,
            CASE
                WHEN c.contents ? 'filters' THEN 'filters'
                ELSE 'Filters'
            END AS filter_key
        FROM configs c
        WHERE c.contents IS NOT NULL
          AND c.contents != '{}'::jsonb
          AND jsonb_typeof(COALESCE(c.contents -> 'filters', c.contents -> 'Filters')) = 'array'
    ),
    updated_filters AS (
        SELECT
            cf.id,
            cf.filter_key,
            jsonb_agg(
                CASE
                    WHEN lower(COALESCE(f ->> 'type', f ->> 'Type', '')) = 'hierarchicalpicker'
                         AND lower(COALESCE(f ->> 'optionsName', f ->> 'OptionsName', '')) IN ('locationlist', 'organisationlist')
                    THEN (f - 'allowAdd' - 'AllowAdd') || '{"allowAdd": false}'::jsonb
                    ELSE f
                END
            ) AS new_filters
        FROM configs_with_filters cf,
             jsonb_array_elements(cf.filters_arr) AS f
        GROUP BY cf.id, cf.filter_key
    )
UPDATE configs c
SET contents = (
        CASE
            WHEN u.filter_key = 'filters' THEN c.contents - 'Filters'
            ELSE c.contents - 'filters'
        END
    ) || jsonb_build_object(u.filter_key, u.new_filters),
    lastmodifieddate = now()
FROM updated_filters u
WHERE c.id = u.id;

-- ---------------------------------------------------------------------------
-- 2. Page fields: allowAdd on hierarchical picker location/org fields
--    true  = LocationId, OrganisationId (data entry)
--    false = LocationSearch (search criteria)
-- ---------------------------------------------------------------------------

WITH
    field_allow_add AS (
        SELECT *
        FROM (VALUES
            ('locationid', true),
            ('organisationid', true),
            ('locationsearch', false)
        ) AS t(field_id, allow_add)
    ),
    target_page_names AS (
        SELECT unnest(ARRAY[
            'patientaddresspage',
            'advancespecimendetailspage',
            'runexportpage',
            'exportschedulecriteriapage',
            'patientsearchpage'
        ]) AS configname
    ),
    pages_to_update AS (
        SELECT
            c.id,
            COALESCE(c.contents -> 'columns', c.contents -> 'Columns') AS columns_arr,
            CASE
                WHEN c.contents ? 'columns' THEN 'columns'
                ELSE 'Columns'
            END AS columns_key
        FROM configs c
        INNER JOIN target_page_names tp ON lower(c.configname) = tp.configname
        WHERE c.configtypeid = 9
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
                                                                WHEN EXISTS (
                                                                    SELECT 1
                                                                    FROM field_allow_add fa
                                                                    WHERE fa.field_id = lower(COALESCE(f ->> 'id', f ->> 'Id'))
                                                                      AND lower(COALESCE(f ->> 'type', f ->> 'Type', '')) = 'hierarchicalpicker'
                                                                      AND lower(COALESCE(f ->> 'optionsName', f ->> 'OptionsName', '')) IN ('locationlist', 'organisationlist')
                                                                )
                                                                THEN (f - 'allowAdd' - 'AllowAdd')
                                                                    || jsonb_build_object(
                                                                        'allowAdd',
                                                                        (SELECT fa.allow_add
                                                                         FROM field_allow_add fa
                                                                         WHERE fa.field_id = lower(COALESCE(f ->> 'id', f ->> 'Id'))
                                                                         LIMIT 1)
                                                                    )
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
        FROM pages_to_update p,
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
