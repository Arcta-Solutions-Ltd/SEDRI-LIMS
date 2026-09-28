-- AST manual susceptibility override audit: lab toggle, canned reason catalog, per-line override rows.
-- Run as a new migration; does not amend existing scripts.

ALTER TABLE laboratory
  ADD COLUMN IF NOT EXISTS recordsusceptibilitychangeaudit VARCHAR(3) NOT NULL DEFAULT 'No';

-- Canned override reasons: list 145 ASTSusceptibilityOverrideCannedComments (see 125-ast-susceptibility-override-canned-list.sql).

CREATE TABLE IF NOT EXISTS astsusceptibilityoverride (
  id SERIAL PRIMARY KEY,
  cultureid INT NOT NULL,
  testmethodid INT NOT NULL,
  antibioticid INT NOT NULL,
  guidelinesid INT NOT NULL,
  dosage INT NOT NULL DEFAULT 0,
  specialconsiderationid INT NOT NULL DEFAULT 0,
  ismanuallyset VARCHAR(3) NOT NULL DEFAULT 'Yes',
  setby VARCHAR(80),
  setat TIMESTAMPTZ,
  cannedcommentid INT NOT NULL DEFAULT 0,
  freetextcomment VARCHAR(500),
  overriddenfromsusceptibilityid INT NOT NULL DEFAULT 0,
  lastmodifieddate TIMESTAMPTZ NOT NULL DEFAULT now(),
  CONSTRAINT uq_astsusceptibilityoverride_line
    UNIQUE (cultureid, testmethodid, antibioticid, guidelinesid, dosage, specialconsiderationid)
);

CREATE INDEX IF NOT EXISTS ix_astsusceptibilityoverride_cultureid
  ON astsusceptibilityoverride (cultureid);
