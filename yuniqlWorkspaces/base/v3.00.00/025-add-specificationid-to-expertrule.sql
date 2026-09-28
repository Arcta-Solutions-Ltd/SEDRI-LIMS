-- Add specificationid to expertrule to link expert rules to specifications.
-- Enables "specification in use" validation when deleting specifications.
-- Run as a new migration; does not amend existing scripts.

ALTER TABLE expertrule ADD COLUMN IF NOT EXISTS specificationid INT;
