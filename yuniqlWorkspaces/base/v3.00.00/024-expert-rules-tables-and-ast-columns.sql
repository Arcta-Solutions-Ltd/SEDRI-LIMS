-- Expert Rules tables and AST column additions
-- Supports EUCAST/CLSI-style expert rules for AST interpretation.
-- Run as a new migration; does not amend existing scripts.

-- Main expert rule definition table
CREATE TABLE IF NOT EXISTS expertrule
(
    Id SERIAL PRIMARY KEY,
    ExpertRuleName VARCHAR(200),
    RuleText TEXT,
    OrderId INT,
    FamilyId INT,
    OrganismId INT,
    OrgGroupCodingId INT,
    CombinationRule VARCHAR(20),
    Enabled VARCHAR(3),
    AlertOnRule VARCHAR(3),
    SourceId INT,
    TagId VARCHAR(50),
    LastModifiedDate TIMESTAMPTZ NOT NULL
);

-- Conditions that must be met for a rule to apply (antibiotic + susceptibility etc.)
CREATE TABLE IF NOT EXISTS expertrulecondition
(
    Id SERIAL PRIMARY KEY,
    ExpertRuleId INT NOT NULL,
    AntibioticId INT,
    AntibioticGroupId INT,
    TestMethodId INT,
    SusceptibilityId INT,
    SpecialConsiderationId INT,
    StartVal NUMERIC(7, 3),
    EndVal NUMERIC(7, 3),
    LastModifiedDate TIMESTAMPTZ NOT NULL
);

-- Test-based conditions (field comparisons)
CREATE TABLE IF NOT EXISTS expertruletestcondition
(
    Id SERIAL PRIMARY KEY,
    ExpertRuleId INT NOT NULL,
    TestName VARCHAR(60),
    FieldName VARCHAR(60),
    Comparison VARCHAR(20),
    CompValue VARCHAR(100),
    LastModifiedDate TIMESTAMPTZ NOT NULL
);

-- Actions to apply when rule conditions match
CREATE TABLE IF NOT EXISTS expertruleaction
(
    Id SERIAL PRIMARY KEY,
    ExpertRuleId INT NOT NULL,
    ActionTypeId INT,
    AntibioticId INT,
    AntibioticGroupId INT,
    SusceptibilityId INT,
    DisplayOnReport VARCHAR(3),
    LastModifiedDate TIMESTAMPTZ NOT NULL
);

-- Specimen type include/exclude filters for rules
CREATE TABLE IF NOT EXISTS expertrulespecimentype
(
    Id SERIAL PRIMARY KEY,
    ExpertRuleId INT NOT NULL,
    SpecimenTypeId INT NOT NULL,
    Included BOOLEAN NOT NULL,
    LastModifiedDate TIMESTAMPTZ NOT NULL
);

-- Add expert rule columns to AST table (links AST rows to the rule that generated them)
ALTER TABLE ast ADD COLUMN IF NOT EXISTS expertruleid INT;
ALTER TABLE ast ADD COLUMN IF NOT EXISTS expertruleline BOOLEAN;

insert into topictranslation(id, listitemname, displayedtopic)
values(33,'ExpertRules', 'Expert Rules');

INSERT INTO listitem (id, listid, value, lastmodifieddate, fixed,  enabled,  displayorder,  deleted) 
OVERRIDING SYSTEM VALUE
VALUES
    ( 147, 73, 'ExpertRules', now(), false, true, 1, false );
