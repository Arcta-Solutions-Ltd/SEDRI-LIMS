-- Specimen Growth internal hierarchy: explicit list flag and fixed parent nodes for table maintenance.

ALTER TABLE list ADD COLUMN IF NOT EXISTS internalhierarchy BOOLEAN NOT NULL DEFAULT false;

UPDATE list SET internalhierarchy = true WHERE id = 133;

UPDATE listitem SET fixed = true WHERE id IN (427, 428);
