-- Patch data section grid descriptions so the report designer Grid Layout Editor shows
-- meaningful names instead of the legacy default "unknown".
-- Matches grids by stable Name id (case-insensitive), never by translated display text.
-- Patches all ConfigTypeId 16 rows, including cloned test data sections.

DO $$
DECLARE
    v_record RECORD;
    v_grids jsonb;
    v_grid jsonb;
    v_updated_grids jsonb;
    v_i int;
    v_name text;
    v_description text;
    v_new_description text;
    v_changed boolean;
    v_grids_key text;
BEGIN
    FOR v_record IN
        SELECT id, contents
        FROM configs
        WHERE configtypeid = 16
    LOOP
        v_grids := COALESCE(v_record.contents -> 'Grids', v_record.contents -> 'grids');
        IF v_grids IS NULL OR jsonb_typeof(v_grids) <> 'array' OR jsonb_array_length(v_grids) = 0 THEN
            CONTINUE;
        END IF;

        v_updated_grids := '[]'::jsonb;
        v_changed := false;

        FOR v_i IN 0 .. jsonb_array_length(v_grids) - 1 LOOP
            v_grid := v_grids -> v_i;
            v_name := LOWER(TRIM(COALESCE(v_grid ->> 'Name', v_grid ->> 'name', '')));
            v_description := TRIM(COALESCE(v_grid ->> 'Description', v_grid ->> 'description', ''));

            IF v_description = '' OR LOWER(v_description) = 'unknown' THEN
                v_new_description := CASE v_name
                    WHEN 'crystalgrid' THEN 'Crystal'
                    WHEN 'castgrid' THEN 'Cast'
                    WHEN 'organismgrid' THEN 'Organism'
                    WHEN 'parasitegrid' THEN 'Parasite'
                    WHEN 'gramculturetable' THEN 'Organism'
                    WHEN 'organismlisttable' THEN 'Organism List'
                    WHEN 'commentstable' THEN 'Culture Comments'
                    WHEN 'specimencommentstable' THEN 'Specimen Comments'
                    ELSE COALESCE(NULLIF(v_grid ->> 'Name', v_grid ->> 'name'), v_name)
                END;

                IF v_new_description IS NOT NULL AND v_new_description <> '' THEN
                    IF v_grid ? 'Description' THEN
                        v_grid := jsonb_set(v_grid, '{Description}', to_jsonb(v_new_description), true);
                    ELSIF v_grid ? 'description' THEN
                        v_grid := jsonb_set(v_grid, '{description}', to_jsonb(v_new_description), true);
                    ELSE
                        v_grid := v_grid || jsonb_build_object('description', v_new_description);
                    END IF;

                    v_changed := true;

                    IF v_name NOT IN (
                        'crystalgrid', 'castgrid', 'organismgrid', 'parasitegrid',
                        'gramculturetable', 'organismlisttable', 'commentstable', 'specimencommentstable'
                    ) THEN
                        RAISE NOTICE 'Set grid description from Name fallback on configs id=% grid=% description=%.',
                            v_record.id, v_name, v_new_description;
                    END IF;
                END IF;
            END IF;

            v_updated_grids := v_updated_grids || jsonb_build_array(v_grid);
        END LOOP;

        IF v_changed THEN
            IF v_record.contents ? 'Grids' THEN
                v_grids_key := 'Grids';
            ELSE
                v_grids_key := 'grids';
            END IF;

            UPDATE configs
            SET contents = jsonb_set(contents, ARRAY[v_grids_key], v_updated_grids, true)
            WHERE id = v_record.id;

            RAISE NOTICE 'Updated data section grid descriptions on configs id=%.', v_record.id;
        END IF;
    END LOOP;
END $$;
