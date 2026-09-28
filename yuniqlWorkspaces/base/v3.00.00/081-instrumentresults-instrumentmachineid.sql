-- Stores InstrumentMachine list item id (list 144) for each instrument result row so the machine interface
-- can filter the outbound queue and correlate with appsettings InstrumentId without relying on profile name alone.

DO $$
BEGIN
    IF NOT EXISTS (
        SELECT 1 FROM information_schema.columns
        WHERE table_schema = current_schema()
          AND table_name = 'instrumentresults'
          AND column_name = 'instrumentmachineid'
    ) THEN
        ALTER TABLE instrumentresults ADD COLUMN instrumentmachineid INTEGER NULL;
    END IF;
END $$;

COMMENT ON COLUMN instrumentresults.instrumentmachineid IS 'Optional FK-style reference to listitem.id on the InstrumentMachine list (144); matches SingleInstrumentConfig.InstrumentMachineId.';

update list set deleted = false;

INSERT INTO listitem (id, listid, value, fixed, enabled, displayorder, deleted, lastmodifieddate)
SELECT 59, 100, 'Vitek', true, true, 1, false, now()
WHERE NOT EXISTS (SELECT 1 FROM listitem WHERE id = 59);