-- Add link table for patient file attachments.
-- Files are stored in fileattachments; this table links patients to their attachments.

-- Link table: Patient <-> FileAttachment (many-to-many)
CREATE TABLE IF NOT EXISTS patientfileattachments (
    id SERIAL PRIMARY KEY,
    patientid INTEGER NOT NULL,
    fileattachmentid INTEGER NOT NULL,
    CONSTRAINT uniquepatientattachment UNIQUE (patientid, fileattachmentid)
);

CREATE INDEX IF NOT EXISTS idxpatientfileattachmentpatient ON patientfileattachments(patientid);
CREATE INDEX IF NOT EXISTS idxpatientfileattachmentfile ON patientfileattachments(fileattachmentid);

-- Foreign keys for patientfileattachments
DO $$
BEGIN
    IF NOT EXISTS (
        SELECT 1 FROM pg_constraint WHERE conname = 'fk_patientfileattachments_patient'
    ) THEN
        ALTER TABLE patientfileattachments
            ADD CONSTRAINT fk_patientfileattachments_patient
            FOREIGN KEY (patientid) REFERENCES patient(id) ON DELETE CASCADE;
    END IF;
    IF NOT EXISTS (
        SELECT 1 FROM pg_constraint WHERE conname = 'fk_patientfileattachments_fileattachment'
    ) THEN
        ALTER TABLE patientfileattachments
            ADD CONSTRAINT fk_patientfileattachments_fileattachment
            FOREIGN KEY (fileattachmentid) REFERENCES fileattachments(id) ON DELETE CASCADE;
    END IF;
END $$;
