-- Set the culturenumber of an pre-existing culture records
DO $$
DECLARE 
    previous_record_specimen_id INTEGER := NULL;
    culture_record RECORD;
    culture_number INTEGER := 1;
BEGIN
    FOR culture_record IN SELECT * FROM culture ORDER BY specimenid ASC, id ASC LOOP
        IF previous_record_specimen_id IS NOT NULL AND previous_record_specimen_id != culture_record.specimenid THEN
            culture_number := 1;
        END IF;
        previous_record_specimen_id := culture_record.specimenid;
		
		UPDATE culture SET culturenumber = culture_number where id = culture_record.id;
		
		culture_number := culture_number + 1;
    END LOOP;
END $$;
