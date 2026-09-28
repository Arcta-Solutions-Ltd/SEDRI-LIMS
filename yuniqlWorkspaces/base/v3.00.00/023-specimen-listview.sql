-- Validation (guard conditions): It fetches the current GridColumns for the specimens config, checks that the array length is exactly 10, then verifies every expected key+fieldName pair is present — all case-insensitively. If anything doesn't match, it raises a notice and exits without making any changes.
-- Modifications (only if validation passes):

-- column1 — removes both IsSorted and IsSortedDescending properties
-- column6 — sets MinWidth/MaxWidth to 75 and renames to @SpecimenListViewColumnNameCollected@
-- column7 — sets MinWidth/MaxWidth to 75 and renames to @SpecimenListViewColumnNameReceived@
-- column9 — appended as a new entry at the end of the array

-- The script is safe to run in a transaction and will print NOTICE messages indicating whether it succeeded or why it aborted.

DO $$
DECLARE
    v_current_columns jsonb;
    v_expected_keys_fields jsonb := '[
        {"key": "alert", "fieldName": ""},
        {"key": "column1", "fieldName": "accessionnumber"},
        {"key": "menu", "fieldName": ""},
        {"key": "column2", "fieldName": "firstname"},
        {"key": "column3", "fieldName": "surname"},
        {"key": "column4", "fieldName": "patientref"},
        {"key": "column5", "fieldName": "specimentype"},
        {"key": "column6", "fieldName": "collectiondate"},
        {"key": "column7", "fieldName": "receiveddate"},
        {"key": "column8", "fieldName": "state"}
    ]'::jsonb;
    v_new_column jsonb := '{
        "Key": "column9",
        "Name": "@SpeMod@",
        "FieldName": "lastmodifieddate",
        "MinWidth": 110,
        "MaxWidth": 120,
        "IsResizable": true,
        "IsCollapsible": false,
        "Highlight": true,
        "IsSorted": true,
        "IsSortedDescending": true
    }'::jsonb;
    v_matches boolean := true;
    i int;
    v_item jsonb;
    v_expected_item jsonb;
    v_updated_columns jsonb;
BEGIN
    -- Fetch current GridColumns for the specimens config
    SELECT contents -> 'GridColumns'
    INTO v_current_columns
    FROM configs
    WHERE configname = 'specimens';

    IF v_current_columns IS NULL THEN
        RAISE NOTICE 'No GridColumns found for specimens config. Aborting.';
        RETURN;
    END IF;

    -- Check array length matches
    IF jsonb_array_length(v_current_columns) != jsonb_array_length(v_expected_keys_fields) THEN
        RAISE NOTICE 'GridColumns length mismatch (found %, expected %). Aborting.',
            jsonb_array_length(v_current_columns),
            jsonb_array_length(v_expected_keys_fields);
        RETURN;
    END IF;

    -- Validate each item's key and fieldName match expected values (case-insensitive key lookup)
    FOR i IN 0 .. jsonb_array_length(v_expected_keys_fields) - 1 LOOP
        v_expected_item := v_expected_keys_fields -> i;

        -- Find matching item in current columns by key (handles mixed case)
        SELECT elem INTO v_item
        FROM jsonb_array_elements(v_current_columns) elem
        WHERE lower(elem ->> 'Key') = lower(v_expected_item ->> 'key')
        LIMIT 1;

        IF v_item IS NULL THEN
            v_matches := false;
            RAISE NOTICE 'Key "%" not found in GridColumns. Aborting.', v_expected_item ->> 'key';
            EXIT;
        END IF;

        IF lower(v_item ->> 'FieldName') != lower(v_expected_item ->> 'fieldName') THEN
            v_matches := false;
            RAISE NOTICE 'FieldName mismatch for key "%": found "%" expected "%". Aborting.',
                v_expected_item ->> 'key',
                v_item ->> 'FieldName',
                v_expected_item ->> 'fieldName';
            EXIT;
        END IF;
    END LOOP;

    IF NOT v_matches THEN
        RETURN;
    END IF;

    RAISE NOTICE 'Validation passed. Applying changes...';

    -- Build updated GridColumns array with all modifications applied
    SELECT jsonb_agg(
        CASE
            -- column1: remove isSorted and isSortedDescending
            WHEN lower(elem ->> 'Key') = 'column1' THEN
                (elem - 'IsSorted' - 'IsSortedDescending' - 'isSorted' - 'isSortedDescending')

            -- column6: update MinWidth, MaxWidth, and Name
            WHEN lower(elem ->> 'Key') = 'column6' THEN
                elem
                || '{"MinWidth": 75, "MaxWidth": 75}'::jsonb
                || jsonb_build_object('Name', '@SpeColE@')

            -- column7: update MinWidth, MaxWidth, and Name
            WHEN lower(elem ->> 'Key') = 'column7' THEN
                elem
                || '{"MinWidth": 75, "MaxWidth": 75}'::jsonb
                || jsonb_build_object('Name', '@SpeRecA@')

            ELSE elem
        END
    )
    INTO v_updated_columns
    FROM jsonb_array_elements(v_current_columns) elem;

    -- Append the new column9
    v_updated_columns := v_updated_columns || jsonb_build_array(v_new_column);

    -- Apply the update
    UPDATE configs
    SET contents = jsonb_set(contents, '{GridColumns}', v_updated_columns)
    WHERE configname = 'specimens';

    RAISE NOTICE 'Successfully updated GridColumns for specimens config.';
END;
$$;