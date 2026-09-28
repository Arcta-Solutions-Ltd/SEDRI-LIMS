ALTER TABLE breakpoint ADD COLUMN IF NOT EXISTS specificationid INT;

-- Map and set the specification id's
UPDATE breakpoint SET specificationid = 1 where sourceid = 971;
UPDATE breakpoint SET specificationid = 3 where sourceid = 972;

-- Drop sourceid column
ALTER TABLE breakpoint DROP COLUMN IF EXISTS sourceid;
