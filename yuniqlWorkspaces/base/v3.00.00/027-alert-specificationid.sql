ALTER TABLE alert ADD COLUMN IF NOT EXISTS specificationid INT;

-- Map and set the specification id's
UPDATE alert SET specificationid = 1 where sourceid = 971;
UPDATE alert SET specificationid = 3 where sourceid = 972;

-- Drop sourceid column
ALTER TABLE alert DROP COLUMN IF EXISTS sourceid;
