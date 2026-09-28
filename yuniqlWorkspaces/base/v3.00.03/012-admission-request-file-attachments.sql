-- Admission and request file attachments: junction tables, event taxonomy, role permissions.
-- Id allocation continues after 002-admission-request-record-view.sql (list 74 highest 2190).

-- ---------------------------------------------------------------------------
-- Junction tables
-- ---------------------------------------------------------------------------

CREATE TABLE IF NOT EXISTS admissionfileattachments (
    id SERIAL PRIMARY KEY,
    admissionid INTEGER NOT NULL,
    fileattachmentid INTEGER NOT NULL,
    CONSTRAINT uniqueadmissionattachment UNIQUE (admissionid, fileattachmentid)
);

CREATE INDEX IF NOT EXISTS idxadmissionfileattachmentadmission ON admissionfileattachments(admissionid);
CREATE INDEX IF NOT EXISTS idxadmissionfileattachmentfile ON admissionfileattachments(fileattachmentid);

DELETE FROM admissionfileattachments afa
WHERE NOT EXISTS (SELECT 1 FROM admission a WHERE a.id = afa.admissionid);

DO $$
BEGIN
    IF NOT EXISTS (
        SELECT 1 FROM pg_constraint WHERE conname = 'fk_admissionfileattachments_admission'
    ) THEN
        ALTER TABLE admissionfileattachments
            ADD CONSTRAINT fk_admissionfileattachments_admission
            FOREIGN KEY (admissionid) REFERENCES admission(id) ON DELETE CASCADE;
    END IF;
    IF NOT EXISTS (
        SELECT 1 FROM pg_constraint WHERE conname = 'fk_admissionfileattachments_fileattachment'
    ) THEN
        ALTER TABLE admissionfileattachments
            ADD CONSTRAINT fk_admissionfileattachments_fileattachment
            FOREIGN KEY (fileattachmentid) REFERENCES fileattachments(id) ON DELETE CASCADE;
    END IF;
END $$;

CREATE TABLE IF NOT EXISTS requestfileattachments (
    id SERIAL PRIMARY KEY,
    requestid INTEGER NOT NULL,
    fileattachmentid INTEGER NOT NULL,
    CONSTRAINT uniquerequestattachment UNIQUE (requestid, fileattachmentid)
);

CREATE INDEX IF NOT EXISTS idxrequestfileattachmentrequest ON requestfileattachments(requestid);
CREATE INDEX IF NOT EXISTS idxrequestfileattachmentfile ON requestfileattachments(fileattachmentid);

DELETE FROM requestfileattachments rfa
WHERE NOT EXISTS (SELECT 1 FROM request r WHERE r.id = rfa.requestid);

DO $$
BEGIN
    IF NOT EXISTS (
        SELECT 1 FROM pg_constraint WHERE conname = 'fk_requestfileattachments_request'
    ) THEN
        ALTER TABLE requestfileattachments
            ADD CONSTRAINT fk_requestfileattachments_request
            FOREIGN KEY (requestid) REFERENCES request(id) ON DELETE CASCADE;
    END IF;
    IF NOT EXISTS (
        SELECT 1 FROM pg_constraint WHERE conname = 'fk_requestfileattachments_fileattachment'
    ) THEN
        ALTER TABLE requestfileattachments
            ADD CONSTRAINT fk_requestfileattachments_fileattachment
            FOREIGN KEY (fileattachmentid) REFERENCES fileattachments(id) ON DELETE CASCADE;
    END IF;
END $$;

-- ---------------------------------------------------------------------------
-- Event list (list 74) under Admission topic (2183) and Request topic (2184)
-- ---------------------------------------------------------------------------

INSERT INTO listitem (id, listid, value, fixed, enabled, displayorder, deleted, lastmodifieddate)
SELECT v.id, 74, v.value, false, true, 1, false, now()
FROM (VALUES
    (2191, 'manageadmissionattachments'),
    (2192, 'managerequestattachments')
) AS v(id, value)
WHERE NOT EXISTS (SELECT 1 FROM listitem WHERE listid = 74 AND LOWER(TRIM(value)) = LOWER(TRIM(v.value)));

INSERT INTO listitemparentchild (parentid, childid, lastmodifieddate)
SELECT 2183, li.id, now()
FROM listitem li
WHERE li.listid = 74 AND LOWER(li.value) = 'manageadmissionattachments'
  AND NOT EXISTS (SELECT 1 FROM listitemparentchild WHERE parentid = 2183 AND childid = li.id);

INSERT INTO listitemparentchild (parentid, childid, lastmodifieddate)
SELECT 2184, li.id, now()
FROM listitem li
WHERE li.listid = 74 AND LOWER(li.value) = 'managerequestattachments'
  AND NOT EXISTS (SELECT 1 FROM listitemparentchild WHERE parentid = 2184 AND childid = li.id);

-- ---------------------------------------------------------------------------
-- Role permissions — same cohort as admission/request record views
-- ---------------------------------------------------------------------------

UPDATE role
SET eventpermission = jsonb_set(
    eventpermission::jsonb,
    '{AllowedEvents}',
    (eventpermission->'AllowedEvents')::jsonb
        || '["manageadmissionattachments", "managerequestattachments"]'::jsonb
)
WHERE (eventpermission->'AllowedEvents') @> '["editadmission"]'::jsonb
  AND NOT (eventpermission->'AllowedEvents') @> '["manageadmissionattachments"]'::jsonb;
