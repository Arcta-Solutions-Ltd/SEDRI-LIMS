-- Add TopicTranslation for Tags (listitemname matches Topic.Name for display)
INSERT INTO TopicTranslation (Id, listitemname, displayedtopic)
SELECT 35, 'Tags', 'Tags'
WHERE NOT EXISTS (SELECT 1 FROM TopicTranslation WHERE listitemname = 'Tags');

-- Add Tags to Topic list (list 73) for menu/topic display
INSERT INTO listitem (id, listid, value, fixed, enabled, displayorder, deleted, lastmodifieddate)
SELECT 151, 73, 'Tags', false, true, 1, false, now()
WHERE NOT EXISTS (SELECT 1 FROM listitem WHERE listid = 73 AND value = 'Tags');
