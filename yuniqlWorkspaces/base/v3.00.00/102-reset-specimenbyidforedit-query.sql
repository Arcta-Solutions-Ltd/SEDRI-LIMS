-- Use code-backed SpecimenByIdForEdit query (includes TestCategoryId / CultureTypeCategoryId and other edit-form fields).
UPDATE configs
SET contents = '{}'::jsonb
WHERE configname = 'specimenbyidforedit'
  AND contents IS NOT NULL
  AND contents != '{}'::jsonb;
