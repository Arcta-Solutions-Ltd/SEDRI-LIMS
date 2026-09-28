-- Backfill specimens view reportCategories dataSections whitelist for cloned direct and culture tests.
-- Cloned tests create {name}datasection configs but older deployments did not register them on the view whitelist,
-- so the Report Designer could not resolve linked sections (Data Section: None).

DO $$
DECLARE
    v_contents jsonb;
    v_categories jsonb;
    v_categories_key text;
    v_updated_categories jsonb;
    v_category jsonb;
    v_category_name text;
    v_data_sections text;
    v_merged text;
    v_i int;
    v_direct_sections text[];
    v_culture_sections text[];
    v_name text;
BEGIN
    SELECT contents
    INTO v_contents
    FROM configs
    WHERE configtypeid = 5
      AND LOWER(configname) = 'specimens';

    IF v_contents IS NULL THEN
        RAISE NOTICE 'specimens view config not found; skipping data section whitelist backfill.';
        RETURN;
    END IF;

    IF v_contents ? 'reportCategories' THEN
        v_categories_key := 'reportCategories';
        v_categories := v_contents -> 'reportCategories';
    ELSIF v_contents ? 'ReportCategories' THEN
        v_categories_key := 'ReportCategories';
        v_categories := v_contents -> 'ReportCategories';
    ELSE
        RAISE NOTICE 'specimens view has no reportCategories; skipping data section whitelist backfill.';
        RETURN;
    END IF;

    IF v_categories IS NULL OR jsonb_typeof(v_categories) <> 'array' THEN
        RAISE NOTICE 'specimens view reportCategories is not an array; skipping backfill.';
        RETURN;
    END IF;

    SELECT COALESCE(array_agg(DISTINCT LOWER(TRIM(ds))), ARRAY[]::text[])
    INTO v_direct_sections
    FROM (
        SELECT COALESCE(f.contents ->> 'DataSection', f.contents ->> 'datasection') AS ds
        FROM configs f
        WHERE f.configtypeid = 1
          AND LOWER(COALESCE(f.contents ->> 'FormType', f.contents ->> 'formtype', '')) = 'directtest'
    ) direct_forms
    WHERE ds IS NOT NULL AND TRIM(ds) <> '';

    SELECT COALESCE(array_agg(DISTINCT LOWER(TRIM(ds))), ARRAY[]::text[])
    INTO v_culture_sections
    FROM (
        SELECT COALESCE(f.contents ->> 'DataSection', f.contents ->> 'datasection') AS ds
        FROM configs f
        WHERE f.configtypeid = 1
          AND LOWER(COALESCE(f.contents ->> 'FormType', f.contents ->> 'formtype', '')) = 'culturetest'
    ) culture_forms
    WHERE ds IS NOT NULL AND TRIM(ds) <> '';

    v_updated_categories := '[]'::jsonb;

    FOR v_i IN 0 .. jsonb_array_length(v_categories) - 1 LOOP
        v_category := v_categories -> v_i;
        v_category_name := COALESCE(v_category ->> 'name', v_category ->> 'Name', '');
        v_data_sections := COALESCE(v_category ->> 'dataSections', v_category ->> 'DataSections', '');

        IF LOWER(v_category_name) = 'main' THEN
            v_merged := v_data_sections;
            FOREACH v_name IN ARRAY v_direct_sections LOOP
                IF v_name IS NULL OR TRIM(v_name) = '' THEN
                    CONTINUE;
                END IF;
                IF POSITION(',' || v_name || ',' IN ',' || LOWER(v_merged) || ',') = 0 THEN
                    IF v_merged IS NULL OR TRIM(v_merged) = '' THEN
                        v_merged := v_name;
                    ELSE
                        v_merged := v_merged || ',' || v_name;
                    END IF;
                END IF;
            END LOOP;
        ELSIF LOWER(v_category_name) = 'organism' THEN
            v_merged := v_data_sections;
            FOREACH v_name IN ARRAY v_culture_sections LOOP
                IF v_name IS NULL OR TRIM(v_name) = '' THEN
                    CONTINUE;
                END IF;
                IF POSITION(',' || v_name || ',' IN ',' || LOWER(v_merged) || ',') = 0 THEN
                    IF v_merged IS NULL OR TRIM(v_merged) = '' THEN
                        v_merged := v_name;
                    ELSE
                        v_merged := v_merged || ',' || v_name;
                    END IF;
                END IF;
            END LOOP;
        ELSE
            v_merged := v_data_sections;
        END IF;

        IF v_category ? 'dataSections' THEN
            v_category := jsonb_set(v_category, '{dataSections}', to_jsonb(v_merged), true);
        ELSIF v_category ? 'DataSections' THEN
            v_category := jsonb_set(v_category, '{DataSections}', to_jsonb(v_merged), true);
        ELSE
            v_category := v_category || jsonb_build_object('dataSections', v_merged);
        END IF;

        v_updated_categories := v_updated_categories || jsonb_build_array(v_category);
    END LOOP;

    UPDATE configs
    SET contents = jsonb_set(v_contents, ARRAY[v_categories_key], v_updated_categories, false),
        lastmodifieddate = NOW()
    WHERE configtypeid = 5
      AND LOWER(configname) = 'specimens';

    RAISE NOTICE 'Backfilled specimens reportCategories dataSections whitelist (direct=% culture=%).',
        COALESCE(array_length(v_direct_sections, 1), 0),
        COALESCE(array_length(v_culture_sections, 1), 0);
END $$;
