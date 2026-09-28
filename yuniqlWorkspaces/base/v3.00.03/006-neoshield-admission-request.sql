-- Neoshield neonatal specimen request.
--
-- Introduces Admission and Request between Patient and Specimen. Both links on Specimen are nullable so the
-- existing patient-to-specimen forms are unaffected and all four shapes are supported: patient only,
-- patient + admission, patient + admission + request, and patient + request with no admission.
--
-- Id allocation (continues after 129-specimen-growth-internal-hierarchy):
--   list      146-169  (previous highest 145 in 125-ast-susceptibility-override-canned-list)
--   listitem  1900-2000 (the gap above 1886 in 126-queue-taxonomy-remaining-events) and 2103-2182
--             (2001-2102 are the organism groupings on list 77, so the allocation resumes above them)

-- ---------------------------------------------------------------------------
-- Tables
-- ---------------------------------------------------------------------------

CREATE TABLE IF NOT EXISTS Admission (
    Id SERIAL PRIMARY KEY,
    PatientId INT NOT NULL REFERENCES Patient (Id),
    MoreData JSONB,
    LastModifiedDate TIMESTAMPTZ NOT NULL
);

CREATE TABLE IF NOT EXISTS Request (
    Id SERIAL PRIMARY KEY,
    PatientId INT NOT NULL REFERENCES Patient (Id),
    AdmissionId INT REFERENCES Admission (Id),
    RequestId VARCHAR(30),
    MoreData JSONB,
    LastModifiedDate TIMESTAMPTZ NOT NULL
);

-- Human readable request reference. A sequence keeps generation atomic under concurrent requests, which a
-- max-plus-one read inside the creation transaction would not be.
CREATE SEQUENCE IF NOT EXISTS request_reference_seq START 1;

ALTER TABLE Specimen ADD COLUMN IF NOT EXISTS AdmissionId INT REFERENCES Admission (Id);
ALTER TABLE Specimen ADD COLUMN IF NOT EXISTS RequestId INT REFERENCES Request (Id);

CREATE INDEX IF NOT EXISTS ix_admission_patientid ON Admission (PatientId);
CREATE INDEX IF NOT EXISTS ix_request_patientid ON Request (PatientId);
CREATE INDEX IF NOT EXISTS ix_request_admissionid ON Request (AdmissionId);
CREATE UNIQUE INDEX IF NOT EXISTS ux_request_requestid ON Request (RequestId) WHERE RequestId IS NOT NULL;
CREATE INDEX IF NOT EXISTS ix_specimen_admissionid ON Specimen (AdmissionId);
CREATE INDEX IF NOT EXISTS ix_specimen_requestid ON Specimen (RequestId);

-- ---------------------------------------------------------------------------
-- Additions to existing lists
-- ---------------------------------------------------------------------------

-- Specimen types (list 4) needed by the neonatal form that are not already present.
-- Blood (808), Cerebrospinal fluid (809), Ear (810), Eye (811), Genito-urinary (813),
-- Lower respiratory tract (814), Other sterile fluids (815), Skin/Wound (818) and Urine (820) are reused.
INSERT INTO listitem (id, listid, value, fixed, enabled, displayorder, deleted, lastmodifieddate)
SELECT v.id, 4, v.value, false, true, v.displayorder, false, now()
FROM (VALUES
    (1900, 'Tracheal aspirate', 20),
    (1901, 'Umbilical or cord swab', 21),
    (1902, 'Rectal or perianal swab', 22),
    (1903, 'Nasopharyngeal swab', 23),
    (1904, 'Catheter tip', 24),
    (1905, 'Breast milk', 25)
) AS v(id, value, displayorder)
WHERE NOT EXISTS (SELECT 1 FROM listitem WHERE id = v.id);

-- Gender (list 13) already holds Male (189), Female (190) and Unknown (872).
INSERT INTO listitem (id, listid, value, fixed, enabled, displayorder, deleted, lastmodifieddate)
SELECT 1906, 13, 'Ambiguous or indeterminate', false, true, 3, false, now()
WHERE NOT EXISTS (SELECT 1 FROM listitem WHERE id = 1906);

-- Anatomical specimen sites (list 5) for the neonatal form.
INSERT INTO listitem (id, listid, value, fixed, enabled, displayorder, deleted, lastmodifieddate)
SELECT v.id, 5, v.value, false, true, v.displayorder, false, now()
FROM (VALUES
    (1907, 'Umbilicus or cord stump', 1),
    (1908, 'Scalp', 2),
    (1909, 'Face', 3),
    (1910, 'Eye', 4),
    (1911, 'Ear', 5),
    (1912, 'Nose', 6),
    (1913, 'Mouth or oral cavity', 7),
    (1914, 'Neck', 8),
    (1915, 'Chest', 9),
    (1916, 'Axilla', 10),
    (1917, 'Abdomen', 11),
    (1918, 'Back', 12),
    (1919, 'Buttock', 13),
    (1920, 'Perineum or perianal', 14),
    (1921, 'Groin', 15),
    (1922, 'Upper limb', 16),
    (1923, 'Hand', 17),
    (1924, 'Lower limb', 18),
    (1925, 'Foot', 19),
    (1926, 'Surgical wound', 20),
    (1927, 'Cannula or line insertion site', 21),
    (1928, 'Drain site', 22),
    (1929, 'Other', 98),
    (1930, 'Unknown', 99)
) AS v(id, value, displayorder)
WHERE NOT EXISTS (SELECT 1 FROM listitem WHERE id = v.id);

-- Site is filtered by specimen type through listitemparentchild. Field 49 of the specification is hidden for
-- Blood, so the anatomical sites are attached to every other neonatal specimen type.
INSERT INTO listitemparentchild (parentid, childid, displayorder, lastmodifieddate)
SELECT p.parentid, c.childid, 0, now()
FROM (VALUES (809), (810), (811), (813), (814), (815), (818), (820),
             (1900), (1901), (1902), (1903), (1904), (1905)) AS p(parentid)
CROSS JOIN (SELECT generate_series(1907, 1930) AS childid) AS c
WHERE NOT EXISTS (
    SELECT 1 FROM listitemparentchild WHERE parentid = p.parentid AND childid = c.childid
);

-- ---------------------------------------------------------------------------
-- New lists
-- ---------------------------------------------------------------------------

INSERT INTO list (id, name, grouping, parentid, common, description, lastmodifieddate, deleted)
SELECT v.id, v.name, v.grouping, null, true, v.description, now(), false
FROM (VALUES
    (146, 'NeoYesNo', 'Neoshield', 'Yes or no'),
    (147, 'NeoYesNoUnknown', 'Neoshield', 'Yes, no or unknown'),
    (148, 'NeoBabyNamed', 'Neoshield', 'Whether the baby has been named'),
    (149, 'NeoDateTimeKnown', 'Neoshield', 'Which parts of a date and time are known'),
    (150, 'NeoInbornOutborn', 'Neoshield', 'Inborn or outborn'),
    (151, 'NeoClinicianCadre', 'Neoshield', 'Requesting clinician cadre'),
    (152, 'NeoNotificationRoute', 'Neoshield', 'Preferred route for urgent result notification'),
    (153, 'NeoWard', 'Neoshield', 'Neonatal ward'),
    (154, 'NeoCotIdentifier', 'Neoshield', 'Cot or incubator identifier'),
    (155, 'NeoUrgency', 'Neoshield', 'Request urgency'),
    (156, 'NeoIndication', 'Neoshield', 'Indication for request'),
    (157, 'NeoWeightKnown', 'Neoshield', 'Which parts of the current weight record are known'),
    (158, 'NeoAntibioticTiming', 'Neoshield', 'Timing of most recent antibiotic dose'),
    (159, 'NeoAntibioticAgent', 'Neoshield', 'Antibiotic agents received'),
    (160, 'NeoRecentSurgery', 'Neoshield', 'Recent surgery'),
    (161, 'NeoCentralLineType', 'Neoshield', 'Type of central line in place'),
    (162, 'NeoRespiratorySupport', 'Neoshield', 'Respiratory support'),
    (163, 'NeoTestRequested', 'Neoshield', 'Tests requested on a specimen'),
    (164, 'NeoBodySide', 'Neoshield', 'Body side'),
    (165, 'NeoCollectionMethod', 'Neoshield', 'Blood collection method'),
    (166, 'NeoCollectedBy', 'Neoshield', 'Person who collected the specimen'),
    (167, 'NeoVolumeMethod', 'Neoshield', 'How the volume of blood in the bottle was determined'),
    (168, 'NeoBottleType', 'Neoshield', 'Blood culture bottle type'),
    (169, 'RequestFormList', 'System', 'Request forms that can restrict their specimen types')
) AS v(id, name, grouping, description)
WHERE NOT EXISTS (SELECT 1 FROM list WHERE id = v.id OR lower(name) = lower(v.name));

INSERT INTO listitem (id, listid, value, fixed, enabled, displayorder, deleted, lastmodifieddate)
SELECT v.id, v.listid, v.value, false, true, v.displayorder, false, now()
FROM (VALUES
    -- 146 NeoYesNo
    (1940, 146, 'Yes', 1),
    (1941, 146, 'No', 2),
    -- 147 NeoYesNoUnknown
    (1942, 147, 'Yes', 1),
    (1943, 147, 'No', 2),
    (1944, 147, 'Unknown', 99),
    -- 148 NeoBabyNamed
    (1945, 148, 'Named', 1),
    (1946, 148, 'Not yet named', 2),
    -- 149 NeoDateTimeKnown
    (1947, 149, 'Date known', 1),
    (1948, 149, 'Time known', 2),
    (1949, 149, 'Unknown', 99),
    -- 150 NeoInbornOutborn
    (1950, 150, 'Inborn', 1),
    (1951, 150, 'Outborn - home birth', 2),
    (1952, 150, 'Outborn - other facility', 3),
    (1953, 150, 'Outborn - born in transit', 4),
    (1954, 150, 'Unknown', 99),
    -- 151 NeoClinicianCadre
    (1955, 151, 'Consultant or specialist', 1),
    (1956, 151, 'Medical officer', 2),
    (1957, 151, 'Registrar or resident', 3),
    (1958, 151, 'Clinical officer', 4),
    (1959, 151, 'Nurse', 5),
    (1960, 151, 'Midwife', 6),
    (1961, 151, 'Intern or student', 7),
    (1962, 151, 'Other', 98),
    (1963, 151, 'Unknown', 99),
    -- 152 NeoNotificationRoute
    (1964, 152, 'Ward telephone', 1),
    (1965, 152, 'Clinician bleep', 2),
    (1966, 152, 'Clinician mobile', 3),
    (1967, 152, 'SMS to ward phone', 4),
    (1968, 152, 'WhatsApp group', 5),
    (1969, 152, 'In person to the ward', 6),
    (1970, 152, 'Other', 98),
    -- 153 NeoWard
    (1971, 153, 'NICU', 1),
    (1972, 153, 'SCBU or special care', 2),
    (1973, 153, 'Kangaroo mother care', 3),
    (1974, 153, 'Postnatal ward', 4),
    (1975, 153, 'Paediatric ward', 5),
    (1976, 153, 'Isolation room', 6),
    (1977, 153, 'Other', 98),
    (1978, 153, 'Unknown', 99),
    -- 154 NeoCotIdentifier (site configured through table maintenance)
    (1979, 154, 'Unknown', 99),
    -- 155 NeoUrgency
    (1980, 155, 'Routine', 1),
    (1981, 155, 'Urgent', 2),
    (1982, 155, 'Critically unwell - process immediately', 3),
    (1983, 155, 'Unknown', 99),
    -- 156 NeoIndication
    (1984, 156, 'Suspected sepsis, early onset (under 72 hours)', 1),
    (1985, 156, 'Suspected sepsis, late onset (72 hours or more)', 2),
    (1986, 156, 'Meningitis', 3),
    (1987, 156, 'Necrotising enterocolitis', 4),
    (1988, 156, 'Pneumonia', 5),
    (1989, 156, 'Urinary tract infection', 6),
    (1990, 156, 'Suspected line infection', 7),
    (1991, 156, 'Surgical site or wound infection', 8),
    (1992, 156, 'Omphalitis', 9),
    (1993, 156, 'Skin or soft tissue infection', 10),
    (1994, 156, 'Congenital infection work-up', 11),
    (1995, 156, 'Clinical deterioration, source unclear', 12),
    (1996, 156, 'Treatment failure on current antibiotics', 13),
    (1997, 156, 'Test of cure or clearance', 14),
    (1998, 156, 'Colonisation screening', 15),
    (1999, 156, 'Outbreak investigation', 16),
    (2000, 156, 'Other', 98),
    -- 157 NeoWeightKnown
    (2103, 157, 'Current weight known', 1),
    (2104, 157, 'Date of current weight known', 2),
    (2105, 157, 'Unknown', 99),
    -- 158 NeoAntibioticTiming
    (2106, 158, 'At the time of collection', 1),
    (2107, 158, 'Within the previous 24 hours', 2),
    (2108, 158, '1 to 14 days before collection', 3),
    (2109, 158, 'More than 14 days before collection', 4),
    (2110, 158, 'Unknown', 99),
    -- 159 NeoAntibioticAgent
    (2111, 159, 'Ampicillin', 1),
    (2112, 159, 'Benzylpenicillin', 2),
    (2113, 159, 'Cloxacillin or flucloxacillin', 3),
    (2114, 159, 'Gentamicin', 4),
    (2115, 159, 'Amikacin', 5),
    (2116, 159, 'Cefotaxime', 6),
    (2117, 159, 'Ceftriaxone', 7),
    (2118, 159, 'Ceftazidime', 8),
    (2119, 159, 'Vancomycin', 9),
    (2120, 159, 'Meropenem', 10),
    (2121, 159, 'Piperacillin-tazobactam', 11),
    (2122, 159, 'Ciprofloxacin', 12),
    (2123, 159, 'Metronidazole', 13),
    (2124, 159, 'Fluconazole', 14),
    (2125, 159, 'Amphotericin B', 15),
    (2126, 159, 'Other', 98),
    (2127, 159, 'Unknown', 99),
    -- 160 NeoRecentSurgery
    (2128, 160, 'No', 1),
    (2129, 160, 'Yes, within the previous 7 days', 2),
    (2130, 160, 'Yes, 8 to 30 days before', 3),
    (2131, 160, 'Yes, more than 30 days before', 4),
    (2132, 160, 'Unknown', 99),
    -- 161 NeoCentralLineType
    (2133, 161, 'Umbilical venous catheter', 1),
    (2134, 161, 'Umbilical arterial catheter', 2),
    (2135, 161, 'Peripherally inserted central catheter', 3),
    (2136, 161, 'Tunnelled central line', 4),
    (2137, 161, 'Femoral or other percutaneous central line', 5),
    (2138, 161, 'Other', 98),
    (2139, 161, 'Unknown', 99),
    -- 162 NeoRespiratorySupport
    (2140, 162, 'None', 1),
    (2141, 162, 'Low-flow oxygen', 2),
    (2142, 162, 'High-flow nasal cannula', 3),
    (2143, 162, 'CPAP', 4),
    (2144, 162, 'Invasive mechanical ventilation', 5),
    (2145, 162, 'Unknown', 99),
    -- 163 NeoTestRequested
    (2146, 163, 'Culture with identification and susceptibility', 1),
    (2147, 163, 'Microscopy and Gram stain', 2),
    (2148, 163, 'Cell count and chemistry', 3),
    (2149, 163, 'Molecular or PCR panel', 4),
    (2150, 163, 'Fungal culture', 5),
    (2151, 163, 'Mycobacterial culture', 6),
    (2152, 163, 'Other', 98),
    -- 164 NeoBodySide
    (2153, 164, 'Left', 1),
    (2154, 164, 'Right', 2),
    (2155, 164, 'Midline', 3),
    (2156, 164, 'Not applicable', 98),
    (2157, 164, 'Unknown', 99),
    -- 165 NeoCollectionMethod
    (2158, 165, 'Peripheral venepuncture', 1),
    (2159, 165, 'Arterial puncture', 2),
    (2160, 165, 'Existing peripheral line', 3),
    (2161, 165, 'Newly inserted peripheral line', 4),
    (2162, 165, 'Existing central line', 5),
    (2163, 165, 'Newly inserted central line', 6),
    (2164, 165, 'Umbilical catheter at insertion', 7),
    (2165, 165, 'Heel prick', 8),
    (2166, 165, 'Unknown', 99),
    -- 166 NeoCollectedBy (site configured through table maintenance)
    (2167, 166, 'Unknown', 99),
    -- 167 NeoVolumeMethod
    (2168, 167, 'Bottle weighed before and after inoculation', 1),
    (2169, 167, 'Estimated visually by the collector', 2),
    (2170, 167, 'Not determined', 3),
    -- 168 NeoBottleType
    (2171, 168, 'Paediatric aerobic', 1),
    (2172, 168, 'Standard aerobic', 2),
    (2173, 168, 'Anaerobic', 3),
    (2174, 168, 'Mycobacterial or fungal', 4),
    (2175, 168, 'Manual broth bottle', 5),
    (2176, 168, 'Unknown', 99),
    -- 169 RequestFormList
    (2177, 169, 'createneoshieldspecimenform', 1),
    (2178, 169, 'createneoshieldspecimenforpatientform', 2)
) AS v(id, listid, value, displayorder)
WHERE NOT EXISTS (SELECT 1 FROM listitem WHERE id = v.id);

-- Tests requested are filtered by specimen type. Culture, microscopy, molecular and other apply to every
-- neonatal specimen type; the remainder are attached only where they are clinically meaningful.
INSERT INTO listitemparentchild (parentid, childid, displayorder, lastmodifieddate)
SELECT p.parentid, c.childid, 0, now()
FROM (VALUES (808), (809), (810), (811), (813), (814), (815), (818), (820),
             (1900), (1901), (1902), (1903), (1904), (1905)) AS p(parentid)
CROSS JOIN (VALUES (2146), (2147), (2149), (2152)) AS c(childid)
WHERE NOT EXISTS (
    SELECT 1 FROM listitemparentchild WHERE parentid = p.parentid AND childid = c.childid
);

INSERT INTO listitemparentchild (parentid, childid, displayorder, lastmodifieddate)
SELECT v.parentid, v.childid, 0, now()
FROM (VALUES
    -- Cell count and chemistry: cerebrospinal fluid, other sterile fluids, urine
    (809, 2148), (815, 2148), (820, 2148),
    -- Fungal culture: blood, cerebrospinal fluid, lower respiratory tract, tracheal aspirate
    (808, 2150), (809, 2150), (814, 2150), (1900, 2150),
    -- Mycobacterial culture: blood, lower respiratory tract, tracheal aspirate
    (808, 2151), (814, 2151), (1900, 2151)
) AS v(parentid, childid)
WHERE NOT EXISTS (
    SELECT 1 FROM listitemparentchild WHERE parentid = v.parentid AND childid = v.childid
);

-- ---------------------------------------------------------------------------
-- Queue taxonomy (list 74)
-- ---------------------------------------------------------------------------

INSERT INTO listitem (id, listid, value, fixed, enabled, displayorder, deleted, lastmodifieddate)
SELECT 2179, 74, 'neoshieldspecimen', false, true, 1, false, now()
WHERE NOT EXISTS (SELECT 1 FROM listitem WHERE listid = 74 AND LOWER(TRIM(value)) = LOWER(TRIM('neoshieldspecimen')));

INSERT INTO listitemparentchild (parentid, childid, lastmodifieddate)
SELECT 620, li.id, now()
FROM listitem li
WHERE li.listid = 74 AND LOWER(li.value) = LOWER('neoshieldspecimen')
  AND NOT EXISTS (SELECT 1 FROM listitemparentchild WHERE parentid = 620 AND childid = li.id);

INSERT INTO listitem (id, listid, value, fixed, enabled, displayorder, deleted, lastmodifieddate)
SELECT v.id, 74, v.value, false, true, 1, false, now()
FROM (VALUES
    (2180, 'addformspecimentypeoption'),
    (2181, 'editformspecimentypeoption'),
    (2182, 'deleteformspecimentypeoption')
) AS v(id, value)
WHERE NOT EXISTS (SELECT 1 FROM listitem WHERE listid = 74 AND LOWER(TRIM(value)) = LOWER(TRIM(v.value)));

INSERT INTO listitemparentchild (parentid, childid, lastmodifieddate)
SELECT 402, li.id, now()
FROM listitem li
WHERE li.listid = 74
  AND LOWER(li.value) IN ('addformspecimentypeoption', 'editformspecimentypeoption', 'deleteformspecimentypeoption')
  AND NOT EXISTS (SELECT 1 FROM listitemparentchild WHERE parentid = 402 AND childid = li.id);

-- ---------------------------------------------------------------------------
-- Specimen workflow entry
-- ---------------------------------------------------------------------------

-- A Neoshield request creates a specimen that has been asked for but not yet received, which is the state the
-- ordinary request form enters through remotespecimen. Without an entry condition the workflow yields no state
-- for the event and the specimen insert fails.
UPDATE configs
SET contents = jsonb_set(
        contents,
        '{EntryConditions}',
        (contents->'EntryConditions') || '[{"Events": "neoshieldspecimen", "Default": "525"}]'::jsonb
    ),
    lastmodifieddate = now()
WHERE configtypeid = 18
  AND LOWER(configname) = 'specimendefault'
  AND contents::text NOT LIKE '%neoshieldspecimen%';

-- ---------------------------------------------------------------------------
-- Role permissions
-- ---------------------------------------------------------------------------

-- Roles that can already create a received specimen may raise a Neoshield request.
UPDATE role
SET eventpermission = jsonb_set(
    eventpermission::jsonb,
    '{AllowedEvents}',
    (eventpermission->'AllowedEvents')::jsonb || '["neoshieldspecimen"]'::jsonb
)
WHERE (eventpermission->'AllowedEvents') @> '["newreceivedspecimen"]'::jsonb
  AND NOT (eventpermission->'AllowedEvents') @> '["neoshieldspecimen"]'::jsonb;

-- Roles that maintain the culture type options may maintain the form specimen type options.
UPDATE role
SET eventpermission = jsonb_set(
    eventpermission::jsonb,
    '{AllowedEvents}',
    (eventpermission->'AllowedEvents')::jsonb
        || '["addformspecimentypeoption", "editformspecimentypeoption", "deleteformspecimentypeoption"]'::jsonb
)
WHERE (eventpermission->'AllowedEvents') @> '["addspecimentypeculturetypeoptionevent"]'::jsonb
  AND NOT (eventpermission->'AllowedEvents') @> '["addformspecimentypeoption"]'::jsonb;

-- ---------------------------------------------------------------------------
-- Language keys
-- ---------------------------------------------------------------------------

UPDATE language
SET pack = COALESCE(pack, '[]'::jsonb) || '[
    {"Key": "@NeoNeo@", "Value": "Neoshield"},
    {"Key": "@NeoNeoA@", "Value": "Raise a Neoshield neonatal specimen request"},
    {"Key": "@NeoPatIde@", "Value": "Patient Identification"},
    {"Key": "@NeoPatIdeA@", "Value": "Identify the baby and, where available, the linked maternal record"},
    {"Key": "@NeoMotAva@", "Value": "Is the mother''s record available to link?"},
    {"Key": "@NeoMotRef@", "Value": "Mother''s Patient Identifier"},
    {"Key": "@NeoBabNam@", "Value": "Has the baby been named?"},
    {"Key": "@NeoBabFir@", "Value": "First Name of Baby"},
    {"Key": "@NeoBabLas@", "Value": "Last Name of Baby"},
    {"Key": "@NeoBirAdm@", "Value": "Birth Details"},
    {"Key": "@NeoBirAdmA@", "Value": "Record what is known about the birth"},
    {"Key": "@NeoBirKno@", "Value": "Which parts of the date and time of birth are known?"},
    {"Key": "@NeoBirTim@", "Value": "Time of Birth"},
    {"Key": "@NeoBirWeiAva@", "Value": "Is birth weight available?"},
    {"Key": "@NeoBirWei@", "Value": "Birth Weight (g)"},
    {"Key": "@NeoInbOut@", "Value": "Inborn or Outborn"},
    {"Key": "@NeoAdm@", "Value": "Admission"},
    {"Key": "@NeoAdmA@", "Value": "Record the admission for this baby"},
    {"Key": "@NeoAdmSel@", "Value": "Select Admission"},
    {"Key": "@NeoAdmSelA@", "Value": "Use an existing admission or create a new one"},
    {"Key": "@NeoAdmCre@", "Value": "Create a new admission"},
    {"Key": "@NeoAdmNon@", "Value": "This patient has no previous admissions"},
    {"Key": "@NeoAdmKno@", "Value": "Which parts of the date and time of admission are known?"},
    {"Key": "@NeoAdmDat@", "Value": "Date of Admission"},
    {"Key": "@NeoAdmTim@", "Value": "Time of Admission"},
    {"Key": "@NeoReq@", "Value": "Request"},
    {"Key": "@NeoReqA@", "Value": "Record the clinical decision to investigate"},
    {"Key": "@NeoReqSel@", "Value": "Select Request"},
    {"Key": "@NeoReqSelA@", "Value": "Use an existing request or create a new one"},
    {"Key": "@NeoReqCre@", "Value": "Create a new request"},
    {"Key": "@NeoReqNon@", "Value": "There are no previous requests"},
    {"Key": "@NeoReqRef@", "Value": "Request Identifier"},
    {"Key": "@NeoReqHea@", "Value": "Request Header"},
    {"Key": "@NeoReqDat@", "Value": "Date of Request"},
    {"Key": "@NeoReqTim@", "Value": "Time of Request"},
    {"Key": "@NeoCliNam@", "Value": "Requesting Clinician"},
    {"Key": "@NeoCliCad@", "Value": "Requesting Clinician Cadre"},
    {"Key": "@NeoCliCon@", "Value": "Requesting Clinician Contact Number"},
    {"Key": "@NeoWarTel@", "Value": "Ward Telephone Number"},
    {"Key": "@NeoNotRou@", "Value": "Preferred Route for Urgent Result Notification"},
    {"Key": "@NeoWar@", "Value": "Ward"},
    {"Key": "@NeoCotAva@", "Value": "Is a cot or incubator identifier available?"},
    {"Key": "@NeoCot@", "Value": "Cot or Incubator Identifier"},
    {"Key": "@NeoUrg@", "Value": "Urgency"},
    {"Key": "@NeoInd@", "Value": "Indication for Request"},
    {"Key": "@NeoCliSta@", "Value": "Clinical State at Request"},
    {"Key": "@NeoCliStaA@", "Value": "Record the clinical state of the baby at the time of this request"},
    {"Key": "@NeoWeiKno@", "Value": "Which parts of the current weight record are known?"},
    {"Key": "@NeoCurWei@", "Value": "Current Weight (g)"},
    {"Key": "@NeoCurWeiDat@", "Value": "Date Current Weight Measured"},
    {"Key": "@NeoAntRec@", "Value": "Has the baby received any antibiotic before this request?"},
    {"Key": "@NeoAntTim@", "Value": "Timing of Most Recent Antibiotic Dose"},
    {"Key": "@NeoAntAge@", "Value": "Antibiotic Agents Received"},
    {"Key": "@NeoAntStaKno@", "Value": "Which parts of the antibiotic course start date and time are known?"},
    {"Key": "@NeoAntStaDat@", "Value": "Date Current Antibiotic Course Started"},
    {"Key": "@NeoAntStaTim@", "Value": "Time Current Antibiotic Course Started"},
    {"Key": "@NeoRecSur@", "Value": "Recent Surgery"},
    {"Key": "@NeoCenLin@", "Value": "Is a central line in place?"},
    {"Key": "@NeoCenLinTyp@", "Value": "Type of Central Line in Place"},
    {"Key": "@NeoResSup@", "Value": "Respiratory Support"},
    {"Key": "@NeoSpe@", "Value": "Specimen"},
    {"Key": "@NeoSpeA@", "Value": "Record the specimen taken for this request"},
    {"Key": "@NeoTesReq@", "Value": "Tests Requested on this Specimen"},
    {"Key": "@NeoBodSid@", "Value": "Body Side"},
    {"Key": "@NeoColMet@", "Value": "Collection Method"},
    {"Key": "@NeoColBy@", "Value": "Person who Collected the Specimen"},
    {"Key": "@NeoSpeLab@", "Value": "Specimen Labelled"},
    {"Key": "@NeoBot@", "Value": "Blood Culture Bottle"},
    {"Key": "@NeoBotA@", "Value": "Record the bottle used for this blood culture"},
    {"Key": "@NeoVolMet@", "Value": "How was the volume of blood in the bottle determined?"},
    {"Key": "@NeoBotTyp@", "Value": "Bottle Type"},
    {"Key": "@NeoBotWei@", "Value": "Weight of Bottle Before Inoculation (g)"},
    {"Key": "@NeoEstVol@", "Value": "Estimated Volume of Blood in Bottle (ml)"},
    {"Key": "@NeoAddAno@", "Value": "Add another specimen"},
    {"Key": "@NeoAddAnoA@", "Value": "The specimen has been saved. Do you want to add another specimen to this request?"},
    {"Key": "@NeoValSpeTyp@", "Value": "A specimen type must be selected"},
    {"Key": "@NeoValTes@", "Value": "At least one test must be requested"},
    {"Key": "@NeoValWar@", "Value": "A ward must be selected"},
    {"Key": "@NeoValInd@", "Value": "An indication for the request must be selected"},
    {"Key": "@NeoValUrg@", "Value": "An urgency must be selected"},
    {"Key": "@ConFormSpe@", "Value": "Form Specimen Type Options"},
    {"Key": "@ConFormSpeA@", "Value": "Restrict which specimen types are offered on a request form"},
    {"Key": "@ConFormSpeAdd@", "Value": "Add Form Specimen Type Option"},
    {"Key": "@ConFormSpeEdi@", "Value": "Edit Form Specimen Type Option"},
    {"Key": "@ConFormSpeDel@", "Value": "Delete Form Specimen Type Option"},
    {"Key": "@ConFormSpeDelA@", "Value": "Delete this form specimen type option"},
    {"Key": "@ConFormNam@", "Value": "Request Form"}
]'::jsonb,
    lastmodifieddate = now()
WHERE translationid = 669
  AND NOT EXISTS (
    SELECT 1
    FROM jsonb_array_elements(COALESCE(pack, '[]'::jsonb)) AS elem
    WHERE elem->>'Key' = '@NeoNeo@'
  );

-- ---------------------------------------------------------------------------
-- Default laboratory configuration
-- ---------------------------------------------------------------------------

-- Restrict the Neoshield forms to the neonatal specimen types for laboratory 1. Sites change this through
-- Administration > Laboratories > Form Specimen Type Options.
INSERT INTO laboratoryconfigs (laboratoryid, configname, contents, lastmodifieddate)
SELECT 1, 'formspecimentypeoption',
       '{"GroupId": "createneoshieldspecimenform", "AssociatedListId": "808,809,810,811,813,814,815,818,820,1900,1901,1902,1903,1904,1905"}'::jsonb,
       now()
WHERE NOT EXISTS (
    SELECT 1 FROM laboratoryconfigs
    WHERE laboratoryid = 1 AND configname = 'formspecimentypeoption'
      AND contents->>'GroupId' = 'createneoshieldspecimenform'
);

INSERT INTO laboratoryconfigs (laboratoryid, configname, contents, lastmodifieddate)
SELECT 1, 'formspecimentypeoption',
       '{"GroupId": "createneoshieldspecimenforpatientform", "AssociatedListId": "808,809,810,811,813,814,815,818,820,1900,1901,1902,1903,1904,1905"}'::jsonb,
       now()
WHERE NOT EXISTS (
    SELECT 1 FROM laboratoryconfigs
    WHERE laboratoryid = 1 AND configname = 'formspecimentypeoption'
      AND contents->>'GroupId' = 'createneoshieldspecimenforpatientform'
);
