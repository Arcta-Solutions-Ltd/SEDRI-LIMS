-- Rename Instrument(s) user-facing language to Interface(s) for sidebar, views, forms, and events.
UPDATE language
SET pack = (
    SELECT COALESCE(jsonb_agg(
        CASE
            WHEN elem->>'Key' = '@GenIns@' THEN jsonb_set(elem, '{Value}', '"Interface"'::jsonb)
            WHEN elem->>'Key' = '@InsAccA@' THEN jsonb_set(elem, '{Value}', '"Accept and store results from external interface"'::jsonb)
            WHEN elem->>'Key' = '@InsAccB@' THEN jsonb_set(elem, '{Value}', '"Accept interface results"'::jsonb)
            WHEN elem->>'Key' = '@InsAdd@' THEN jsonb_set(elem, '{Value}', '"Add Interface Profile"'::jsonb)
            WHEN elem->>'Key' = '@InsAddA@' THEN jsonb_set(elem, '{Value}', '"Add interface profile"'::jsonb)
            WHEN elem->>'Key' = '@InsAddB@' THEN jsonb_set(elem, '{Value}', '"Add the profile for a new interface"'::jsonb)
            WHEN elem->>'Key' = '@InsReqT@' THEN jsonb_set(elem, '{Value}', '"Request Interface Test"'::jsonb)
            WHEN elem->>'Key' = '@InsReqTB@' THEN jsonb_set(elem, '{Value}', '"Request a test from the selected interface"'::jsonb)
            WHEN elem->>'Key' = '@InsReqEv@' THEN jsonb_set(elem, '{Value}', '"Request interface test"'::jsonb)
            WHEN elem->>'Key' = '@InsBatA@' THEN jsonb_set(elem, '{Value}', '"Batch replay interface errors"'::jsonb)
            WHEN elem->>'Key' = '@InsBatB@' THEN jsonb_set(elem, '{Value}', '"Batch Replay Interface Errors"'::jsonb)
            WHEN elem->>'Key' = '@InsBatC@' THEN jsonb_set(elem, '{Value}', '"Replay all of the selected interface errors"'::jsonb)
            WHEN elem->>'Key' = '@InsBatH@' THEN jsonb_set(elem, '{Value}', '"Batch accept and store results from external interface"'::jsonb)
            WHEN elem->>'Key' = '@InsBatI@' THEN jsonb_set(elem, '{Value}', '"Batch reject and remove results from external interface"'::jsonb)
            WHEN elem->>'Key' = '@InsDel@' THEN jsonb_set(elem, '{Value}', '"Delete Interface Profile"'::jsonb)
            WHEN elem->>'Key' = '@InsDelA@' THEN jsonb_set(elem, '{Value}', '"Delete interface profile"'::jsonb)
            WHEN elem->>'Key' = '@InsDelB@' THEN jsonb_set(elem, '{Value}', '"Delete an existing interface profile"'::jsonb)
            WHEN elem->>'Key' = '@InsEdi@' THEN jsonb_set(elem, '{Value}', '"Edit Interface Profile"'::jsonb)
            WHEN elem->>'Key' = '@InsEdiB@' THEN jsonb_set(elem, '{Value}', '"Edit key parameters that determine whether and how the system interacts with an external interface"'::jsonb)
            WHEN elem->>'Key' = '@InsEnt@' THEN jsonb_set(elem, '{Value}', '"Enter interface name"'::jsonb)
            WHEN elem->>'Key' = '@InsErr@' THEN jsonb_set(elem, '{Value}', '"Interface Errors"'::jsonb)
            WHEN elem->>'Key' = '@InsErrA@' THEN jsonb_set(elem, '{Value}', '"Interface Errors"'::jsonb)
            WHEN elem->>'Key' = '@InsFor@' THEN jsonb_set(elem, '{Value}', '"Formatted interface error details"'::jsonb)
            WHEN elem->>'Key' = '@InsForA@' THEN jsonb_set(elem, '{Value}', '"Formatted interface result details"'::jsonb)
            WHEN elem->>'Key' = '@InsIda@' THEN jsonb_set(elem, '{Value}', '"Id & AST Interface"'::jsonb)
            WHEN elem->>'Key' = '@InsIns@' THEN jsonb_set(elem, '{Value}', '"Interface Results"'::jsonb)
            WHEN elem->>'Key' = '@InsInsA@' THEN jsonb_set(elem, '{Value}', '"Interfaces"'::jsonb)
            WHEN elem->>'Key' = '@InsInsB@' THEN jsonb_set(elem, '{Value}', '"Interface Profile"'::jsonb)
            WHEN elem->>'Key' = '@InsInsC@' THEN jsonb_set(elem, '{Value}', '"Interface message details"'::jsonb)
            WHEN elem->>'Key' = '@InsInsD@' THEN jsonb_set(elem, '{Value}', '"Interface type"'::jsonb)
            WHEN elem->>'Key' = '@InsInsE@' THEN jsonb_set(elem, '{Value}', '"Interface name must be entered"'::jsonb)
            WHEN elem->>'Key' = '@InsInsF@' THEN jsonb_set(elem, '{Value}', '"Interface type must be selected"'::jsonb)
            WHEN elem->>'Key' = '@InsValGro@' THEN jsonb_set(elem, '{Value}', '"Default Growth must be selected for Id & AST interface type"'::jsonb)
            WHEN elem->>'Key' = '@InsValOrg@' THEN jsonb_set(elem, '{Value}', '"Organism Group must be selected for Id & AST interface type"'::jsonb)
            WHEN elem->>'Key' = '@InsValAnt@' THEN jsonb_set(elem, '{Value}', '"Antibiotic Group must be selected for Id & AST interface type"'::jsonb)
            WHEN elem->>'Key' = '@InsMan@' THEN jsonb_set(elem, '{Value}', '"Manage Interface Profiles"'::jsonb)
            WHEN elem->>'Key' = '@InsManA@' THEN jsonb_set(elem, '{Value}', '"Enable, disable and configure external interfaces"'::jsonb)
            WHEN elem->>'Key' = '@InsManB@' THEN jsonb_set(elem, '{Value}', '"Manage Interface Results"'::jsonb)
            WHEN elem->>'Key' = '@InsManC@' THEN jsonb_set(elem, '{Value}', '"Review and accept or reject test results from external interfaces"'::jsonb)
            WHEN elem->>'Key' = '@InsNam@' THEN jsonb_set(elem, '{Value}', '"Interface Name"'::jsonb)
            WHEN elem->>'Key' = '@InsNew@' THEN jsonb_set(elem, '{Value}', '"New interface results"'::jsonb)
            WHEN elem->>'Key' = '@InsNo@' THEN jsonb_set(elem, '{Value}', '"No specimen or culture found against which the interface result can be applied"'::jsonb)
            WHEN elem->>'Key' = '@InsRawA@' THEN jsonb_set(elem, '{Value}', '"Raw interface result details"'::jsonb)
            WHEN elem->>'Key' = '@InsRejA@' THEN jsonb_set(elem, '{Value}', '"Reject and remove results from external interface"'::jsonb)
            WHEN elem->>'Key' = '@InsRejB@' THEN jsonb_set(elem, '{Value}', '"Delete interface results"'::jsonb)
            WHEN elem->>'Key' = '@InsRep@' THEN jsonb_set(elem, '{Value}', '"Replay Interface Error"'::jsonb)
            WHEN elem->>'Key' = '@InsRepA@' THEN jsonb_set(elem, '{Value}', '"Replay interface error"'::jsonb)
            WHEN elem->>'Key' = '@InsRepB@' THEN jsonb_set(elem, '{Value}', '"Replay the selected interface error"'::jsonb)
            WHEN elem->>'Key' = '@InsUpd@' THEN jsonb_set(elem, '{Value}', '"Update interface profile"'::jsonb)
            ELSE elem
        END
    ), '[]'::jsonb)
    FROM jsonb_array_elements(COALESCE(pack, '[]'::jsonb)) AS elem
),
lastmodifieddate = now()
WHERE translationid = 669
  AND EXISTS (
    SELECT 1
    FROM jsonb_array_elements(COALESCE(pack, '[]'::jsonb)) AS elem
    WHERE elem->>'Key' IN (
        '@GenIns@', '@InsAccA@', '@InsAccB@', '@InsAdd@', '@InsAddA@', '@InsAddB@',
        '@InsReqT@', '@InsReqTB@', '@InsReqEv@', '@InsBatA@', '@InsBatB@', '@InsBatC@',
        '@InsBatH@', '@InsBatI@', '@InsDel@', '@InsDelA@', '@InsDelB@', '@InsEdi@', '@InsEdiB@',
        '@InsEnt@', '@InsErr@', '@InsErrA@', '@InsFor@', '@InsForA@', '@InsIda@', '@InsIns@',
        '@InsInsA@', '@InsInsB@', '@InsInsC@', '@InsInsD@', '@InsInsE@', '@InsInsF@',
        '@InsValGro@', '@InsValOrg@', '@InsValAnt@', '@InsMan@', '@InsManA@', '@InsManB@',
        '@InsManC@', '@InsNam@', '@InsNew@', '@InsNo@', '@InsRawA@', '@InsRejA@', '@InsRejB@',
        '@InsRep@', '@InsRepA@', '@InsRepB@', '@InsUpd@'
    )
  );

-- UI event tooltip tags (append if missing).
UPDATE language
SET pack = COALESCE(pack, '[]'::jsonb) || '[
    {"Key": "@InsAddC@", "Value": "Add profile for external interface"},
    {"Key": "@InsEdiC@", "Value": "Edit profile for external interface"},
    {"Key": "@InsDelC@", "Value": "Delete profile for external interface"},
    {"Key": "@InsAccD@", "Value": "Accept results from external interface"},
    {"Key": "@InsRejC@", "Value": "Reject results from external interface"},
    {"Key": "@InsBatJ@", "Value": "Batch accept results from external interface"},
    {"Key": "@InsBatK@", "Value": "Batch reject results from external interface"},
    {"Key": "@InsRepC@", "Value": "Replays an interface load which had an error"},
    {"Key": "@InsViewA@", "Value": "View interface result record"},
    {"Key": "@InsViewB@", "Value": "View the details of an interface error"}
]'::jsonb,
    lastmodifieddate = now()
WHERE translationid = 669
  AND NOT EXISTS (
    SELECT 1
    FROM jsonb_array_elements(COALESCE(pack, '[]'::jsonb)) AS elem
    WHERE elem->>'Key' = '@InsAddC@'
  );
