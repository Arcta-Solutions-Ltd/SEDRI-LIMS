-- Migrate workflow (configtypeid 18) action parameters from legacy comma-separated string to JSON object,
-- aligned with PublishReportAction (reportconfig = config name, reportname = translation key token).
-- Idempotent: only rows whose contents still contain the legacy substring are updated.
--
-- Scope: configs table only. Per-laboratory workflow copies live in laboratoryconfigs and are NOT updated here;
-- run an equivalent UPDATE there if labs keep workflow JSON in that table.
UPDATE configs
SET    contents = replace(
           contents::text,
           '"parameters": "DefaultSpecimenReport,@RepFin@"',
           '"parameters": {"reportconfig": "DefaultSpecimenReport", "reportname": "@RepFin@", "doreportsneedapproval": "no"}'
       )::jsonb
WHERE  configtypeid = 18
  AND  contents::text LIKE '%"parameters": "DefaultSpecimenReport,@RepFin@"%';
