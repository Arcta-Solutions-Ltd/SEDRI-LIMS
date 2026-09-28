-- Normalize whitespace-only report section HeadingText to empty string.
-- Empty HeadingText is the canonical "no section heading" state on printed reports.

DO $$
DECLARE
    v_row record;
    v_heading text;
    v_updated int := 0;
BEGIN
    FOR v_row IN
        SELECT c.id, c.configname, c.configtypeid, c.contents
        FROM configs c
        WHERE c.configtypeid IN (14, 15)
          AND c.contents ? 'HeadingText'
    LOOP
        v_heading := v_row.contents ->> 'HeadingText';

        IF v_heading IS NOT NULL AND btrim(v_heading) = '' THEN
            UPDATE configs
            SET contents = jsonb_set(v_row.contents, '{HeadingText}', '""'::jsonb, true)
            WHERE id = v_row.id;

            v_updated := v_updated + 1;
        END IF;
    END LOOP;

    RAISE NOTICE 'Normalized empty/whitespace HeadingText on % report section config(s).', v_updated;
END $$;
