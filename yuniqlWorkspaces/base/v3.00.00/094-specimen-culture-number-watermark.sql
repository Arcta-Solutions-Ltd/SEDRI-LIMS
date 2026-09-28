-- Add a watermark column to Specimen that records the highest CultureNumber ever
-- allocated for that specimen. Unlike MAX(culturenumber) on the Culture table, this
-- value is never decremented when an isolate is deleted, which prevents culture
-- numbers from being reused after deletion.

ALTER TABLE Specimen ADD COLUMN NextCultureNumber INT NOT NULL DEFAULT 0;

-- Back-fill from existing live data so the column is immediately correct for all
-- current specimens. Specimens with no cultures get 0 (the default).
UPDATE Specimen s
SET    NextCultureNumber = COALESCE(
           (SELECT MAX(culturenumber) FROM Culture WHERE specimenid = s.id),
           0
       );
