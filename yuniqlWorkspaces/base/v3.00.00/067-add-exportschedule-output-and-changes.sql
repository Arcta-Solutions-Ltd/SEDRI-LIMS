-- Add outputdirectory and changestoinclude columns to exportschedule
ALTER TABLE exportschedule ADD COLUMN IF NOT EXISTS outputdirectory VARCHAR(500) NULL;
ALTER TABLE exportschedule ADD COLUMN IF NOT EXISTS changestoinclude VARCHAR(20) NULL;

-- Migrate existing incrementalonly to changestoinclude
UPDATE exportschedule SET changestoinclude = CASE WHEN incrementalonly THEN 'newandmodified' ELSE NULL END WHERE changestoinclude IS NULL;
