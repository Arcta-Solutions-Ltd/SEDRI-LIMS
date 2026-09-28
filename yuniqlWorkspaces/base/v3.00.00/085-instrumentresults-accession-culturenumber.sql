-- Denormalized accession and culture number for inbound instrument result matching (interface + composite SQL).

DO $$
BEGIN
    IF NOT EXISTS (
        SELECT 1 FROM information_schema.columns
        WHERE table_schema = current_schema()
          AND table_name = 'instrumentresults'
          AND column_name = 'accessionnumber'
    ) THEN
        ALTER TABLE instrumentresults ADD COLUMN accessionnumber TEXT NULL;
    END IF;
    IF NOT EXISTS (
        SELECT 1 FROM information_schema.columns
        WHERE table_schema = current_schema()
          AND table_name = 'instrumentresults'
          AND column_name = 'culturenumber'
    ) THEN
        ALTER TABLE instrumentresults ADD COLUMN culturenumber TEXT NULL;
    END IF;
END $$;

COMMENT ON COLUMN instrumentresults.accessionnumber IS 'Copy of specimen.accessionnumber at insert time; used to match inbound results.';
COMMENT ON COLUMN instrumentresults.culturenumber IS 'Copy of culture.culturenumber when cultureid is set; used to match inbound culture-type results.';

-- Backfill from specimen and culture
UPDATE instrumentresults ir
SET accessionnumber = s.accessionnumber
FROM specimen s
WHERE s.id = ir.specimenid
  AND (ir.accessionnumber IS NULL OR ir.accessionnumber = '');

UPDATE instrumentresults ir
SET culturenumber = trim(c.culturenumber::text)
FROM culture c
WHERE c.id = ir.cultureid
  AND ir.cultureid IS NOT NULL
  AND ir.cultureid > 0
  AND (ir.culturenumber IS NULL OR ir.culturenumber = '');

CREATE INDEX IF NOT EXISTS ix_instrumentresults_accession_machine
    ON instrumentresults (accessionnumber, instrumentmachineid)
    WHERE accessionnumber IS NOT NULL AND instrumentmachineid IS NOT NULL;
