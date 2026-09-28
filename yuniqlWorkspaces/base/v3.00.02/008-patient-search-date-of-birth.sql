-- Add DateOfBirthSearch to workflow patient search configs when stored in configs table (DB overrides).
-- Idempotent: no-op when field/mapper/query changes already present.

-- patientsearchpage: append DateOfBirthSearch field and add to required OR list
WITH
    dob_field AS (
        SELECT '{"id": "DateOfBirthSearch", "type": "date", "label": "@PatDat@", "placeholder": "@PatSel@", "Min": "now y-130", "Max": "now", "Configurable": "No"}'::jsonb AS field_elem
    ),
    pages_needing_dob AS (
        SELECT c.id,
               COALESCE(
                   c.contents->'columns'->0->'formGroups'->0->'fields',
                   c.contents->'Columns'->0->'FormGroups'->0->'Fields'
               ) AS fields_arr,
               CASE
                   WHEN c.contents->'columns' IS NOT NULL THEN 'columns'
                   ELSE 'Columns'
               END AS columns_key,
               CASE
                   WHEN c.contents->'columns'->0->'formGroups' IS NOT NULL THEN 'formGroups'
                   ELSE 'FormGroups'
               END AS form_groups_key,
               CASE
                   WHEN c.contents->'columns'->0->'formGroups'->0->'fields' IS NOT NULL THEN 'fields'
                   ELSE 'Fields'
               END AS fields_key
        FROM configs c
        WHERE lower(c.configname) = 'patientsearchpage'
          AND c.configtypeid = 9
          AND c.contents IS NOT NULL
          AND c.contents != '{}'::jsonb
          AND NOT EXISTS (
              SELECT 1
              FROM jsonb_array_elements(
                  COALESCE(
                      c.contents->'columns'->0->'formGroups'->0->'fields',
                      c.contents->'Columns'->0->'FormGroups'->0->'Fields',
                      '[]'::jsonb
                  )
              ) AS elem
              WHERE lower(COALESCE(elem->>'id', elem->>'Id')) = 'dateofbirthsearch'
          )
    )
UPDATE configs c
SET contents = jsonb_set(
    CASE
        WHEN cnt.columns_key = 'columns' THEN
            jsonb_set(
                c.contents,
                ARRAY['columns', '0', 'formGroups', '0', 'fields'],
                (cnt.fields_arr || (SELECT field_elem FROM dob_field))
            )
        ELSE
            jsonb_set(
                c.contents,
                ARRAY['Columns', '0', 'FormGroups', '0', 'Fields'],
                (cnt.fields_arr || (SELECT field_elem FROM dob_field))
            )
    END,
    ARRAY['required'],
    to_jsonb(
        COALESCE(c.contents->>'required', c.contents->>'Required', '')
        || CASE
            WHEN COALESCE(c.contents->>'required', c.contents->>'Required', '') LIKE '%DateOfBirthSearch%' THEN ''
            ELSE ',DateOfBirthSearch'
        END
    )
)
FROM pages_needing_dob cnt
WHERE c.id = cnt.id;

-- patientsearchparametermapper: append DateOfBirthSearch -> startdate/enddate rules
WITH
    mapper_needing_dob AS (
        SELECT c.id, c.contents
        FROM configs c
        WHERE lower(c.configname) = 'patientsearchparametermapper'
          AND c.configtypeid = 10
          AND c.contents IS NOT NULL
          AND c.contents != '{}'::jsonb
          AND c.contents::text NOT LIKE '%DateOfBirthSearch%'
    )
UPDATE configs c
SET contents = replace(
    replace(
        c.contents::text,
        '''AgeToDays'', ''Value'': ''<:10:>''}',
        '''AgeToDays'', ''Value'': ''<:10:>''}, { Key: ''<:11:>'', Type: ''Mapping'', Source: ''Key'', Where: ''DateOfBirthSearch'', Value: ''Value''}, { Key: ''<:12:>'', Type: ''Mapping'', Source: ''Key'', Where: ''DateOfBirthSearch'', Value: ''Value''}'
    ),
    '''AgeToDays'', ''Value'': ''<:10:>''} ]',
    '''AgeToDays'', ''Value'': ''<:10:>''}, {''Key'': ''startdate'', ''Value'': ''<:11:>''}, {''Key'': ''enddate'', ''Value'': ''<:12:>''} ]'
)::jsonb
FROM mapper_needing_dob m
WHERE c.id = m.id;

-- patientsearch query: append DateOfBirth daterange WHERE clause
WITH
    query_needing_dob AS (
        SELECT c.id, c.contents
        FROM configs c
        WHERE lower(c.configname) = 'patientsearch'
          AND c.configtypeid = 8
          AND c.contents IS NOT NULL
          AND c.contents != '{}'::jsonb
          AND NOT EXISTS (
              SELECT 1
              FROM jsonb_array_elements(
                  COALESCE(c.contents->'Where', c.contents->'where', '[]'::jsonb)
              ) AS elem
              WHERE lower(COALESCE(elem->>'Field', elem->>'field')) = 'dateofbirth'
                AND lower(COALESCE(elem->>'Comparison', elem->>'comparison')) = 'daterange'
          )
    )
UPDATE configs c
SET contents = replace(
    c.contents::text,
    '''AgeFromYears'', ''Comparison'': ''agerange'' }',
    '''AgeFromYears'', ''Comparison'': ''agerange'' }, {''Field'': ''DateOfBirth'', ''Comparison'': ''daterange'' }'
)::jsonb
FROM query_needing_dob q
WHERE c.id = q.id;
