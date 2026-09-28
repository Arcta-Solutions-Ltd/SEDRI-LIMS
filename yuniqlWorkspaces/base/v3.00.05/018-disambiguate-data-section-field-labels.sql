-- Disambiguate report designer data section field labels that shared generic tokens
-- (Location, WBC, Test Result, Result, Positive Result) across multiple data sections.
-- Patches all ConfigTypeId 16 rows, including cloned test data sections, by field Value.

DO $$
DECLARE
    v_record RECORD;
    v_fields jsonb;
    v_field jsonb;
    v_updated_fields jsonb;
    v_i int;
    v_value text;
    v_new_label text;
    v_changed boolean;
BEGIN
    FOR v_record IN
        SELECT id, contents
        FROM configs
        WHERE configtypeid = 16
    LOOP
        v_fields := COALESCE(v_record.contents -> 'Fields', v_record.contents -> 'fields');
        IF v_fields IS NULL OR jsonb_typeof(v_fields) <> 'array' THEN
            CONTINUE;
        END IF;

        v_updated_fields := '[]'::jsonb;
        v_changed := false;

        FOR v_i IN 0 .. jsonb_array_length(v_fields) - 1 LOOP
            v_field := v_fields -> v_i;
            v_value := LOWER(TRIM(COALESCE(v_field ->> 'Value', v_field ->> 'value', '')));
            v_new_label := COALESCE(v_field ->> 'Label', v_field ->> 'label');

            v_new_label := CASE v_value
                WHEN 'fullyqualifiedname' THEN '@GenLocA@'
                WHEN 'patientlocation' THEN '@RepPatLoc@'
                WHEN 'wbc' THEN '@TesGraWbc@'
                WHEN 'wbcwetprep' THEN '@TesWetWbc@'
                WHEN 'oxidaseresultid' THEN '@TesOxiRes@'
                WHEN 'catalaseresultid' THEN '@TesCatRes@'
                WHEN 'carbapenemaseresultid' THEN '@TesCarRes@'
                WHEN 'betalactamaseresultid' THEN '@TesBetRes@'
                WHEN 'esblresultid' THEN '@TesEsbRes@'
                WHEN 'jevserologyresultid' THEN '@TesJevRes@'
                WHEN 'wrightsstainresultid' THEN '@TesWriRes@'
                WHEN 'auramineid' THEN '@TesAurRes@'
                WHEN 'pregnancyid' THEN '@TesPreRes@'
                WHEN 'antresultid' THEN '@TesHpyRes@'
                WHEN 'kohresultid' THEN '@TesFunRes@'
                WHEN 'indiainkresult' THEN '@TesIndA@'
                WHEN 'kohfungalid' THEN '@TesFunPos@'
                WHEN 'positiveresult' THEN '@TesIndPos@'
                ELSE v_new_label
            END;

            IF v_new_label IS DISTINCT FROM COALESCE(v_field ->> 'Label', v_field ->> 'label') THEN
                v_changed := true;
            END IF;

            IF v_field ? 'Label' THEN
                v_field := jsonb_set(v_field, '{Label}', to_jsonb(v_new_label), true);
            ELSIF v_field ? 'label' THEN
                v_field := jsonb_set(v_field, '{label}', to_jsonb(v_new_label), true);
            END IF;

            v_updated_fields := v_updated_fields || jsonb_build_array(v_field);
        END LOOP;

        IF v_changed THEN
            IF v_record.contents ? 'Fields' THEN
                UPDATE configs
                SET contents = jsonb_set(contents, '{Fields}', v_updated_fields, true)
                WHERE id = v_record.id;
            ELSE
                UPDATE configs
                SET contents = jsonb_set(contents, '{fields}', v_updated_fields, true)
                WHERE id = v_record.id;
            END IF;

            RAISE NOTICE 'Updated data section field labels on configs id=%.', v_record.id;
        END IF;
    END LOOP;
END $$;
