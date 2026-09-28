-- Add exportschedule table for scheduled export reports
CREATE TABLE exportschedule (
    id SERIAL PRIMARY KEY,
    exportprofileid INTEGER NOT NULL,
    name VARCHAR(100) NOT NULL,
    filter JSONB,
    frequency VARCHAR(20) NOT NULL,
    timeofday TIME NULL,
    dayofmonth INTEGER NULL,
    incrementalonly BOOLEAN NOT NULL DEFAULT false,
    enabled BOOLEAN NOT NULL DEFAULT true,
    modifieddate TIMESTAMPTZ NOT NULL,
    CONSTRAINT exportschedule_exportprofileid_fkey
        FOREIGN KEY (exportprofileid)
        REFERENCES exportprofile (id)
        ON UPDATE CASCADE
        ON DELETE CASCADE,
    CONSTRAINT exportschedule_name_unique_per_profile UNIQUE (exportprofileid, name)
);

CREATE INDEX idx_exportschedule_exportprofileid ON exportschedule(exportprofileid);
CREATE INDEX idx_exportschedule_enabled ON exportschedule(enabled);

-- Add exportscheduleid to exportrunhistory for linking scheduled runs
ALTER TABLE exportrunhistory ADD COLUMN exportscheduleid INTEGER NULL;

ALTER TABLE exportrunhistory
    ADD CONSTRAINT fk_exportrunhistory_exportschedule
    FOREIGN KEY (exportscheduleid) REFERENCES exportschedule(id) ON DELETE SET NULL;

CREATE INDEX idx_exportrunhistory_exportscheduleid ON exportrunhistory(exportscheduleid);
