-- Use code-backed editspecimenmapper (includes TestCategoryId / CultureTypeCategoryId with PascalCase MoreData keys).
UPDATE configs
SET contents = '{}'::jsonb
WHERE configname = 'editspecimenmapper'
  AND contents IS NOT NULL
  AND contents != '{}'::jsonb;

-- Use code-backed editspecimen event (StringFields for multiselect category values).
UPDATE configs
SET contents = '{}'::jsonb
WHERE configname = 'editspecimen'
  AND contents IS NOT NULL
  AND contents != '{}'::jsonb;
