-- Configurable page rules.
-- Adds the editpagerules event so page visibility and page state can be configured from
-- Define Page Contents, and marks the one state that is safe for a user to change.
--
-- Topic: Configuration (402). Id 2200 sits clear of every block already handed out, the highest of
-- which is 2184 in v3.00.01/002-admission-request-record-view.sql. The id falls back to one past the
-- current maximum if an environment has already taken 2200, so the insert cannot break the run.

INSERT INTO listitem (id, listid, value, fixed, enabled, displayorder, deleted, lastmodifieddate)
SELECT 2194, 74, 'editpagerules', false, true, 1, false, now()
WHERE NOT EXISTS (SELECT 1 FROM listitem WHERE listid = 74 AND LOWER(TRIM(value)) = LOWER(TRIM('editpagerules')));

INSERT INTO listitemparentchild (parentid, childid, lastmodifieddate)
SELECT 402, li.id, now()
FROM listitem li
WHERE li.listid = 74 AND LOWER(li.value) = LOWER('editpagerules')
  AND NOT EXISTS (SELECT 1 FROM listitemparentchild WHERE parentid = 402 AND childid = li.id);

-- Grant editpagerules to every role that can already edit a form group, because page rules are
-- configured from the same screen and by the same people.
UPDATE role
SET eventpermission = jsonb_set(
    eventpermission::jsonb,
    '{AllowedEvents}',
    (eventpermission->'AllowedEvents')::jsonb || '["editpagerules"]'::jsonb
)
WHERE (eventpermission->'AllowedEvents') @> '["editformgroup"]'::jsonb
  AND NOT (eventpermission->'AllowedEvents') @> '["editpagerules"]'::jsonb;

-- A state may only be changed by a user when the on click state that declares it is marked
-- configurable. Every other state is an integral part of the system and stays read only.
-- bloodspecimen on the Neoshield specimen page is the first state opened up, so bring any stored
-- override of that page into step with the C# definition. PascalCase and camelCase keys are both
-- handled because stored overrides can be written by either the seed or the configuration tools.

UPDATE configs
SET contents = jsonb_set(
    contents,
    '{NextButton,OnClickState,Configurable}',
    '"Yes"'::jsonb,
    true
)
WHERE configname = 'neoshieldspecimenpage'
  AND contents #> '{NextButton,OnClickState,State}' IS NOT NULL
  AND contents #>> '{NextButton,OnClickState,Configurable}' IS DISTINCT FROM 'Yes';

UPDATE configs
SET contents = jsonb_set(
    contents,
    '{nextButton,onclickstate,configurable}',
    '"Yes"'::jsonb,
    true
)
WHERE configname = 'neoshieldspecimenpage'
  AND contents #> '{nextButton,onclickstate,state}' IS NOT NULL
  AND contents #>> '{nextButton,onclickstate,configurable}' IS DISTINCT FROM 'Yes';
