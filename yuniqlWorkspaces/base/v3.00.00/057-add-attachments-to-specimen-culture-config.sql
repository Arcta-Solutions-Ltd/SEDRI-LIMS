-- Add FileAttachmentIds field to specimenforspecimenview query config when it exists in ConfigList
-- so attachments display on the specimen record view.
WITH
    fileattachmentids_field AS (
        SELECT '{"Name": "FileAttachmentIds", "Type": "specimenfileattachmentids", "KnownAs": "fileattachmentids"}'::jsonb AS field
    ),
    configs_needing_field AS (
        SELECT c.id, c.contents
        FROM configs c
        WHERE c.configname = 'specimenforspecimenview'
          AND c.contents IS NOT NULL
          AND c.contents != '{}'::jsonb
          AND NOT EXISTS (
              SELECT 1
              FROM jsonb_array_elements(c.contents->'Fields') AS existing
              WHERE lower(existing->>'Name') = 'fileattachmentids'
          )
    )
UPDATE configs c
SET contents = jsonb_set(
    c.contents,
    '{Fields}',
    (c.contents->'Fields') || (SELECT field FROM fileattachmentids_field)
)
FROM configs_needing_field cnt
WHERE c.id = cnt.id;

-- Add fileattachmentids rule to specimenviewmapper when it exists in ConfigList
WITH
    fileattachmentids_rule AS (
        SELECT '{"Key": "<:42:>", "Type": "Mapping", "Source": "fileattachmentids", "Value": "fileattachmentids"}'::jsonb AS rule
    ),
    configs_needing_rule AS (
        SELECT c.id, c.contents
        FROM configs c
        WHERE c.configname = 'specimenviewmapper'
          AND c.contents IS NOT NULL
          AND c.contents != '{}'::jsonb
          AND NOT EXISTS (
              SELECT 1
              FROM jsonb_array_elements(c.contents->'Rules') AS r
              WHERE (r->>'Key') = '<:42:>'
          )
    )
UPDATE configs c
SET contents = jsonb_set(
    c.contents,
    '{Rules}',
    (c.contents->'Rules') || (SELECT rule FROM fileattachmentids_rule)
)
FROM configs_needing_rule cnt
WHERE c.id = cnt.id;

-- Add attachments section to specimenviewmapper Target Sections when it exists in ConfigList
DO $$
DECLARE
    r RECORD;
    section_count int;
    attachments_section jsonb;
BEGIN
    attachments_section := '{"Id": "attachments", "Title": "@GenAtts@", "Fields": [{"Id": "fileattachmentids", "Label": "", "Type": "upload", "Value": "<:42:>"}]}'::jsonb;

    FOR r IN
        SELECT c.id, c.contents
        FROM configs c
        WHERE c.configname = 'specimenviewmapper'
          AND c.contents IS NOT NULL
          AND c.contents != '{}'::jsonb
    LOOP
        IF NOT EXISTS (
            SELECT 1 FROM jsonb_array_elements(r.contents->'Target'->'Sections') AS s
            WHERE (s->>'Id') = 'attachments'
        ) THEN
            UPDATE configs
            SET contents = jsonb_set(
                contents,
                '{Target,Sections}',
                (contents->'Target'->'Sections') || attachments_section
            )
            WHERE id = r.id;
        END IF;
    END LOOP;
END $$;
