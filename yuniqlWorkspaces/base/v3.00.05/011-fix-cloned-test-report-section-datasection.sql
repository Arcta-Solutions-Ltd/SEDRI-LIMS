-- Repair cloned direct/culture test report sections whose DataSection still references
-- a built-in or stale data section name while the form points at {name}formdatasection.

DO $$
DECLARE
    v_form record;
    v_section_name text;
    v_section_ds text;
    v_form_ds text;
    v_updated int := 0;
BEGIN
    FOR v_form IN
        SELECT
            lower(trim(c.configname)) AS form_name,
            lower(trim(coalesce(c.contents ->> 'DataSection', c.contents ->> 'datasection', ''))) AS form_data_section
        FROM configs c
        WHERE c.configtypeid IN (1, 17)
          AND lower(coalesce(c.contents ->> 'FormType', c.contents ->> 'formtype', '')) IN ('directtest', 'culturetest')
          AND lower(coalesce(c.contents ->> 'DataSection', c.contents ->> 'datasection', '')) LIKE '%formdatasection'
    LOOP
        v_form_ds := v_form.form_data_section;
        v_section_name := replace(v_form_ds, 'datasection', 'section');

        SELECT lower(trim(coalesce(s.contents ->> 'DataSection', s.contents ->> 'datasection', '')))
        INTO v_section_ds
        FROM configs s
        WHERE s.configtypeid = 14
          AND lower(s.configname) = v_section_name;

        IF v_section_ds IS NULL THEN
            RAISE NOTICE 'Cloned test form %: report section % not found; skipping.', v_form.form_name, v_section_name;
            CONTINUE;
        END IF;

        IF v_section_ds = v_form_ds THEN
            CONTINUE;
        END IF;

        UPDATE configs
        SET contents = jsonb_set(
                contents,
                '{DataSection}',
                to_jsonb(v_form_ds),
                true
            ),
            lastmodifieddate = now()
        WHERE configtypeid = 14
          AND lower(configname) = v_section_name
          AND lower(trim(coalesce(contents ->> 'DataSection', contents ->> 'datasection', ''))) <> v_form_ds;

        IF FOUND THEN
            v_updated := v_updated + 1;
            RAISE NOTICE 'Updated report section % DataSection from % to %.', v_section_name, v_section_ds, v_form_ds;
        END IF;
    END LOOP;

    RAISE NOTICE 'Cloned test report section DataSection backfill complete; sections updated=%.', v_updated;
END $$;
