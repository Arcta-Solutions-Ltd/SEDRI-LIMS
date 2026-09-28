ALTER TABLE breakpoint
ADD COLUMN versionid INTEGER NOT NULL DEFAULT 0;

INSERT INTO List(id, name, grouping, common, description, lastmodifieddate, deleted)
VALUES(134, 'Version', 'System', false, 'Standards version', now(), false);

INSERT INTO listitem(
    id, listid, value, fixed, enabled, displayorder, deleted, lastmodifieddate)
VALUES 
    (134, 134, 'M100 - Version 35', true, true, 1, false, now()),
    (135, 134, 'EUCAST - Version 15', true, true, 1, false, now());