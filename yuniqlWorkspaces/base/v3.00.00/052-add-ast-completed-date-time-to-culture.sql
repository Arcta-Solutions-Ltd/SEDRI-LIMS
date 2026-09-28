-- Add AST recorded date/time to Culture table.
-- Values are set when AST results are saved and displayed when the AST form loads.
ALTER TABLE Culture ADD COLUMN IF NOT EXISTS ASTCompletedDate DATE;
ALTER TABLE Culture ADD COLUMN IF NOT EXISTS ASTCompletedTime VARCHAR(10);
