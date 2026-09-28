-- Billing rules and billing records (laboratory-scoped).

CREATE TABLE BillingRule (
    Id SERIAL PRIMARY KEY,
    Name VARCHAR(200) NOT NULL,
    LaboratoryId INT NOT NULL REFERENCES Laboratory (Id),
    LastModifiedDate TIMESTAMPTZ NOT NULL
);

CREATE INDEX ix_billingrule_laboratoryid ON BillingRule (LaboratoryId);

CREATE TABLE BillingRecord (
    Id SERIAL PRIMARY KEY,
    LaboratoryId INT NOT NULL REFERENCES Laboratory (Id),
    PatientId INT NOT NULL REFERENCES Patient (Id),
    SpecimenId INT NOT NULL REFERENCES Specimen (Id),
    CultureId INT REFERENCES Culture (Id),
    DirectTestName VARCHAR(200) NOT NULL,
    TestPatternId INT NOT NULL REFERENCES TestPattern (Id),
    Description VARCHAR(2000),
    TotalAmount NUMERIC(18, 2) NOT NULL,
    LastModifiedDate TIMESTAMPTZ NOT NULL
);

CREATE INDEX ix_billingrecord_laboratoryid ON BillingRecord (LaboratoryId);

-- Menu: roles that can see billing rules also get billing records list.
UPDATE role
SET menupermission = jsonb_set(
    menupermission,
    '{AllowedSidebarItems}',
    (menupermission->'AllowedSidebarItems') || '["billingrecord"]'::jsonb
)
WHERE (menupermission->'AllowedSidebarItems') @> '["billingrule"]'::jsonb
  AND NOT (menupermission->'AllowedSidebarItems') @> '["billingrecord"]'::jsonb;

-- Events for billing CRUD (lowercase EventName matches app configs).
UPDATE role
SET eventpermission = jsonb_set(
    eventpermission::jsonb,
    '{AllowedEvents}',
    (eventpermission->'AllowedEvents')::jsonb
        || '["addbillingrule","deletebillingrule","editbillingrecord","editbillingrule","deletebillingrecord"]'::jsonb
)
WHERE (menupermission->'AllowedSidebarItems') @> '["billingrule"]'::jsonb
  AND eventpermission IS NOT NULL
  AND (eventpermission->'AllowedEvents') IS NOT NULL
  AND NOT (eventpermission->'AllowedEvents') @> '["addbillingrule"]'::jsonb;
