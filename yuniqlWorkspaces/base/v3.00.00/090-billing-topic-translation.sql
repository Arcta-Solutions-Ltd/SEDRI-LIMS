-- TopicTranslation for Billing (listitemname matches Event Topic for role UI TranslatedTopic)
INSERT INTO TopicTranslation (Id, listitemname, displayedtopic)
SELECT 36, 'Billing', 'Billing'
WHERE NOT EXISTS (SELECT 1 FROM TopicTranslation WHERE listitemname = 'Billing');

-- Add Billing to Topic list (list 73) for menu/topic display
INSERT INTO listitem (id, listid, value, fixed, enabled, displayorder, deleted, lastmodifieddate)
SELECT 152, 73, 'Billing', false, true, 1, false, now()
WHERE NOT EXISTS (SELECT 1 FROM listitem WHERE listid = 73 AND value = 'Billing');
