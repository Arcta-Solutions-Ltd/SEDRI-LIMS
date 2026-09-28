-- Fix ApprovalDataSection date field labels: SubmittedDate and ApprovedDate shared @RepPreA@ ("Date").
-- Assign distinct labels @SpeSubDat@ (Submitted Date) and @SpeAppDat@ (Approved Date).

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
          AND LOWER(configname) = 'approvaldatasection'
    LOOP
        v_fields := COALESCE(v_record.contents -> 'Fields', v_record.contents -> 'fields');
        IF v_fields IS NULL OR jsonb_typeof(v_fields) <> 'array' THEN
            CONTINUE;
        END IF;

        v_updated_fields := '[]'::jsonb;
        v_changed := false;

        FOR v_i IN 0 .. jsonb_array_length(v_fields) - 1 LOOP
            v_field := v_fields -> v_i;
            v_value := COALESCE(v_field ->> 'Value', v_field ->> 'value', '');

            IF LOWER(v_value) = 'submitteddate' THEN
                v_new_label := '@SpeSubDat@';
                v_changed := true;
            ELSIF LOWER(v_value) = 'approveddate' THEN
                v_new_label := '@SpeAppDat@';
                v_changed := true;
            ELSE
                v_new_label := COALESCE(v_field ->> 'Label', v_field ->> 'label');
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

            RAISE NOTICE 'Updated ApprovalDataSection date labels on configs id=%.', v_record.id;
        END IF;
    END LOOP;
END $$;
