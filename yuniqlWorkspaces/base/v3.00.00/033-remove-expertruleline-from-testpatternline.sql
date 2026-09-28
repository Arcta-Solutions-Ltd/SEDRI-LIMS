-- Remove Expert Rule column from testpatternline.
-- The Expert Rule toggle has been removed from the edit antibiotics screen.
ALTER TABLE testpatternline DROP COLUMN IF EXISTS expertruleline;
