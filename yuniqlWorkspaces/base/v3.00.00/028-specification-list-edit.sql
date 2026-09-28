

-- Add TopicTranslation for Specification (listitemname matches Topic.Name for display)
INSERT INTO TopicTranslation (Id, listitemname, displayedtopic)
SELECT 34, 'Specification', 'Specification'
WHERE NOT EXISTS (SELECT 1 FROM TopicTranslation WHERE listitemname = 'Specification');

-- Add Specification to Topic list (list 73) for menu/topic display
INSERT INTO listitem (id, listid, value, fixed, enabled, displayorder, deleted, lastmodifieddate)
SELECT 148, 73, 'Specification', false, true, 1, false, now()
WHERE NOT EXISTS (SELECT 1 FROM listitem WHERE listid = 73 AND value = 'Specification');

-- 2. Create DocumentType (135) and PublicationYear (136) lists if not exist
INSERT INTO List (id, name, grouping, common, description, lastmodifieddate, deleted)
SELECT 135, 'DocumentType', 'System', false, 'Document type for specifications', now(), false
WHERE NOT EXISTS (SELECT 1 FROM List WHERE Id = 135 OR Name = 'DocumentType');

INSERT INTO List (id, name, grouping, common, description, lastmodifieddate, deleted)
SELECT 136, 'PublicationYear', 'System', false, 'Publication year for specifications', now(), false
WHERE NOT EXISTS (SELECT 1 FROM List WHERE Id = 136 OR Name = 'PublicationYear');

-- 3. Set Common = false for specification-related lists (edited from Specification list view only)
UPDATE list SET common = false
WHERE name IN ('Guidelines', 'DocumentType', 'Version', 'PublicationYear');


