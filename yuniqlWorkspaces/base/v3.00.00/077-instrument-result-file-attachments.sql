-- Link table: InstrumentResult <-> FileAttachment (many-to-many).
-- Files are stored in fileattachments; this table links instrument result rows to their source files.

CREATE TABLE IF NOT EXISTS instrumentresultfileattachments (
    id SERIAL PRIMARY KEY,
    instrumentresultid INTEGER NOT NULL,
    fileattachmentid INTEGER NOT NULL,
    CONSTRAINT uniqueinstrumentresultattachment UNIQUE (instrumentresultid, fileattachmentid)
);

CREATE INDEX IF NOT EXISTS idxinstrumentresultfileattachmentresult ON instrumentresultfileattachments(instrumentresultid);
CREATE INDEX IF NOT EXISTS idxinstrumentresultfileattachmentfile ON instrumentresultfileattachments(fileattachmentid);

DO $$
BEGIN
    IF NOT EXISTS (
        SELECT 1 FROM pg_constraint WHERE conname = 'fk_instrumentresultfileattachments_instrumentresult'
    ) THEN
        ALTER TABLE instrumentresultfileattachments
            ADD CONSTRAINT fk_instrumentresultfileattachments_instrumentresult
            FOREIGN KEY (instrumentresultid) REFERENCES instrumentresults(id) ON DELETE CASCADE;
    END IF;
    IF NOT EXISTS (
        SELECT 1 FROM pg_constraint WHERE conname = 'fk_instrumentresultfileattachments_fileattachment'
    ) THEN
        ALTER TABLE instrumentresultfileattachments
            ADD CONSTRAINT fk_instrumentresultfileattachments_fileattachment
            FOREIGN KEY (fileattachmentid) REFERENCES fileattachments(id) ON DELETE CASCADE;
    END IF;
END $$;
