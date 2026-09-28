-- Register home dashboard TAT Compliance KPI query (SpecialFactory: homedashboardtatcompliance).
-- Empty contents so QueryAdapter falls back to SpecimenQueryFactory / HomeDashboardTatComplianceQuery definition.

INSERT INTO configs (configname, configtypeid, contents, lastmodifieddate)
SELECT 'homedashboardtatcompliance', 8, '{}'::jsonb, now()
WHERE NOT EXISTS (SELECT 1 FROM configs WHERE configname = 'homedashboardtatcompliance');
