-- Admission and Request record views: topics, queue taxonomy, role permissions and language keys.
-- Id allocation continues after 130-neoshield-admission-request.sql (list 74 highest 2182).

-- ---------------------------------------------------------------------------
-- TopicTranslation (listitemname matches Event.Topic)
-- ---------------------------------------------------------------------------

INSERT INTO TopicTranslation (Id, listitemname, displayedtopic)
SELECT 39, 'Admission', 'Admission'
WHERE NOT EXISTS (SELECT 1 FROM TopicTranslation WHERE listitemname = 'Admission');

INSERT INTO TopicTranslation (Id, listitemname, displayedtopic)
SELECT 40, 'Request', 'Request'
WHERE NOT EXISTS (SELECT 1 FROM TopicTranslation WHERE listitemname = 'Request');

-- ---------------------------------------------------------------------------
-- Topic list (list 73)
-- ---------------------------------------------------------------------------

INSERT INTO listitem (id, listid, value, fixed, enabled, displayorder, deleted, lastmodifieddate)
SELECT 2183, 73, 'Admission', false, true, 1, false, now()
WHERE NOT EXISTS (SELECT 1 FROM listitem WHERE listid = 73 AND LOWER(value) = LOWER('Admission'));

INSERT INTO listitem (id, listid, value, fixed, enabled, displayorder, deleted, lastmodifieddate)
SELECT 2184, 73, 'Request', false, true, 1, false, now()
WHERE NOT EXISTS (SELECT 1 FROM listitem WHERE listid = 73 AND LOWER(value) = LOWER('Request'));

-- ---------------------------------------------------------------------------
-- Event list (list 74) under Admission topic (2183)
-- ---------------------------------------------------------------------------

INSERT INTO listitem (id, listid, value, fixed, enabled, displayorder, deleted, lastmodifieddate)
SELECT v.id, 74, v.value, false, true, 1, false, now()
FROM (VALUES
    (2185, 'editadmission'),
    (2186, 'deleteadmission'),
    (2187, 'viewadmissionrecord')
) AS v(id, value)
WHERE NOT EXISTS (SELECT 1 FROM listitem WHERE listid = 74 AND LOWER(TRIM(value)) = LOWER(TRIM(v.value)));

INSERT INTO listitemparentchild (parentid, childid, lastmodifieddate)
SELECT 2183, li.id, now()
FROM listitem li
WHERE li.listid = 74
  AND LOWER(li.value) IN ('editadmission', 'deleteadmission', 'viewadmissionrecord')
  AND NOT EXISTS (SELECT 1 FROM listitemparentchild WHERE parentid = 2183 AND childid = li.id);

-- ---------------------------------------------------------------------------
-- Event list (list 74) under Request topic (2184)
-- ---------------------------------------------------------------------------

INSERT INTO listitem (id, listid, value, fixed, enabled, displayorder, deleted, lastmodifieddate)
SELECT v.id, 74, v.value, false, true, 1, false, now()
FROM (VALUES
    (2188, 'editrequest'),
    (2189, 'deleterequest'),
    (2190, 'viewrequestrecord')
) AS v(id, value)
WHERE NOT EXISTS (SELECT 1 FROM listitem WHERE listid = 74 AND LOWER(TRIM(value)) = LOWER(TRIM(v.value)));

INSERT INTO listitemparentchild (parentid, childid, lastmodifieddate)
SELECT 2184, li.id, now()
FROM listitem li
WHERE li.listid = 74
  AND LOWER(li.value) IN ('editrequest', 'deleterequest', 'viewrequestrecord')
  AND NOT EXISTS (SELECT 1 FROM listitemparentchild WHERE parentid = 2184 AND childid = li.id);

-- ---------------------------------------------------------------------------
-- Role permissions — same cohort as neoshieldspecimen
-- ---------------------------------------------------------------------------

UPDATE role
SET eventpermission = jsonb_set(
    eventpermission::jsonb,
    '{AllowedEvents}',
    (eventpermission->'AllowedEvents')::jsonb
        || '["editadmission", "deleteadmission", "viewadmissionrecord", "editrequest", "deleterequest", "viewrequestrecord"]'::jsonb
)
WHERE (eventpermission->'AllowedEvents') @> '["neoshieldspecimen"]'::jsonb
  AND NOT (eventpermission->'AllowedEvents') @> '["editadmission"]'::jsonb;
