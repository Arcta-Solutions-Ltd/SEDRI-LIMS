-- Expert rule source to specification migration.
-- Replaces sourceid with specificationid (FK to specification).
-- Run as a new migration; does not amend existing scripts.

CREATE TABLE public.specification
(
    Id SERIAL PRIMARY KEY,
    guidelinesid integer NOT NULL,
    documentid integer NOT NULL,
    versionnumberid integer,
    publicationyearid integer,
    lastmodifieddate timestamp with time zone NOT NULL
);

ALTER TABLE expertrule ADD COLUMN IF NOT EXISTS specificationid INT;

-- Migrate data: link expert rules to specification via guidelinesid = sourceid
UPDATE expertrule e
SET specificationid = (
    SELECT s.id FROM specification s
    WHERE s.guidelinesid = e.sourceid
    LIMIT 1
)
WHERE e.sourceid IS NOT NULL AND e.specificationid IS NULL;

-- Drop sourceid column
ALTER TABLE expertrule DROP COLUMN IF EXISTS sourceid;
