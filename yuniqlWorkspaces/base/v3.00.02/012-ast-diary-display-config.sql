-- AST diary entry contents.
--
-- The updateast event now carries DisplayRoot, DisplaySections and a Display array so the AST diary entry
-- shows the AST lines, special considerations, susceptibility override audit and expert rule groups that were
-- saved with the event. The definition lives in arc.app/Config/Events/AST/ASTUpdateEventConfig.cs, and
-- EventAdapter.GetEventAsync falls back to it whenever the configs row is missing or holds '{}'.
--
-- Installed databases that carry a real override for updateast would keep shadowing the new definition with
-- the old one, which has no Display array at all and therefore renders an empty panel. This script rewrites
-- only those overrides. Rows that are absent or '{}' are deliberately left alone so they keep using the
-- built-in definition and pick up future changes automatically.
--
-- Run as a new migration; does not amend existing scripts.
UPDATE configs
SET contents = '{
    "EventName": "updateast",
    "Description": "@AstAdd@",
    "EventType": "specialadddata",
    "Topic": "Culture",
    "TableName": "AST",
    "ValidationRules": [],
    "DisplayRoot": "Crafted[0].Contents[0].value",
    "DisplaySections": [
        { "Path": "ASTResults", "Translation": "@AstDiaLin@" },
        { "Path": "ASTResults.SpecialRows", "Translation": "@AstDiaSpe@" },
        { "Path": "ASTResults.SusceptibilityOverride", "Translation": "@AstDiaOvr@" },
        { "Path": "ASTResults.SpecialRows.SusceptibilityOverride", "Translation": "@AstDiaOvr@" },
        { "Path": "ExpertRuleGroups", "Translation": "@AstExpRul@" },
        { "Path": "ExpertRuleGroups.Actions", "Translation": "@AstDiaAct@" }
    ],
    "Display": [
        { "Label": "CompletedDate", "Translation": "@AstDiaRecD@", "List": "No", "Date": true },
        { "Label": "CompletedTime", "Translation": "@AstDiaRecT@", "List": "No" },
        { "Label": "testPattern", "Translation": "@AstTesA@", "List": "No", "Resolver": "testpattern" },
        { "Label": "deleteBlankRows", "Translation": "@AstDiaDel@", "List": "No" },
        { "Label": "ASTCommentOne", "Translation": "@AstCom1@", "List": "Yes" },
        { "Label": "ASTCommentTwo", "Translation": "@AstCom2@", "List": "Yes" },
        { "Label": "ASTAdditionalNotes", "Translation": "@AstDiaNot@", "List": "No" },

        { "Label": "ASTResults", "Translation": "@AstDiaLin@", "List": "No", "Grid": true },
        { "Label": "TestType", "Translation": "@GenTesA@", "List": "No" },
        { "Label": "Antibiotic", "Translation": "@GenAnt@", "List": "No", "Resolver": "antibiotic" },
        { "Label": "Dosage", "Translation": "@GenDos@", "List": "No" },
        { "Label": "Guidelines", "Translation": "@GenGui@", "List": "Yes" },
        { "Label": "Measurement", "Translation": "@AstDiaMea@", "List": "No", "Resolver": "astmeasurement" },
        { "Label": "Susceptibility", "Translation": "@GenSus@", "List": "Yes" },
        { "Label": "TestResult", "Translation": "@GenSus@", "List": "Yes" },
        { "Label": "Category", "Translation": "@GenCat@", "List": "Yes" },
        { "Label": "DrugCategory", "Translation": "@GenCat@", "List": "Yes" },
        { "Label": "IncludeInReport", "Translation": "@GenInc@", "List": "No" },
        { "Label": "IncludeOnReport", "Translation": "@GenInc@", "List": "No" },
        { "Label": "AppliedBreakpointId", "Translation": "@AstDiaBrk@", "List": "No", "Resolver": "breakpointspecification" },

        { "Label": "SpecialRows", "Translation": "@AstDiaSpe@", "List": "No", "Grid": true },
        { "Label": "SpecialTypeId", "Translation": "@AstDiaSpeT@", "List": "Yes" },
        { "Label": "BreakpointId", "Translation": "@AstDiaBrk@", "List": "No", "Resolver": "breakpointspecification" },

        { "Label": "SusceptibilityOverride", "Translation": "@AstDiaOvr@", "List": "No" },
        { "Label": "OverriddenFromSusceptibilityId", "Translation": "@AstSusOverFrom@", "List": "Yes" },
        { "Label": "CannedCommentId", "Translation": "@AstSusCan@", "List": "Yes" },
        { "Label": "FreeTextComment", "Translation": "@AstSusFree@", "List": "No" },
        { "Label": "SetByUsername", "Translation": "@AstSusSetBy@", "List": "No" },
        { "Label": "SetAt", "Translation": "@AstSusSetAt@", "List": "No", "Date": true },

        { "Label": "ExpertRuleGroups", "Translation": "@AstExpRul@", "List": "No", "Grid": true },
        { "Label": "RuleId", "Translation": "@AstDiaRulN@", "List": "No", "Resolver": "expertrule" },
        { "Label": "RuleText", "Translation": "@AstDiaRulT@", "List": "No" },
        { "Label": "ApplyRule", "Translation": "@AstDiaApp@", "List": "No" },
        { "Label": "Actions", "Translation": "@AstDiaAct@", "List": "No", "Grid": true }
    ]
}'::jsonb,
    lastmodifieddate = now()
WHERE lower(configname) = 'updateast'
  AND configtypeid = 7
  AND contents IS NOT NULL
  AND contents <> '{}'::jsonb
  AND NOT (contents ? 'DisplayRoot');
