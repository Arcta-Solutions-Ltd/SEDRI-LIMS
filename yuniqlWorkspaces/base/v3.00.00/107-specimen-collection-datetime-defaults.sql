-- Ensure stored specimen timing page configs default CollectionDate and CollectionTime to now.
-- Sites with empty '{}' rows use C# factory defaults via PageConfigAdapter and are unaffected.

UPDATE configs c
SET contents = updated.new_contents
FROM (
    SELECT
        configname,
        jsonb_set(
            contents,
            '{Columns}',
            (
                SELECT jsonb_agg(
                    jsonb_set(
                        col,
                        '{FormGroups}',
                        (
                            SELECT jsonb_agg(
                                jsonb_set(
                                    fg,
                                    '{Fields}',
                                    (
                                        SELECT jsonb_agg(
                                            CASE
                                                WHEN lower(field ->> 'Id') IN ('collectiondate', 'collectiontime')
                                                    THEN field || '{"DefaultToNow": true}'::jsonb
                                                ELSE field
                                            END
                                        )
                                        FROM jsonb_array_elements(fg -> 'Fields') AS field
                                    )
                                )
                            )
                            FROM jsonb_array_elements(col -> 'FormGroups') AS fg
                        )
                    )
                )
                FROM jsonb_array_elements(contents -> 'Columns') AS col
            )
        ) AS new_contents
    FROM configs
    WHERE configname IN ('specimentimings', 'specimentimingsreceived')
      AND contents IS NOT NULL
      AND contents != '{}'::jsonb
      AND jsonb_typeof(contents -> 'Columns') = 'array'
) AS updated
WHERE c.configname = updated.configname;
