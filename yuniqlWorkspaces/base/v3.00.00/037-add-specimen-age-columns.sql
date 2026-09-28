-- Add specimen age columns for age at specimen creation time (years, months, days, hours)
ALTER TABLE Specimen ADD COLUMN IF NOT EXISTS AgeYears INT;
ALTER TABLE Specimen ADD COLUMN IF NOT EXISTS AgeMonths INT;
ALTER TABLE Specimen ADD COLUMN IF NOT EXISTS AgeDays INT;
ALTER TABLE Specimen ADD COLUMN IF NOT EXISTS AgeHours INT;

-- Add Age (150) to FieldTypeList for specimen age control (years, months, days, hours)
INSERT INTO listitem (id, listid, value, lastmodifieddate, fixed, enabled, displayorder, deleted)
SELECT 150, 109, 'Age', now(), false, true, 1, false
WHERE NOT EXISTS (SELECT 1 FROM listitem WHERE id = 150);
