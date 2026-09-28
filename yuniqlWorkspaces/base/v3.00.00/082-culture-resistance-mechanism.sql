-- Expert resistance mechanisms from instrument interfaces (e.g. Vitek2), stored per culture (isolate). Replaced on each ingest when the client sends a non-null list.

CREATE TABLE IF NOT EXISTS cultureresistancemechanism (
    id SERIAL PRIMARY KEY,
    cultureid INT NOT NULL REFERENCES culture (id) ON DELETE CASCADE,
    drugfamily TEXT NOT NULL DEFAULT '',
    phenotype TEXT NOT NULL DEFAULT '',
    lastmodifieddate TIMESTAMPTZ NOT NULL DEFAULT now()
);

CREATE INDEX IF NOT EXISTS ix_cultureresistancemechanism_cultureid ON cultureresistancemechanism (cultureid);

COMMENT ON TABLE cultureresistancemechanism IS 'Instrument-reported resistance mechanisms (drug family + phenotype) for display; clinical interpretation remains breakpoints, test patterns, alerts, expert rules.';
COMMENT ON COLUMN cultureresistancemechanism.drugfamily IS 'e.g. Vitek ExpertFinding resistanceMechanism drugFamily';
COMMENT ON COLUMN cultureresistancemechanism.phenotype IS 'e.g. Vitek PhenoType';

update list set deleted = false;
