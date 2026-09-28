-- Safety net for databases created before specialastrow.breakpointid was present.
ALTER TABLE specialastrow ADD COLUMN IF NOT EXISTS breakpointid INT;
