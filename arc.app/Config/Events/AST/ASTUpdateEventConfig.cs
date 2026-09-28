using arc.app.Common;

namespace arc.app.Config.Events
{
    /// <summary>
    /// Built-in configuration for the <c>updateast</c> event.
    /// </summary>
    /// <remarks>
    /// AST is a crafted event, so the whole payload the AST page submits is nested under
    /// <c>Crafted[0].Contents[0].value</c> rather than sitting at the payload root; <c>DisplayRoot</c> points the
    /// display pipeline at it. <c>DisplaySections</c> then declares which nested collections stay nested beneath
    /// their parent AST line, and <c>Display</c> names every field that is shown along with how to resolve it.
    /// Anything not named in <c>Display</c> is dropped, which is what keeps internal payload members such as
    /// <c>Event</c>, <c>View</c>, <c>EntryType</c>, <c>TestMethod</c>, <c>ExpertRuleLine</c>,
    /// <c>ExpertRuleEvalContext</c> and <c>testPatternFullList</c> out of the diary.
    /// Every lookup is keyed on the stored id so a diary entry still resolves after list items or tags have been
    /// translated into another language.
    /// </remarks>
    internal class ASTUpdateEventConfig : IDefinition
    {
        /// <summary>
        /// Returns the JSON event definition.
        /// </summary>
        /// <returns>JSON matching <see cref="arc.domain.Configuration.EventsConfig.EventConfig"/>.</returns>
        public string Get()
        {
            return @"{ 
                        EventName: 'updateast',
                        Description: '@AstAdd@',
                        EventType : 'specialadddata',
                        Topic : 'Culture',
                        TableName: 'AST',
                        ValidationRules: [],
                        DisplayRoot: 'Crafted[0].Contents[0].value',
                        DisplaySections: [
                            { Path: 'ASTResults', Translation: '@AstDiaLin@' },
                            { Path: 'ASTResults.SpecialRows', Translation: '@AstDiaSpe@' },
                            { Path: 'ASTResults.SusceptibilityOverride', Translation: '@AstDiaOvr@' },
                            { Path: 'ASTResults.SpecialRows.SusceptibilityOverride', Translation: '@AstDiaOvr@' },
                            { Path: 'ExpertRuleGroups', Translation: '@AstExpRul@' },
                            { Path: 'ExpertRuleGroups.Actions', Translation: '@AstDiaAct@' }
                        ],
                        Display: [
                            { Label: 'CompletedDate', Translation: '@AstDiaRecD@', List: 'No', Date: true },
                            { Label: 'CompletedTime', Translation: '@AstDiaRecT@', List: 'No' },
                            { Label: 'testPattern', Translation: '@AstTesA@', List: 'No', Resolver: 'testpattern' },
                            { Label: 'deleteBlankRows', Translation: '@AstDiaDel@', List: 'No' },
                            { Label: 'ASTCommentOne', Translation: '@AstCom1@', List: 'Yes' },
                            { Label: 'ASTCommentTwo', Translation: '@AstCom2@', List: 'Yes' },
                            { Label: 'ASTAdditionalNotes', Translation: '@AstDiaNot@', List: 'No' },

                            { Label: 'ASTResults', Translation: '@AstDiaLin@', List: 'No', Grid: true },
                            { Label: 'TestType', Translation: '@GenTesA@', List: 'No' },
                            { Label: 'Antibiotic', Translation: '@GenAnt@', List: 'No', Resolver: 'antibiotic' },
                            { Label: 'Dosage', Translation: '@GenDos@', List: 'No' },
                            { Label: 'Guidelines', Translation: '@GenGui@', List: 'Yes' },
                            { Label: 'Measurement', Translation: '@AstDiaMea@', List: 'No', Resolver: 'astmeasurement' },
                            { Label: 'Susceptibility', Translation: '@GenSus@', List: 'Yes' },
                            { Label: 'TestResult', Translation: '@GenSus@', List: 'Yes' },
                            { Label: 'Category', Translation: '@GenCat@', List: 'Yes' },
                            { Label: 'DrugCategory', Translation: '@GenCat@', List: 'Yes' },
                            { Label: 'IncludeInReport', Translation: '@GenInc@', List: 'No' },
                            { Label: 'IncludeOnReport', Translation: '@GenInc@', List: 'No' },
                            { Label: 'AppliedBreakpointId', Translation: '@AstDiaBrk@', List: 'No', Resolver: 'breakpointspecification' },

                            { Label: 'SpecialRows', Translation: '@AstDiaSpe@', List: 'No', Grid: true },
                            { Label: 'SpecialTypeId', Translation: '@AstDiaSpeT@', List: 'Yes' },
                            { Label: 'BreakpointId', Translation: '@AstDiaBrk@', List: 'No', Resolver: 'breakpointspecification' },

                            { Label: 'SusceptibilityOverride', Translation: '@AstDiaOvr@', List: 'No' },
                            { Label: 'OverriddenFromSusceptibilityId', Translation: '@AstSusOverFrom@', List: 'Yes' },
                            { Label: 'CannedCommentId', Translation: '@AstSusCan@', List: 'Yes' },
                            { Label: 'FreeTextComment', Translation: '@AstSusFree@', List: 'No' },
                            { Label: 'SetByUsername', Translation: '@AstSusSetBy@', List: 'No' },
                            { Label: 'SetAt', Translation: '@AstSusSetAt@', List: 'No', Date: true },

                            { Label: 'ExpertRuleGroups', Translation: '@AstExpRul@', List: 'No', Grid: true },
                            { Label: 'RuleId', Translation: '@AstDiaRulN@', List: 'No', Resolver: 'expertrule' },
                            { Label: 'RuleText', Translation: '@AstDiaRulT@', List: 'No' },
                            { Label: 'ApplyRule', Translation: '@AstDiaApp@', List: 'No' },
                            { Label: 'Actions', Translation: '@AstDiaAct@', List: 'No', Grid: true }
                        ]
                    }";
        }

    }
}
