-- Add exportprofilemapping table for storing JSON/XML structural mappers per export profile.
-- Each export profile can have at most one mapping (unique constraint on exportprofileid).
-- The structure column holds the canonical recursive tree (object | array | attribute) used
-- by both the JSON and XML structure editors. Format selects how that tree is rendered.
CREATE TABLE exportprofilemapping (
    id SERIAL PRIMARY KEY,
    exportprofileid INTEGER NOT NULL,
    format VARCHAR(10) NOT NULL,
    structure JSONB NOT NULL,
    modifieddate TIMESTAMPTZ NOT NULL,
    CONSTRAINT exportprofilemapping_format_check CHECK (format IN ('json','xml')),
    CONSTRAINT exportprofilemapping_exportprofileid_fkey
        FOREIGN KEY (exportprofileid) REFERENCES exportprofile (id)
        ON UPDATE CASCADE
        ON DELETE CASCADE,
    CONSTRAINT exportprofilemapping_unique_per_profile UNIQUE (exportprofileid)
);

CREATE INDEX idx_exportprofilemapping_profile ON exportprofilemapping(exportprofileid);
