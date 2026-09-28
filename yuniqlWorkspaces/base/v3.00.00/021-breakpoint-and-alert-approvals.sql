CREATE TABLE breakpointapproval
(
    Id SERIAL PRIMARY KEY,
    BreakpointId INT NOT NULL,
    DateRecorded TIMESTAMPTZ NOT NULL,
    RecordedBy VARCHAR(40),
    CodingStatusId INT,
    LastModifiedDate TIMESTAMPTZ NOT NULL
);

CREATE TABLE alertapproval
(
    Id SERIAL PRIMARY KEY,
    AlertId INT NOT NULL,
    DateRecorded TIMESTAMPTZ NOT NULL,
    RecordedBy VARCHAR(40),
    CodingStatusId INT,
    LastModifiedDate TIMESTAMPTZ NOT NULL
);


INSERT INTO list (id, name, grouping, parentid, common, description, lastmodifieddate,  deleted) 
OVERRIDING SYSTEM VALUE
VALUES (137, 'CodingApprovalStatus', 'System', null, true, 'Coding Approval Status', now(), false);

INSERT INTO listitem (id, listid, value, lastmodifieddate, fixed,  enabled,  displayorder,  deleted) 
OVERRIDING SYSTEM VALUE
VALUES
    ( 145, 137, 'Approved', now(), false, true, 1, false ),
    ( 146, 137, 'Rejected', now(), false, true, 1, false );