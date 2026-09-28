-- Ensure Specification topic has TopicTranslation for display.
-- When creating a Topic, a corresponding TopicTranslation record must be added.
-- Id 34, listitemname 'Specification' (matches Topic.Name), displayedtopic 'Specification'.
INSERT INTO topictranslation (id, listitemname, displayedtopic) OVERRIDING SYSTEM VALUE
VALUES (34, 'Specification', 'Specification')
ON CONFLICT (id) DO UPDATE SET listitemname = EXCLUDED.listitemname, displayedtopic = EXCLUDED.displayedtopic;
