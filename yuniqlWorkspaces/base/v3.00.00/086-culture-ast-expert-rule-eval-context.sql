-- Sidecar for manual Disk/MIC rows omitted from AST save when suppressed by an applied expert rule.
-- Used only for AST screen display and expert-rule evaluation context; not joined to clinical AST reporting by default.

CREATE TABLE IF NOT EXISTS cultureastexpertruleevalcontext (
    cultureid INT NOT NULL PRIMARY KEY REFERENCES culture(id) ON DELETE CASCADE,
    payload JSONB NOT NULL,
    lastmodifieddate TIMESTAMP NOT NULL DEFAULT now()
);

COMMENT ON TABLE cultureastexpertruleevalcontext IS 'JSON snapshot of suppressed manual AST rows for expert rule conditions; not part of the AST table.';
COMMENT ON COLUMN cultureastexpertruleevalcontext.payload IS 'JSON with DiskResults and MicResults arrays (AST row shapes matching the portal craft payload).';
COMMENT ON COLUMN cultureastexpertruleevalcontext.lastmodifieddate IS 'Timestamp of last upsert.';
