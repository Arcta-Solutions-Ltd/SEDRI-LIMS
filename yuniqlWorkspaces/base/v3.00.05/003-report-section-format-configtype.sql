-- Report section formats shared ConfigTypeId 21 with the 'reportstatus' settings record, so every
-- report designer load read that settings row and failed to deserialise it as a format. Give formats
-- their own ConfigType, move the format records onto it, and strip the transient designer flags that
-- leaked into stored contents before the save path projected models onto the domain config shape.
-- Idempotent: safe to re-run.

INSERT INTO ConfigType (Id, Name, Type)
OVERRIDING SYSTEM VALUE
SELECT 23, 'ReportSectionFormat', 'ReportSection'
WHERE NOT EXISTS (SELECT 1 FROM ConfigType WHERE Id = 23);

-- Keep the identity sequence ahead of the explicitly inserted id.
SELECT setval(
    pg_get_serial_sequence('configtype', 'id'),
    GREATEST((SELECT MAX(Id) FROM ConfigType), 1),
    true
);

-- Move the format records off the shared settings type. Everything on 21 is a section format document
-- apart from 'reportstatus', which is the status mapping array the ReportStatusMapper reads by name.
-- Shape is not a reliable test here: the 'absolute' format has neither Columns nor Grids.
UPDATE configs
SET configtypeid = 23,
    lastmodifieddate = now()
WHERE configtypeid = 21
  AND lower(configname) <> 'reportstatus'
  AND jsonb_typeof(contents::jsonb) = 'object';

-- Remove the transient designer flags that were serialised straight into contents by the old save path.
UPDATE configs
SET contents = ((contents::jsonb) - 'IsNew' - 'State' - 'isNew' - 'state' - 'ConfigId' - 'configId')::json,
    lastmodifieddate = now()
WHERE configtypeid IN (13, 14, 15, 21, 23)
  AND jsonb_typeof(contents::jsonb) = 'object'
  AND (
        contents::jsonb ? 'IsNew'
     OR contents::jsonb ? 'State'
     OR contents::jsonb ? 'isNew'
     OR contents::jsonb ? 'state'
     OR contents::jsonb ? 'ConfigId'
     OR contents::jsonb ? 'configId'
  );
