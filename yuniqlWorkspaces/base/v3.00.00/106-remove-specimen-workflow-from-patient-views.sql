-- Remove SpecimenDefault workflow binding from patient list views stored in configs.
-- Patient rows have no stateid; workflow applies on specimen form save, not patient list selection.
-- Does not modify buttons (Add Specimen submenu must remain).

UPDATE configs
SET contents = contents - 'workflow' - 'Workflow'
WHERE configname IN ('patients', 'patientcomments')
  AND contents IS NOT NULL
  AND contents != '{}'::jsonb
  AND (contents ? 'workflow' OR contents ? 'Workflow');
