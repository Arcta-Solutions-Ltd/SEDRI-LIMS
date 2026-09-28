-- Add link tables for specimen and culture file attachments.
-- Files are stored in fileattachments; these tables link specimens/cultures to their attachments.

-- Link table: Specimen <-> FileAttachment (many-to-many)
CREATE TABLE IF NOT EXISTS specimenfileattachments (
    id SERIAL PRIMARY KEY,
    specimenid INTEGER NOT NULL,
    fileattachmentid INTEGER NOT NULL,
    CONSTRAINT uniquespecimenattachment UNIQUE (specimenid, fileattachmentid)
);

CREATE INDEX IF NOT EXISTS idxspecimenfileattachmentspecimen ON specimenfileattachments(specimenid);
CREATE INDEX IF NOT EXISTS idxspecimenfileattachmentfile ON specimenfileattachments(fileattachmentid);

-- Foreign keys for specimenfileattachments
DO $$
BEGIN
    IF NOT EXISTS (
        SELECT 1 FROM pg_constraint WHERE conname = 'fk_specimenfileattachments_specimen'
    ) THEN
        ALTER TABLE specimenfileattachments
            ADD CONSTRAINT fk_specimenfileattachments_specimen
            FOREIGN KEY (specimenid) REFERENCES specimen(id) ON DELETE CASCADE;
    END IF;
    IF NOT EXISTS (
        SELECT 1 FROM pg_constraint WHERE conname = 'fk_specimenfileattachments_fileattachment'
    ) THEN
        ALTER TABLE specimenfileattachments
            ADD CONSTRAINT fk_specimenfileattachments_fileattachment
            FOREIGN KEY (fileattachmentid) REFERENCES fileattachments(id) ON DELETE CASCADE;
    END IF;
END $$;

-- Link table: Culture <-> FileAttachment (many-to-many)
CREATE TABLE IF NOT EXISTS culturefileattachments (
    id SERIAL PRIMARY KEY,
    cultureid INTEGER NOT NULL,
    fileattachmentid INTEGER NOT NULL,
    CONSTRAINT uniquecultureattachment UNIQUE (cultureid, fileattachmentid)
);

CREATE INDEX IF NOT EXISTS idxculturefileattachmentculture ON culturefileattachments(cultureid);
CREATE INDEX IF NOT EXISTS idxculturefileattachmentfile ON culturefileattachments(fileattachmentid);

-- Foreign keys for culturefileattachments
DO $$
BEGIN
    IF NOT EXISTS (
        SELECT 1 FROM pg_constraint WHERE conname = 'fk_culturefileattachments_culture'
    ) THEN
        ALTER TABLE culturefileattachments
            ADD CONSTRAINT fk_culturefileattachments_culture
            FOREIGN KEY (cultureid) REFERENCES culture(id) ON DELETE CASCADE;
    END IF;
    IF NOT EXISTS (
        SELECT 1 FROM pg_constraint WHERE conname = 'fk_culturefileattachments_fileattachment'
    ) THEN
        ALTER TABLE culturefileattachments
            ADD CONSTRAINT fk_culturefileattachments_fileattachment
            FOREIGN KEY (fileattachmentid) REFERENCES fileattachments(id) ON DELETE CASCADE;
    END IF;
END $$;
