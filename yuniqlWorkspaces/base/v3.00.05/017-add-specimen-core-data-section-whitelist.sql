-- Add SpecimenCoreDataSection to the specimens view Main reportCategories dataSections whitelist
-- so the Report Designer field dropdown includes core specimen/patient/request fields.

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
    v_section_name constant text := 'SpecimenCoreDataSection';
BEGIN
    SELECT contents
    INTO v_contents
    FROM configs
    WHERE configtypeid = 5
      AND LOWER(configname) = 'specimens';

    IF v_contents IS NULL THEN
        RAISE NOTICE 'specimens view config not found; skipping SpecimenCoreDataSection whitelist update.';
        RETURN;
    END IF;

    IF v_contents ? 'reportCategories' THEN
        v_categories_key := 'reportCategories';
        v_categories := v_contents -> 'reportCategories';
    ELSIF v_contents ? 'ReportCategories' THEN
        v_categories_key := 'ReportCategories';
        v_categories := v_contents -> 'ReportCategories';
    ELSE
        RAISE NOTICE 'specimens view has no reportCategories; skipping SpecimenCoreDataSection whitelist update.';
        RETURN;
    END IF;

    IF v_categories IS NULL OR jsonb_typeof(v_categories) <> 'array' THEN
        RAISE NOTICE 'specimens view reportCategories is not an array; skipping SpecimenCoreDataSection whitelist update.';
        RETURN;
    END IF;

    v_updated_categories := '[]'::jsonb;

    FOR v_i IN 0 .. jsonb_array_length(v_categories) - 1 LOOP
        v_category := v_categories -> v_i;
        v_category_name := COALESCE(v_category ->> 'name', v_category ->> 'Name', '');
        v_data_sections := COALESCE(v_category ->> 'dataSections', v_category ->> 'DataSections', '');

        IF LOWER(v_category_name) = 'main' THEN
            IF POSITION(',' || LOWER(v_section_name) || ',' IN ',' || LOWER(v_data_sections) || ',') = 0 THEN
                IF TRIM(v_data_sections) = '' THEN
                    v_merged := v_section_name;
                ELSE
                    v_merged := v_section_name || ',' || v_data_sections;
                END IF;
            ELSE
                v_merged := v_data_sections;
            END IF;

            IF v_category ? 'dataSections' THEN
                v_category := jsonb_set(v_category, '{dataSections}', to_jsonb(v_merged), true);
            ELSIF v_category ? 'DataSections' THEN
                v_category := jsonb_set(v_category, '{DataSections}', to_jsonb(v_merged), true);
            END IF;
        END IF;

        v_updated_categories := v_updated_categories || jsonb_build_array(v_category);
    END LOOP;

    UPDATE configs
    SET contents = jsonb_set(v_contents, ARRAY[v_categories_key], v_updated_categories, true)
    WHERE configtypeid = 5
      AND LOWER(configname) = 'specimens';

    RAISE NOTICE 'SpecimenCoreDataSection whitelist update complete.';
END $$;
