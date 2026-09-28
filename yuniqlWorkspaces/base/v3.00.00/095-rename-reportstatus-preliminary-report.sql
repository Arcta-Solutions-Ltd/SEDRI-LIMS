-- Rename report status label from 'Still not finalised and approved'
-- to 'Preliminary Report' in the reportstatus configuration mapping.
-- This updates all MappingValues entries (Types 525-533, 535-537) that
-- previously displayed this label on printed reports when a specimen has
-- not yet been finalised and approved.
UPDATE configs
SET    contents = replace(
           contents::text,
           'Still not finalised and approved',
           'Preliminary Report'
       )::jsonb
WHERE  configname = 'reportstatus'
  AND  contents::text LIKE '%Still not finalised and approved%';
