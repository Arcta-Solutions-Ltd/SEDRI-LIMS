CREATE TABLE expertruleapproval
(
    Id SERIAL PRIMARY KEY,
    ExpertRuleId INT NOT NULL,
    DateRecorded TIMESTAMPTZ NOT NULL,
    RecordedBy VARCHAR(40),
    CodingStatusId INT,
    LastModifiedDate TIMESTAMPTZ NOT NULL
);

-- Add addexpertruleapproval event to OrganisationAdmin role (id 2)
UPDATE role
SET eventpermission = jsonb_set(
  eventpermission::jsonb,
  array['AllowedEvents'],
  (eventpermission->'AllowedEvents')::jsonb || '["addexpertruleapproval"]'::jsonb)
WHERE id = 2;
