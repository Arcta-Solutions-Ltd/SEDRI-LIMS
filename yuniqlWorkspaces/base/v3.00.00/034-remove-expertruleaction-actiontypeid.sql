-- Remove ActionTypeId column from expertruleaction.
-- Action type is no longer part of expert rules; DisplayOnReport is used for report inclusion.
ALTER TABLE expertruleaction DROP COLUMN IF EXISTS actiontypeid;
