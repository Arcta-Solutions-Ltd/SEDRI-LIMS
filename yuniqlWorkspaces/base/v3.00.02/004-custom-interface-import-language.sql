-- Error-message language tags raised by the Custom interface inbound file load (CustomInstrumentProcessor).
-- These tags are stored in instrumenterrors.errortext, which the Interface Errors list translates
-- (InstrumentErrorListQuery has 'Translate': true), so the operator sees a meaningful description.
-- Matching everywhere is by id; only the display text is defined here. Run as a new migration; does not
-- amend existing scripts (mirrors 119-custom-interface-type-and-criteria-language.sql).
UPDATE language
SET pack = COALESCE(pack, '[]'::jsonb) || '[
    {"Key": "@InsCusPat@", "Value": "The patient does not exist and this profile does not create patients, so the specimen could not be loaded"},
    {"Key": "@InsCusParse@", "Value": "The inbound file could not be interpreted using the linked export profile structure"},
    {"Key": "@InsCusNoExp@", "Value": "The custom interface profile has no export profile mapping to load the file with"},
    {"Key": "@InsCusVal@", "Value": "An imported value could not be matched to a list item for this profile"},
    {"Key": "@InsCusSave@", "Value": "The custom interface record could not be saved"}
]'::jsonb,
    lastmodifieddate = now()
WHERE translationid = 669
  AND NOT EXISTS (
    SELECT 1
    FROM jsonb_array_elements(COALESCE(pack, '[]'::jsonb)) AS elem
    WHERE elem->>'Key' = '@InsCusPat@'
  );
