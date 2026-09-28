-- Set default DisplayOnReport for existing expert rule actions that have NULL.
-- The Add/Edit Expert Rule Action mappers now persist DisplayOnReport; this backfills
-- rows created before the fix. Default 'Yes' matches the form defaultValue.
UPDATE expertruleaction SET displayonreport = 'Yes' WHERE displayonreport IS NULL;
