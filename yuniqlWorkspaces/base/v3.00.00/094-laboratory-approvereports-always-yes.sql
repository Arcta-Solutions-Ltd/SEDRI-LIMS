-- Normalize report approval: all laboratories require the report approval workflow.
-- Aligns existing rows and sets DB default when ApproveReports is omitted from generic insert JSON.

UPDATE laboratory SET approvereports = 'Yes';

ALTER TABLE laboratory ALTER COLUMN approvereports SET DEFAULT 'Yes';
