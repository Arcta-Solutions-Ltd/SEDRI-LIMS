-- Lock down the Location field on the patient address page so it cannot be edited or deleted
-- in Configuration > Views. Matches the factory definition in PatientAddressPageConfig.

-- ---------------------------------------------------------------------------
-- patientaddresspage: set Configurable = 'No' on LocationId
-- Idempotent: no-op when the field is already non-configurable.
-- ---------------------------------------------------------------------------

WITH
    pages_with_location AS (
        SELECT
            c.id,
            COALESCE(c.contents -> 'columns', c.contents -> 'Columns') AS columns_arr,
            CASE
                WHEN c.contents ? 'columns' THEN 'columns'
                ELSE 'Columns'
            END AS columns_key
        FROM configs c
        WHERE c.configtypeid = 9
          AND lower(c.configname) = 'patientaddresspage'
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
                                                                WHEN lower(COALESCE(f ->> 'id', f ->> 'Id')) = 'locationid'
                                                                     AND lower(COALESCE(f ->> 'Configurable', f ->> 'configurable', '')) IS DISTINCT FROM 'no'
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
        FROM pages_with_location p,
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
WHERE c.id = u.id
  AND EXISTS (
      SELECT 1
      FROM jsonb_array_elements(u.new_columns) AS col,
           jsonb_array_elements(COALESCE(col -> 'formGroups', col -> 'FormGroups', '[]'::jsonb)) AS fg,
           jsonb_array_elements(COALESCE(fg -> 'fields', fg -> 'Fields', '[]'::jsonb)) AS f
      WHERE lower(COALESCE(f ->> 'id', f ->> 'Id')) = 'locationid'
        AND lower(COALESCE(f ->> 'Configurable', f ->> 'configurable', '')) = 'no'
  );
