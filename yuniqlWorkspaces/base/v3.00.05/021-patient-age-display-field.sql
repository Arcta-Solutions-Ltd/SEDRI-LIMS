-- Patient age display: calculated read-only field after DateOfBirth on patient pages,
-- and updated patient/specimen record view mappers. Idempotent.

-- ---------------------------------------------------------------------------
-- 1. Page configs: insert PatientAgeDisplay after DateOfBirth
-- ---------------------------------------------------------------------------

CREATE OR REPLACE FUNCTION pg_temp.insert_field_after_dob(
    fields jsonb,
    new_field jsonb,
    after_field_id text
) RETURNS jsonb AS $$
DECLARE
    result jsonb := '[]'::jsonb;
    elem jsonb;
    field_id text;
    new_field_id text := lower(COALESCE(new_field->>'id', new_field->>'Id', ''));
BEGIN
    IF fields IS NULL OR jsonb_typeof(fields) != 'array' THEN
        RETURN fields;
    END IF;

    IF EXISTS (
        SELECT 1 FROM jsonb_array_elements(fields) f
        WHERE lower(COALESCE(f->>'id', f->>'Id', '')) = new_field_id
    ) THEN
        RETURN (
            SELECT jsonb_agg(
                CASE
                    WHEN lower(COALESCE(e->>'id', e->>'Id', '')) = new_field_id THEN new_field
                    ELSE e
                END
            )
            FROM jsonb_array_elements(fields) e
        );
    END IF;

    FOR elem IN SELECT * FROM jsonb_array_elements(fields)
    LOOP
        result := result || jsonb_build_array(elem);
        field_id := lower(COALESCE(elem->>'id', elem->>'Id', ''));
        IF field_id = lower(after_field_id) THEN
            IF NOT EXISTS (
                SELECT 1 FROM jsonb_array_elements(result || jsonb_build_array(new_field)) f
                WHERE lower(COALESCE(f->>'id', f->>'Id')) = lower(COALESCE(new_field->>'id', new_field->>'Id'))
            ) THEN
                result := result || jsonb_build_array(new_field);
            END IF;
        END IF;
    END LOOP;

    IF NOT EXISTS (
        SELECT 1 FROM jsonb_array_elements(result) f
        WHERE lower(COALESCE(f->>'id', f->>'Id')) = lower(COALESCE(new_field->>'id', new_field->>'Id'))
    ) THEN
        result := result || jsonb_build_array(new_field);
    END IF;

    RETURN result;
END;
$$ LANGUAGE plpgsql;

WITH
    age_display_field AS (
        SELECT '{"id": "PatientAgeDisplay", "type": "singleline", "label": "@PatAge@", "ReadOnly": true, "Configurable": "No"}'::jsonb AS field_elem
    ),
    target_pages AS (
        SELECT c.id,
               c.configname,
               COALESCE(c.contents->'columns', c.contents->'Columns') AS columns_arr,
               CASE WHEN c.contents ? 'columns' THEN 'columns' ELSE 'Columns' END AS columns_key
        FROM configs c
        WHERE c.configtypeid = 9
          AND lower(c.configname) IN ('patientdetailspage', 'editpatientdetailspage', 'neoshieldbirthdetailspage')
          AND c.contents IS NOT NULL
          AND c.contents != '{}'::jsonb
    ),
    updated AS (
        SELECT
            tp.id,
            tp.columns_key,
            jsonb_agg(
                CASE
                    WHEN jsonb_typeof(col) = 'object' THEN
                        jsonb_set(
                            col,
                            CASE WHEN col ? 'formGroups' THEN ARRAY['formGroups'] ELSE ARRAY['FormGroups'] END,
                            (
                                SELECT jsonb_agg(
                                    CASE
                                        WHEN jsonb_typeof(fg) = 'object'
                                             AND (
                                                 lower(tp.configname) IN ('patientdetailspage', 'editpatientdetailspage')
                                                 AND lower(COALESCE(fg->>'key', fg->>'Key', '')) = 'fg1'
                                             OR lower(tp.configname) = 'neoshieldbirthdetailspage'
                                                 AND lower(COALESCE(fg->>'key', fg->>'Key', '')) = 'fg2'
                                             )
                                        THEN
                                            jsonb_set(
                                                fg,
                                                CASE WHEN fg ? 'fields' THEN ARRAY['fields'] ELSE ARRAY['Fields'] END,
                                                pg_temp.insert_field_after_dob(
                                                    COALESCE(fg->'fields', fg->'Fields'),
                                                    (SELECT field_elem FROM age_display_field),
                                                    'DateOfBirth'
                                                )
                                            )
                                        ELSE fg
                                    END
                                )
                                FROM jsonb_array_elements(COALESCE(col->'formGroups', col->'FormGroups', '[]'::jsonb)) fg
                            )
                        )
                    ELSE col
                END
            ) AS new_columns
        FROM target_pages tp
        CROSS JOIN LATERAL jsonb_array_elements(tp.columns_arr) col
        GROUP BY tp.id, tp.columns_key
    )
UPDATE configs c
SET contents = jsonb_set(c.contents, ARRAY[u.columns_key], u.new_columns),
    lastmodifieddate = now()
FROM updated u
WHERE c.id = u.id;

-- ---------------------------------------------------------------------------
-- 2. SpecimenForSpecimenView query: ensure Patient.DateOfBirth is joined
-- ---------------------------------------------------------------------------

UPDATE configs c
SET contents = jsonb_set(
        c.contents,
        '{Joins}',
        (
            SELECT jsonb_agg(
                CASE
                    WHEN lower(elem->>'Table') = 'patient' THEN
                        jsonb_set(
                            elem,
                            '{Fields}',
                            (
                                SELECT COALESCE(jsonb_agg(DISTINCT f), '[]'::jsonb)
                                FROM (
                                    SELECT jsonb_array_elements(COALESCE(elem->'Fields', '[]'::jsonb)) AS f
                                    UNION ALL
                                    SELECT '{"Name": "DateOfBirth"}'::jsonb
                                ) sub
                            )
                        )
                    ELSE elem
                END
            )
            FROM jsonb_array_elements(c.contents->'Joins') elem
        )
    ),
    lastmodifieddate = now()
WHERE lower(c.configname) = 'specimenforspecimenview'
  AND c.configtypeid = 8
  AND c.contents IS NOT NULL
  AND c.contents ? 'Joins'
  AND NOT EXISTS (
      SELECT 1
      FROM jsonb_array_elements(c.contents->'Joins') j
      CROSS JOIN LATERAL jsonb_array_elements(COALESCE(j->'Fields', '[]'::jsonb)) f
      WHERE lower(j->>'Table') = 'patient'
        AND lower(f->>'Name') = 'dateofbirth'
  );

-- ---------------------------------------------------------------------------
-- 3. patientviewmapper: replace ageyears/agemonths with patientagedisplay
-- ---------------------------------------------------------------------------

UPDATE configs c
SET contents = regexp_replace(
        regexp_replace(
            c.contents::text,
            '\{[^}]*"Id"\s*:\s*''ageyears''[^}]*\},?\s*',
            '',
            'gi'
        ),
        '\{[^}]*"Id"\s*:\s*''agemonths''[^}]*\},?\s*',
        '',
        'gi'
    )::jsonb,
    lastmodifieddate = now()
WHERE lower(c.configname) = 'patientviewmapper'
  AND c.configtypeid = 10
  AND c.contents::text ILIKE '%ageyears%';

UPDATE configs c
SET contents = regexp_replace(
        c.contents::text,
        '(\{[^}]*"Id"\s*:\s*''dateofbirth''[^}]*\})',
        '\1, { Id: ''patientagedisplay'', Label: ''@PatAge@'', Value: ''<:16:>'' }',
        'gi'
    )::jsonb,
    lastmodifieddate = now()
WHERE lower(c.configname) = 'patientviewmapper'
  AND c.configtypeid = 10
  AND c.contents::text NOT ILIKE '%patientagedisplay%';

-- ---------------------------------------------------------------------------
-- 4. specimenviewmapper: replace ageatspecimen with patientageatspecimen
-- ---------------------------------------------------------------------------

UPDATE configs c
SET contents = regexp_replace(
        c.contents::text,
        '\{[^}]*"Id"\s*:\s*''ageatspecimen''[^}]*\}',
        '{ Id: ''patientageatspecimen'', Label: ''@SpeAge@'', Value: ''<:68:>'' }',
        'gi'
    )::jsonb,
    lastmodifieddate = now()
WHERE lower(c.configname) = 'specimenviewmapper'
  AND c.configtypeid = 10
  AND c.contents::text ILIKE '%ageatspecimen%';

DROP FUNCTION IF EXISTS pg_temp.insert_field_after_dob(jsonb, jsonb, text);
