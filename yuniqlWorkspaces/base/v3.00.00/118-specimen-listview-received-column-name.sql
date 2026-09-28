-- Validation (guard condition): It fetches the current GridColumns for the specimens config,
-- finds the entry whose FieldName is "receiveddate" (case-insensitive), and checks that its
-- Name is currently "@SpeRecA@" (a validation-message key that was mistakenly used as the
-- column header in 023-specimen-listview.sql). If the entry isn't found or the Name doesn't
-- match, it raises a notice and exits without making any changes.

-- Modification (only if validation passes): renames that column's Name to "@SpeRecH@"
-- (the correct "Received" header key) and sets its MinWidth/MaxWidth to 75.

-- The script is safe to run in a transaction and will print NOTICE messages indicating
-- whether it succeeded or why it aborted.

DO $$
DECLARE
    v_current_columns jsonb;
    v_item jsonb;
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

    -- Find the entry with FieldName = receiveddate (case-insensitive)
    SELECT elem INTO v_item
    FROM jsonb_array_elements(v_current_columns) elem
    WHERE lower(elem ->> 'FieldName') = 'receiveddate'
    LIMIT 1;

    IF v_item IS NULL THEN
        RAISE NOTICE 'No GridColumns entry found with FieldName "receiveddate". Aborting.';
        RETURN;
    END IF;

    IF v_item ->> 'Name' != '@SpeRecA@' THEN
        RAISE NOTICE 'GridColumns entry with FieldName "receiveddate" has Name "%", expected "@SpeRecA@". Aborting.',
            v_item ->> 'Name';
        RETURN;
    END IF;

    RAISE NOTICE 'Validation passed. Applying changes...';

    -- Build updated GridColumns array, renaming the matching entry
    SELECT jsonb_agg(
        CASE
            WHEN lower(elem ->> 'FieldName') = 'receiveddate' THEN
                elem
                || jsonb_build_object('Name', '@SpeRecH@')
                || '{"MinWidth": 75, "MaxWidth": 75}'::jsonb
            ELSE elem
        END
    )
    INTO v_updated_columns
    FROM jsonb_array_elements(v_current_columns) elem;

    -- Apply the update
    UPDATE configs
    SET contents = jsonb_set(contents, '{GridColumns}', v_updated_columns)
    WHERE configname = 'specimens';

    RAISE NOTICE 'Successfully renamed receiveddate GridColumns entry to @SpeRecH@ and updated its MinWidth/MaxWidth for specimens config.';
END;
$$;
