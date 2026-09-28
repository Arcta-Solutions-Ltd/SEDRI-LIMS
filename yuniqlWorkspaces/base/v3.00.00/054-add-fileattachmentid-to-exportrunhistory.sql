-- Add fileattachmentid to exportrunhistory for storing export files via file attachment mechanism
ALTER TABLE exportrunhistory ADD COLUMN fileattachmentid INTEGER NULL;

ALTER TABLE exportrunhistory 
  ADD CONSTRAINT fk_exportrunhistory_fileattachment 
  FOREIGN KEY (fileattachmentid) REFERENCES fileattachments(id) ON DELETE SET NULL;

CREATE INDEX idx_exportrunhistory_fileattachmentid ON exportrunhistory(fileattachmentid);
