CREATE TABLE fileattachments (
    id SERIAL PRIMARY KEY,
    filepath VARCHAR(500),
    filename VARCHAR(255),
    originalfilename VARCHAR(255),
    contenttype VARCHAR(100),
    filesize INTEGER NOT NULL,
    sha256hash VARCHAR(64),
    isactive BOOLEAN NOT NULL DEFAULT true,
    lastmodifieddate TIMESTAMPTZ NOT NULL DEFAULT CURRENT_TIMESTAMP
);

-- Index on sha256hash for quick lookups/duplicate detection
CREATE INDEX idx_fileattachments_sha256hash ON fileattachments(sha256hash);

-- Index on isactive for filtering active files
CREATE INDEX idx_fileattachments_isactive ON fileattachments(isactive);

-- Add a comment to the table
COMMENT ON TABLE fileattachments IS 'Stores file attachment metadata and references';

-- Alter the images table to add file attachment reference
ALTER TABLE images 
    ADD COLUMN fileattachmentid INTEGER NOT NULL;

-- Add foreign key constraint to fileattachments table
ALTER TABLE images
    ADD CONSTRAINT fk_images_fileattachment 
        FOREIGN KEY (fileattachmentid) 
        REFERENCES fileattachments(id) 
        ON DELETE RESTRICT;

-- Index on fileattachmentid for join performance
CREATE INDEX idx_images_fileattachmentid ON images(fileattachmentid);
