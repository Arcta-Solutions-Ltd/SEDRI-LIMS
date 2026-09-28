-- Trim section field placements and grid header arrays that exceed their format geometry.
-- Formats with zero field columns should not retain scalar field bindings on sections.

DO $$
DECLARE
    v_section record;
    v_format_contents jsonb;
    v_section_contents jsonb;
    v_format_columns jsonb;
    v_format_grids jsonb;
    v_fields jsonb;
    v_grids jsonb;
    v_updated_fields jsonb;
    v_updated_grids jsonb;
    v_field jsonb;
    v_grid jsonb;
    v_column_capacity int;
    v_head jsonb;
    v_width text;
    v_head_capacity int;
    v_i int;
    v_j int;
    v_column int;
    v_trimmed_fields int := 0;
    v_trimmed_headers int := 0;
    v_sections_updated int := 0;
BEGIN
    FOR v_section IN
        SELECT c.id, c.configname, c.configtypeid, c.contents
        FROM configs c
        WHERE c.configtypeid IN (14, 15)
          AND COALESCE(c.contents ->> 'Format', c.contents ->> 'format', '') <> ''
    LOOP
        v_section_contents := v_section.contents;
        v_format_columns := NULL;
        v_format_grids := NULL;
        v_column_capacity := 0;

        SELECT f.contents
        INTO v_format_contents
        FROM configs f
        WHERE f.configtypeid = 23
          AND LOWER(f.configname) = LOWER(COALESCE(
              v_section_contents ->> 'Format',
              v_section_contents ->> 'format',
              ''))
        LIMIT 1;

        IF v_format_contents IS NULL THEN
            CONTINUE;
        END IF;

        IF v_format_contents ? 'Columns' THEN
            v_format_columns := v_format_contents -> 'Columns';
        ELSIF v_format_contents ? 'columns' THEN
            v_format_columns := v_format_contents -> 'columns';
        END IF;

        IF v_format_columns IS NOT NULL AND jsonb_typeof(v_format_columns) = 'array' THEN
            v_column_capacity := jsonb_array_length(v_format_columns);
        ELSE
            v_column_capacity := 0;
        END IF;

        IF v_format_contents ? 'Grids' THEN
            v_format_grids := v_format_contents -> 'Grids';
        ELSIF v_format_contents ? 'grids' THEN
            v_format_grids := v_format_contents -> 'grids';
        END IF;

        v_fields := COALESCE(v_section_contents -> 'Fields', v_section_contents -> 'fields', '[]'::jsonb);
        v_grids := COALESCE(v_section_contents -> 'Grids', v_section_contents -> 'grids', '[]'::jsonb);
        v_updated_fields := '[]'::jsonb;
        v_updated_grids := '[]'::jsonb;

        IF jsonb_typeof(v_fields) = 'array' THEN
            FOR v_i IN 0 .. jsonb_array_length(v_fields) - 1 LOOP
                v_field := v_fields -> v_i;
                v_column := COALESCE((v_field ->> 'Column')::int, (v_field ->> 'column')::int, 0);

                IF v_column_capacity > 0 AND v_column >= 1 AND v_column <= v_column_capacity THEN
                    v_updated_fields := v_updated_fields || jsonb_build_array(v_field);
                ELSIF v_column_capacity <= 0 THEN
                    v_trimmed_fields := v_trimmed_fields + 1;
                ELSE
                    v_trimmed_fields := v_trimmed_fields + 1;
                END IF;
            END LOOP;
        END IF;

        IF jsonb_typeof(v_grids) = 'array' AND v_format_grids IS NOT NULL AND jsonb_typeof(v_format_grids) = 'array' THEN
            FOR v_i IN 0 .. jsonb_array_length(v_grids) - 1 LOOP
                v_grid := v_grids -> v_i;

                IF v_i < jsonb_array_length(v_format_grids) THEN
                    v_width := COALESCE(
                        v_format_grids -> v_i ->> 'Width',
                        v_format_grids -> v_i ->> 'width',
                        '');
                ELSE
                    v_width := COALESCE(
                        v_format_grids -> (jsonb_array_length(v_format_grids) - 1) ->> 'Width',
                        v_format_grids -> (jsonb_array_length(v_format_grids) - 1) ->> 'width',
                        '');
                END IF;

                v_head_capacity := CASE
                    WHEN v_width IS NULL OR TRIM(v_width) = '' THEN 0
                    ELSE array_length(string_to_array(v_width, '|'), 1)
                END;

                v_head := COALESCE(v_grid -> 'Head', v_grid -> 'head', '[]'::jsonb);

                IF jsonb_typeof(v_head) = 'array' AND jsonb_array_length(v_head) > v_head_capacity THEN
                    v_head := (
                        SELECT COALESCE(jsonb_agg(value), '[]'::jsonb)
                        FROM (
                            SELECT value
                            FROM jsonb_array_elements(v_head) WITH ORDINALITY AS t(value, ord)
                            WHERE ord <= v_head_capacity
                        ) trimmed
                    );
                    v_trimmed_headers := v_trimmed_headers + 1;
                END IF;

                IF v_grid ? 'Head' THEN
                    v_grid := jsonb_set(v_grid, '{Head}', v_head, true);
                ELSIF v_grid ? 'head' THEN
                    v_grid := jsonb_set(v_grid, '{head}', v_head, true);
                ELSE
                    v_grid := v_grid || jsonb_build_object('Head', v_head);
                END IF;

                v_updated_grids := v_updated_grids || jsonb_build_array(v_grid);
            END LOOP;
        ELSE
            v_updated_grids := v_grids;
        END IF;

        IF v_updated_fields IS DISTINCT FROM v_fields OR v_updated_grids IS DISTINCT FROM v_grids THEN
            IF v_section_contents ? 'Fields' THEN
                v_section_contents := jsonb_set(v_section_contents, '{Fields}', v_updated_fields, true);
            ELSIF v_section_contents ? 'fields' THEN
                v_section_contents := jsonb_set(v_section_contents, '{fields}', v_updated_fields, true);
            END IF;

            IF v_section_contents ? 'Grids' THEN
                v_section_contents := jsonb_set(v_section_contents, '{Grids}', v_updated_grids, true);
            ELSIF v_section_contents ? 'grids' THEN
                v_section_contents := jsonb_set(v_section_contents, '{grids}', v_updated_grids, true);
            END IF;

            UPDATE configs
            SET contents = v_section_contents,
                lastmodifieddate = NOW()
            WHERE id = v_section.id;

            v_sections_updated := v_sections_updated + 1;
        END IF;
    END LOOP;

    RAISE NOTICE 'Trimmed section format bindings (sections=% fields=% headers=%).',
        v_sections_updated, v_trimmed_fields, v_trimmed_headers;
END $$;
