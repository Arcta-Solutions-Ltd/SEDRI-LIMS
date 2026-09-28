-- Add Tags field to patientforpatientview query config when it exists in ConfigList
-- so tags display on the patient record view (patientdetails section).
WITH
    tags_field AS (
        SELECT '{"Name": "Tags", "Type": "patienttags", "KnownAs": "tags"}'::jsonb AS field
    ),
    configs_needing_tags AS (
        SELECT c.id, c.contents
        FROM configs c
        WHERE c.configname = 'patientforpatientview'
          AND c.contents IS NOT NULL
          AND c.contents != '{}'::jsonb
          AND NOT EXISTS (
              SELECT 1
              FROM jsonb_array_elements(c.contents->'Fields') AS existing
              WHERE lower(existing->>'Name') = 'tags'
          )
    )
UPDATE configs c
SET contents = jsonb_set(
    c.contents,
    '{Fields}',
    (c.contents->'Fields') || (SELECT field FROM tags_field)
)
FROM configs_needing_tags cnt
WHERE c.id = cnt.id;

-- Add tags rule and display field to patientviewmapper when it exists in ConfigList
-- Rule: { "Key": "<:15:>", "Type": "Mapping", "Source": "tags", "Value": "tags" }
-- Target: add { "Id": "tags", "Label": "@GenTagA@", "Value": "<:15:>" } to patientdetails section Fields
WITH
    tags_rule AS (
        SELECT '{"Key": "<:15:>", "Type": "Mapping", "Source": "tags", "Value": "tags"}'::jsonb AS rule
    ),
    configs_needing_rule AS (
        SELECT c.id, c.contents
        FROM configs c
        WHERE c.configname = 'patientviewmapper'
          AND c.contents IS NOT NULL
          AND c.contents != '{}'::jsonb
          AND NOT EXISTS (
              SELECT 1
              FROM jsonb_array_elements(c.contents->'Rules') AS r
              WHERE (r->>'Key') = '<:15:>'
          )
    )
UPDATE configs c
SET contents = jsonb_set(
    c.contents,
    '{Rules}',
    (c.contents->'Rules') || (SELECT rule FROM tags_rule)
)
FROM configs_needing_rule cnt
WHERE c.id = cnt.id;

-- Add tags field to patientdetails section in patientviewmapper Target
-- (patientdetails has no SubSections; Fields are on the section directly)
DO $$
DECLARE
    r RECORD;
    section_idx int;
    new_fields jsonb;
BEGIN
    FOR r IN
        SELECT c.id, c.contents
        FROM configs c
        WHERE c.configname = 'patientviewmapper'
          AND c.contents IS NOT NULL
          AND c.contents != '{}'::jsonb
    LOOP
        SELECT s.ord - 1 INTO section_idx
        FROM jsonb_array_elements(r.contents->'Target'->'Sections') WITH ORDINALITY AS s(section, ord)
        WHERE (s.section->>'Id') = 'patientdetails'
        LIMIT 1;

        IF section_idx IS NOT NULL THEN
            IF NOT EXISTS (
                SELECT 1 FROM jsonb_array_elements(
                    r.contents->'Target'->'Sections'->section_idx->'Fields'
                ) AS f WHERE (f->>'Id') = 'tags'
            ) THEN
                new_fields := COALESCE(
                    r.contents->'Target'->'Sections'->section_idx->'Fields',
                    '[]'::jsonb
                ) || '{"Id": "tags", "Label": "@GenTagA@", "Value": "<:15:>"}'::jsonb;

                UPDATE configs
                SET contents = jsonb_set(
                    contents,
                    array['Target', 'Sections', section_idx::text, 'Fields'],
                    new_fields
                )
                WHERE id = r.id;
            END IF;
        END IF;
    END LOOP;
END $$;
