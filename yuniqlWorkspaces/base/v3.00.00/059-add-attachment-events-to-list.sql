-- Add managespecimenattachments and managecultureattachments events to list 74 (Event)
-- so MessageQueue.AddAsync can resolve event ids for the queue (fixes "Missing event id" warning).
-- Links events to Specimen (620) and Culture (668) topics via listitemparentchild.
INSERT INTO listitem (id, listid, value, fixed, enabled, displayorder, deleted, lastmodifieddate)
SELECT 1520, 74, 'managespecimenattachments', false, true, 1, false, now()
WHERE NOT EXISTS (SELECT 1 FROM listitem WHERE listid = 74 AND LOWER(value) = 'managespecimenattachments');

INSERT INTO listitem (id, listid, value, fixed, enabled, displayorder, deleted, lastmodifieddate)
SELECT 1521, 74, 'managecultureattachments', false, true, 1, false, now()
WHERE NOT EXISTS (SELECT 1 FROM listitem WHERE listid = 74 AND LOWER(value) = 'managecultureattachments');

-- Link managespecimenattachments to Specimen topic (620)
INSERT INTO listitemparentchild (parentid, childid, lastmodifieddate)
SELECT 620, li.id, now()
FROM listitem li
WHERE li.listid = 74 AND LOWER(li.value) = 'managespecimenattachments'
  AND NOT EXISTS (SELECT 1 FROM listitemparentchild WHERE parentid = 620 AND childid = li.id);

-- Link managecultureattachments to Culture topic (668)
INSERT INTO listitemparentchild (parentid, childid, lastmodifieddate)
SELECT 668, li.id, now()
FROM listitem li
WHERE li.listid = 74 AND LOWER(li.value) = 'managecultureattachments'
  AND NOT EXISTS (SELECT 1 FROM listitemparentchild WHERE parentid = 668 AND childid = li.id);
