-- Strip legacy Text from format Heading[] entries. Format headings are geometry-only;
-- section HeadingText supplies heading content on printed reports.

DO $$
DECLARE
    v_row record;
    v_heading jsonb;
    v_updated_headings jsonb;
    v_i int;
    v_entry jsonb;
    v_formats_updated int := 0;
BEGIN
    FOR v_row IN
        SELECT c.id, c.configname, c.contents
        FROM configs c
        WHERE c.configtypeid = 23
          AND jsonb_typeof(c.contents -> 'Heading') = 'array'
          AND jsonb_array_length(c.contents -> 'Heading') > 0
    LOOP
        v_heading := v_row.contents -> 'Heading';
        v_updated_headings := '[]'::jsonb;

        FOR v_i IN 0 .. jsonb_array_length(v_heading) - 1 LOOP
            v_entry := v_heading -> v_i;
            v_entry := v_entry - 'Text' - 'text';
            v_updated_headings := v_updated_headings || jsonb_build_array(v_entry);
        END LOOP;

        UPDATE configs
        SET contents = jsonb_set(v_row.contents, '{Heading}', v_updated_headings, true)
        WHERE id = v_row.id;

        v_formats_updated := v_formats_updated + 1;
    END LOOP;

    RAISE NOTICE 'Stripped Text from Heading[] on % section format config(s).', v_formats_updated;
END $$;
