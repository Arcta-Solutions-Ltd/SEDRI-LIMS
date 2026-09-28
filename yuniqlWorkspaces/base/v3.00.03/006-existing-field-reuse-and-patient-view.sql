-- Existing field reuse and patient view configuration.
--
-- Adds the addexistingfield event so fields already used on one form can be referenced onto
-- another, and opens the patients view for form configuration so patient fields captured on the
-- specimen view create forms can also be placed on the edit patient form.
--
-- Referencing a field is a configuration only change. The field keeps its original id, which is
-- also its storage key, so no column is added and namelist is untouched. That is why nothing here
-- alters a data table.
--
-- Topic: Configuration (402). Id 2201 sits one past 2200, taken by
-- v3.00.03/001-page-rules-configuration.sql. The id falls back to one past the current maximum if
-- an environment has already taken 2201, so the insert cannot break the run.

-- ---------------------------------------------------------------------------
-- 1. Queue taxonomy: addexistingfield in the Event list (74) under Configuration
-- ---------------------------------------------------------------------------

INSERT INTO listitem (id, listid, value, fixed, enabled, displayorder, deleted, lastmodifieddate)
SELECT 2195, 74, 'addexistingfield', false, true, 1, false, now()
WHERE NOT EXISTS (SELECT 1 FROM listitem WHERE listid = 74 AND LOWER(TRIM(value)) = LOWER(TRIM('addexistingfield')));

INSERT INTO listitemparentchild (parentid, childid, lastmodifieddate)
SELECT 402, li.id, now()
FROM listitem li
WHERE li.listid = 74 AND LOWER(li.value) = LOWER('addexistingfield')
  AND NOT EXISTS (SELECT 1 FROM listitemparentchild WHERE parentid = 402 AND childid = li.id);

-- ---------------------------------------------------------------------------
-- 2. Grant addexistingfield to every role that can already add a field, because
--    referencing an existing field is done from the same screen by the same people.
-- ---------------------------------------------------------------------------

UPDATE role
SET eventpermission = jsonb_set(
    eventpermission::jsonb,
    '{AllowedEvents}',
    (eventpermission->'AllowedEvents')::jsonb || '["addexistingfield"]'::jsonb
)
WHERE (eventpermission->'AllowedEvents') @> '["addfield"]'::jsonb
  AND NOT (eventpermission->'AllowedEvents') @> '["addexistingfield"]'::jsonb;

-- ---------------------------------------------------------------------------
-- 3. Register the patients view so it appears in Configuration > Views.
--    Empty contents so ListViewConfigFactory falls back to PatientListViewConfig.
--    Only the Forms region shows for this view, because direct tests, culture tests,
--    reports and workflows belong to specimens.
-- ---------------------------------------------------------------------------

-- The id comes from configs_id_seq, which the seed restarts at 10000, so it stays clear of the
-- explicitly numbered seed rows.
INSERT INTO configs (configname, configtypeid, contents, lastmodifieddate)
SELECT 'patients', 5, '{}'::jsonb, now()
WHERE NOT EXISTS (SELECT 1 FROM configs WHERE LOWER(configname) = 'patients' AND configtypeid = 5);

-- ---------------------------------------------------------------------------
-- 4. Make the edit patient form configurable in stored overrides.
--    Only the edit form is opened up; adding a patient stays fixed. Installs with no stored
--    override pick this up from EditPatientFormConfig instead, so only non-empty rows are touched.
--    Both key casings are handled because stored overrides can be written by the seed or by the
--    configuration tools.
-- ---------------------------------------------------------------------------

UPDATE configs
SET contents = contents
        || jsonb_build_object('configurable', 'Yes')
        || jsonb_build_object('singleItemName', 'patient')
        || jsonb_build_object('configureactions', '["edit"]'::jsonb),
    lastmodifieddate = now()
WHERE LOWER(configname) = 'editpatientform'
  AND contents IS NOT NULL
  AND jsonb_typeof(contents) = 'object'
  AND contents != '{}'::jsonb
  AND LOWER(COALESCE(contents->>'configurable', contents->>'Configurable', '')) IS DISTINCT FROM 'yes';

-- ---------------------------------------------------------------------------
-- 5. Patient pages: allow fields to be added, and confirm the patient entity table.
--    The table name is what limits the existing field picker on these pages to patient fields.
--    patientaddresspage previously allowed edit and delete only, so add was never offered.
-- ---------------------------------------------------------------------------

UPDATE configs c
SET contents = (
        CASE
            WHEN c.contents ? 'ConfigureActions' THEN c.contents - 'ConfigureActions'
            ELSE c.contents
        END
    ) || jsonb_build_object('configureActions', 'add,edit,delete'),
    lastmodifieddate = now()
WHERE c.configtypeid = 9
  AND LOWER(c.configname) IN ('editpatientdetailspage', 'patientaddresspage')
  AND c.contents IS NOT NULL
  AND jsonb_typeof(c.contents) = 'object'
  AND c.contents != '{}'::jsonb
  AND POSITION('add' IN LOWER(COALESCE(c.contents->>'configureActions', c.contents->>'ConfigureActions', ''))) = 0;

UPDATE configs c
SET contents = (
        CASE
            WHEN c.contents ? 'TableName' THEN c.contents - 'TableName'
            ELSE c.contents
        END
    ) || jsonb_build_object('tablename', 'patient'),
    lastmodifieddate = now()
WHERE c.configtypeid = 9
  AND LOWER(c.configname) IN ('editpatientdetailspage', 'patientdetailspage', 'patientaddresspage')
  AND c.contents IS NOT NULL
  AND jsonb_typeof(c.contents) = 'object'
  AND c.contents != '{}'::jsonb
  AND LOWER(COALESCE(c.contents->>'tablename', c.contents->>'TableName', '')) IS DISTINCT FROM 'patient';

-- ---------------------------------------------------------------------------
-- 6. Search, selection and results pages hold query criteria rather than stored fields, so their
--    fields must never be offered for reuse. The None table sentinel is what marks that, and it is
--    the only thing FieldReuseCatalogue consults: ConfigureActions records whether a page's own
--    layout may be changed, which says nothing about whether its fields are stored data. Backfills
--    stored overrides to match the factory definitions.
-- ---------------------------------------------------------------------------

UPDATE configs c
SET contents = (
        CASE
            WHEN c.contents ? 'TableName' THEN c.contents - 'TableName'
            ELSE c.contents
        END
    ) || jsonb_build_object('tablename', 'None'),
    lastmodifieddate = now()
WHERE c.configtypeid = 9
  AND LOWER(c.configname) IN (
        'patientsearchpage',
        'patientsearchresultspage',
        'patientsearchresultswithnoaddpage',
        'admissionselectionpage',
        'requestselectionpage',
        'requestselectionforadmissionpage',
        'testselectionpage',
        'culturetypeselectionpage'
      )
  AND c.contents IS NOT NULL
  AND jsonb_typeof(c.contents) = 'object'
  AND c.contents != '{}'::jsonb
  AND COALESCE(NULLIF(c.contents->>'tablename', ''), NULLIF(c.contents->>'TableName', '')) IS NULL;

-- ---------------------------------------------------------------------------
-- 7. Language pack additions for the Add Existing Field form and its messages.
--    Added to the DB backed English pack (translationid 669) so installed systems pick them up
--    without relying on the factory pack.
-- ---------------------------------------------------------------------------

UPDATE language
SET pack = COALESCE(pack, '[]'::jsonb) || '[
    {"Key": "@ConAddEF@", "Value": "Add existing field"},
    {"Key": "@ConAddEFA@", "Value": "Select fields that are already used on other forms. The field keeps its identity, so both forms show the same stored information."},
    {"Key": "@ConExiF@", "Value": "Existing fields"},
    {"Key": "@ConExiFB@", "Value": "That field is already on this page."},
    {"Key": "@ConExiFC@", "Value": "That field cannot be added to this page."},
    {"Key": "@ConExiFD@", "Value": "The selected field no longer exists."},
    {"Key": "@ConExiFE@", "Value": "There are no other fields available to add to this page."},
    {"Key": "@ConExiFF@", "Value": "Select at least one field to add."},
    {"Key": "@ConOthP@", "Value": "Other pages"}
]'::jsonb,
    lastmodifieddate = now()
WHERE translationid = 669
  AND NOT EXISTS (
    SELECT 1
    FROM jsonb_array_elements(COALESCE(pack, '[]'::jsonb)) AS elem
    WHERE elem->>'Key' = '@ConAddEF@'
  );

UPDATE language
SET pack = COALESCE(pack, '[]'::jsonb) || '[
    {"Key": "@GenNoRes@", "Value": "No matching results"}
]'::jsonb,
    lastmodifieddate = now()
WHERE translationid = 669
  AND NOT EXISTS (
    SELECT 1
    FROM jsonb_array_elements(COALESCE(pack, '[]'::jsonb)) AS elem
    WHERE elem->>'Key' = '@GenNoRes@'
  );

-- ---------------------------------------------------------------------------
-- 8. Read only field label. A reused definition keeps the id of the original, which is the
--    storage key, so a reference copy on a second page is marked read only to leave a single
--    place the value can be entered.
-- ---------------------------------------------------------------------------

UPDATE language
SET pack = COALESCE(pack, '[]'::jsonb) || '[
    {"Key": "@ConRea@", "Value": "Read only?"}
]'::jsonb,
    lastmodifieddate = now()
WHERE translationid = 669
  AND NOT EXISTS (
    SELECT 1
    FROM jsonb_array_elements(COALESCE(pack, '[]'::jsonb)) AS elem
    WHERE elem->>'Key' = '@ConRea@'
  );
