using arc.app.Common;

namespace arc.app.Config.Workflows
{
    internal class SpecimenDefaultWorkflow : IDefinition
    {
        public string Get()
        {
            return @"{ 
                        Name: 'SpecimenDefault', 
                        Description: 'Default Specimen Workflow',
                        Table: 'specimen',
                        Field: 'stateid',
                        EntryConditions: [
                            {
                                Events: 'remotespecimen',
                                Default: '525'
                            },
                            {
                                Events: 'neoshieldspecimen',
                                Default: '525'
                            },
                            {
                                Events: 'newreceivedspecimen',
                                Default: '526',
                                options: [ 
                                    { newstate: '527', conditiontype: 'and', conditions: [ { field: 'action', value: '509' } ] },
                                    { newstate: '528', conditiontype: 'and', conditions: [ { field: 'action', value: '507' } ] },
                                    { newstate: '535', conditiontype: 'and', conditions: [ { field: 'action', value: '508' } ] }
                                ]
                            }
                        ],
                        StatesList : '524',
                        StartState: '525',
                        Steps: [
                            {
                                entrystate: '',
                                event: 'newreceivedspecimen',
                                actions: {
                                    options: [
                                        { action: 'PublishReport', newState: '528', conditions: [ { field: 'action', value: '507' } ],
                                          parameters: { reportconfig: 'DefaultSpecimenReport', reportname: '@RepFin@', ""doreportsneedapproval"": ""no"" },
                                          conditiontype: 'and'
                                        }
                                    ]
                                }
                            },
                            { 
                                entrystate: '525', 
                                event: 'ACKReceipt', 
                                exitstate: { 
                                    default: '526',
                                    options: [ 
                                        { newstate: '527', conditiontype: 'and', conditions: [ { field: 'action', value: '509' } ] },
                                        { newstate: '528', conditiontype: 'and', conditions: [ { field: 'action', value: '507' } ] },
                                        { newstate: '535', conditiontype: 'and', conditions: [ { field: 'action', value: '508' } ] }
                                    ]
                                },
                                actions: {
                                    options: [
                                        { action: 'PublishReport', newState: '528', parameters: { reportconfig: 'DefaultSpecimenReport', reportname: '@RepFin@', ""doreportsneedapproval"": ""no"" } }
                                    ]
                                }
                            },
                            { 
                                entrystate: '526',
                                event: 'RejectSpecimen',
                                exitstate: { default: '528' },
                                actions: {
                                    options: [
                                        { action: 'PublishReport', newState: '528', parameters: { reportconfig: 'DefaultSpecimenReport', reportname: '@RepFin@', ""doreportsneedapproval"": ""no"" } }
                                    ]
                                }
                            },
                            {
                                entryState: '526, 527, 529, 532, 533, 535',
                                event: 'TestSelection',
                                exitstate: {
                                    options: [ 
                                        { newstate: '527', conditiontype: 'and', conditions: [ { currentstate: '526' } ] }
                                    ]
                                }
                            },
                            {
                                entryState: '526, 527, 529, 532, 533, 535',
                                event: 'directtestentry',
                                exitstate: {
                                    options: [ 
                                        { newstate: '527', conditiontype: 'and', conditions: [ { currentstate: '526' } ] }
                                    ]
                                }
                            },
                            {
                                entryState: '525',
                                event: 'SpecimenCancelRequest',
                                exitstate: { default: '537' }
                            },
                            {
                                entryState: '525, 526, 527, 529, 532, 533, 535',
                                event: 'EditSpecimen'
                            },
                            {
                                entryState: '526, 527, 529, 532, 533, 535',
                                event: 'addCulture',
                                exitstate: {
                                    default: '535',
                                    options: [ 
                                        { newstate: '532', conditiontype: 'and', conditions: [ { currentstate: '532' } ] },
                                        { newstate: '533', conditiontype: 'and', conditions: [ { currentstate: '533' } ] },
                                        { newstate: '529', conditiontype: 'and', conditions: [ { currentstate: '529' } ] },
                                        { newstate: '529', conditiontype: 'and', conditions: [ { field: 'Quantity', value: '178' } ] },
                                        { newstate: '529', conditiontype: 'and', conditions: [ { field: 'Quantity', value: '179' } ] },
                                        { newstate: '529', conditiontype: 'and', conditions: [ { field: 'Quantity', value: '180' } ] },
                                        { newstate: '529', conditiontype: 'and', conditions: [ { field: 'Quantity', value: '181' } ] },
                                        { newstate: '529', conditiontype: 'and', conditions: [ { field: 'Quantity', value: '182' } ] },
                                        { newstate: '529', conditiontype: 'and', conditions: [ { field: 'Quantity', value: '183' } ] },
                                        { newstate: '529', conditiontype: 'and', conditions: [ { field: 'Quantity', value: '184' } ] },
                                        { newstate: '529', conditiontype: 'and', conditions: [ { field: 'Quantity', value: '185' } ] },
                                        { newstate: '529', conditiontype: 'and', conditions: [ { field: 'Quantity', value: '186' } ] },
                                        { newstate: '529', conditiontype: 'and', conditions: [ { field: 'Quantity', value: '187' } ] },
                                        { newstate: '529', conditiontype: 'and', conditions: [ { field: 'Quantity', value: '188' } ] },
                                        { newstate: '529', conditiontype: 'and', conditions: [ { field: 'Quantity', value: '1087' } ] },
                                        { newstate: '526', conditiontype: 'and', conditions: [ { field: 'Quantity', value: '177' }, { currentstate: '526' } ] },
                                        { newstate: '526', conditiontype: 'and', conditions: [ { field: 'Quantity', value: '1050' }, { currentstate: '526' } ] },
                                        { newstate: '526', conditiontype: 'and', conditions: [ { field: 'Quantity', value: '1086' }, { currentstate: '526' } ] },
                                        { newstate: '527', conditiontype: 'and', conditions: [ { field: 'Quantity', value: '177' }, { currentstate: '527' } ] },
                                        { newstate: '527', conditiontype: 'and', conditions: [ { field: 'Quantity', value: '1050' }, { currentstate: '527' } ] },
                                        { newstate: '527', conditiontype: 'and', conditions: [ { field: 'Quantity', value: '1086' }, { currentstate: '527' } ] }
                                    ]
                                }
                            },
                            {
                                entryState: '526, 527, 529, 532, 533, 535',
                                event: 'editCulture',
                                exitstate: {
                                    default: '535',
                                    options: [ 
                                        { newstate: '532', conditiontype: 'and', conditions: [ { currentstate: '532' } ] },
                                        { newstate: '533', conditiontype: 'and', conditions: [ { currentstate: '533' } ] },
                                        { newstate: '529', conditiontype: 'and', conditions: [ { currentstate: '529' } ] },
                                        { newstate: '529', conditiontype: 'and', conditions: [ { field: 'Quantity', value: '178' } ] },
                                        { newstate: '529', conditiontype: 'and', conditions: [ { field: 'Quantity', value: '179' } ] },
                                        { newstate: '529', conditiontype: 'and', conditions: [ { field: 'Quantity', value: '180' } ] },
                                        { newstate: '529', conditiontype: 'and', conditions: [ { field: 'Quantity', value: '181' } ] },
                                        { newstate: '529', conditiontype: 'and', conditions: [ { field: 'Quantity', value: '182' } ] },
                                        { newstate: '529', conditiontype: 'and', conditions: [ { field: 'Quantity', value: '183' } ] },
                                        { newstate: '529', conditiontype: 'and', conditions: [ { field: 'Quantity', value: '184' } ] },
                                        { newstate: '529', conditiontype: 'and', conditions: [ { field: 'Quantity', value: '185' } ] },
                                        { newstate: '529', conditiontype: 'and', conditions: [ { field: 'Quantity', value: '186' } ] },
                                        { newstate: '529', conditiontype: 'and', conditions: [ { field: 'Quantity', value: '187' } ] },
                                        { newstate: '529', conditiontype: 'and', conditions: [ { field: 'Quantity', value: '188' } ] },
                                        { newstate: '529', conditiontype: 'and', conditions: [ { field: 'Quantity', value: '1087' } ] },
                                        { newstate: '526', conditiontype: 'and', conditions: [ { field: 'Quantity', value: '177' }, { currentstate: '526' } ] },
                                        { newstate: '526', conditiontype: 'and', conditions: [ { field: 'Quantity', value: '1050' }, { currentstate: '526' } ] },
                                        { newstate: '526', conditiontype: 'and', conditions: [ { field: 'Quantity', value: '1086' }, { currentstate: '526' } ] },
                                        { newstate: '527', conditiontype: 'and', conditions: [ { field: 'Quantity', value: '177' }, { currentstate: '527' } ] },
                                        { newstate: '527', conditiontype: 'and', conditions: [ { field: 'Quantity', value: '1050' }, { currentstate: '527' } ] },
                                        { newstate: '527', conditiontype: 'and', conditions: [ { field: 'Quantity', value: '1086' }, { currentstate: '527' } ] }
                                    ]
                                }
                            },
                            {
                                entryState: '526, 527, 529, 532, 533, 535',
                                event: 'cultureTestEntry'
                            },
                            {
                                entryState: '526, 527, 529, 532, 533, 535',
                                event: 'deleteCulture',
                                exitstate: { default: '529' }
                            },
                            {
                                entryState: '526, 527, 529, 532, 533, 535',
                                event: 'cultureTestSelection'
                            },
                            {
                                entryState: '529, 532, 533, 535',
                                event: 'updateast'
                            },
                            {
                                entryState: '526, 527, 529, 532, 533, 535',
                                event: 'submitspecimen',
                                exitstate: { 
                                    default: '530',
                                    options: [ 
                                        { newstate: '534', conditiontype: 'and', conditions: [ { field: 'growthid', value: '521' } ] },
                                        { newstate: '535', conditiontype: 'and', conditions: [ { field: 'growthid', value: '536' } ] }
                                    ]
                                }
                            },
                            {
                                entryState: '530',
                                event: 'specimenapprovalone',
                                exitstate: { 
                                    default: '531', 
                                    options: [ 
                                        { newstate: '532', conditiontype: 'and', conditions: [ { field: 'decision', value: '523' } ] }
                                    ]
                                }
                            },
                            {
                                entryState: '531',
                                event: 'specimenapprovaltwo',
                                exitstate: { 
                                    default: '534', 
                                    options: [ 
                                        { newstate: '533', conditiontype: 'and', conditions: [ { field: 'decision', value: '523' } ] }
                                    ]
                                },
                                actions: {
                                    options: [ 
                                        { action: 'PublishReport', parameters: { reportconfig: 'DefaultSpecimenReport', reportname: '@RepFin@', ""doreportsneedapproval"": ""no"" }, newState: '534'}
                                    ]
                                }
                            },
                            {
                                entryState: '526, 527',
                                event: 'day0benchread',
                                exitstate: { 
                                    options: [
                                        { newstate: '528', conditiontype: 'and', conditions: [ { field: 'BenchReadDay0Action', value: '590' } ] },
                                        { newstate: '535', conditiontype: 'and', conditions: [ { field: 'BenchReadDay0Action', value: '591' } ] },
                                        { newstate: '527', conditiontype: 'and', conditions: [ { field: 'BenchReadDay0Action', value: '592' } ] }
                                    ]
                                }
                            },
                            {
                                entryState: '535',
                                event: 'day1benchread',
                                exitstate: { 
                                    options: [
                                        { newstate: '530', conditiontype: 'and', conditions: [ { field: 'BenchReadDay1Action', value: '593' } ] },
                                        { newstate: '529', conditiontype: 'and', conditions: [ { field: 'BenchReadDay1Action', value: '594' } ] },
                                        { newstate: '529', conditiontype: 'and', conditions: [ { field: 'BenchReadDay1Action', value: '595' } ] }
                                    ]
                                }
                            },
                            {
                                entryState: '526, 527, 529, 532, 533, 535',
                                event: 'CellCountTest',
                                exitstate: {
                                    options: [ 
                                        { newstate: '527', conditiontype: 'and', conditions: [ { currentstate: '526' } ] }
                                    ]
                                }
                            },
                            {
                                entryState: '526, 527, 529, 532, 533, 535',
                                event: 'GramStainTest',
                                exitstate: {
                                    options: [ 
                                        { newstate: '527', conditiontype: 'and', conditions: [ { currentstate: '526' } ] }
                                    ]
                                }
                            },
                            {
                                entryState: '526, 527, 529, 532, 533, 535',
                                event: 'IndiaInkTest',
                                exitstate: {
                                    options: [ 
                                        { newstate: '527', conditiontype: 'and', conditions: [ { currentstate: '526' } ] }
                                    ]
                                }
                            },
                            {
                                entryState: '526, 527, 529, 532, 533, 535',
                                event: 'WetPrepTest',
                                exitstate: {
                                    options: [ 
                                        { newstate: '527', conditiontype: 'and', conditions: [ { currentstate: '526' } ] }
                                    ]
                                }
                            },
                            {
                                entryState: '526, 527, 529, 532, 533, 535',
                                event: 'ZnStainTest',
                                exitstate: {
                                    options: [ 
                                        { newstate: '527', conditiontype: 'and', conditions: [ { currentstate: '526' } ] }
                                    ]
                                }
                            },
                            {
                                entryState: '526, 527, 529, 532, 533, 535',
                                event: 'PregnancyTest',
                                exitstate: {
                                    options: [ 
                                        { newstate: '527', conditiontype: 'and', conditions: [ { currentstate: '526' } ] }
                                    ]
                                }
                            },
                            {
                                entryState: '526, 527, 529, 532, 533, 535',
                                event: 'AuramineTest',
                                exitstate: {
                                    options: [ 
                                        { newstate: '527', conditiontype: 'and', conditions: [ { currentstate: '526' } ] }
                                    ]
                                }
                            },
                            {
                                entryState: '526, 527, 529, 532, 533, 535',
                                event: 'MicroscopyTest',
                                exitstate: {
                                    options: [ 
                                        { newstate: '527', conditiontype: 'and', conditions: [ { currentstate: '526' } ] }
                                    ]
                                }
                            },
                            {
                                entryState: '526, 527, 529, 532, 533, 535',
                                event: 'WrightsStainTest',
                                exitstate: {
                                    options: [ 
                                        { newstate: '527', conditiontype: 'and', conditions: [ { currentstate: '526' } ] }
                                    ]
                                }
                            },
                            {
                                entryState: '526, 527, 529, 532, 533, 535',
                                event: 'BiochemistryTest',
                                exitstate: {
                                    options: [ 
                                        { newstate: '527', conditiontype: 'and', conditions: [ { currentstate: '526' } ] }
                                    ]
                                }
                            },
                            {
                                entryState: '526, 527, 529, 532, 533, 535',
                                event: 'JevSerologyTest',
                                exitstate: {
                                    options: [ 
                                        { newstate: '527', conditiontype: 'and', conditions: [ { currentstate: '526' } ] }
                                    ]
                                }
                            },
                            {
                                entryState: '526, 527, 529, 532, 533, 535',
                                event: 'KOHPrepTest',
                                exitstate: {
                                    options: [ 
                                        { newstate: '527', conditiontype: 'and', conditions: [ { currentstate: '526' } ] }
                                    ]
                                }
                            },
                            {
                                entryState: '526, 527, 529, 532, 533, 535',
                                event: 'DipstickTest',
                                exitstate: {
                                    options: [ 
                                        { newstate: '527', conditiontype: 'and', conditions: [ { currentstate: '526' } ] }
                                    ]
                                }
                            },
                            {
                                entryState: '526, 527, 529, 532, 533, 535',
                                event: 'HPyloriAntigenTest',
                                exitstate: {
                                    options: [ 
                                        { newstate: '527', conditiontype: 'and', conditions: [ { currentstate: '526' } ] }
                                    ]
                                }
                            },
                            {
                                'entryState': '534',
                                'event': 'editaliquotevent'
                            }
                        ]
                    }";
        }
    }
}

        // Specimen States:
        //
        // 525 - "Pending Arrival"
        // 526 - "Specimen Received"
        // 527 - "Specimen Active"
        // 528 - "Specimen Rejected"
        // 529 - "Pending ID/AST"
        // 530 - "Pending Approval Level 1"
        // 531 - "Pending Approval Level 2"
        // 532 - "Analysis Rejected Level 1"
        // 533 - "Analysis Rejected Level 2"
        // 534 - "Specimen Finalised"
        // 535 - "Specimen Culturing"
        // 537 - "Request Cancelled"

        // Culture Types
        //
        // 978 - "Routine Culture"
        // 979 - "Fungal Culture"
        // 980 - "Anaerobic Culture"
        // 981 - "Enrichment Culture"
        // 982 - "Melioidosis Culture"


        //PossibleTests: 'Cell Count Test, Gram Stain Test',
        //DefaultTests:
        //[
        //    { specimentype: '810', tests:['gramstaintestform'] },
        //    { specimentype: '811', tests:['gramstaintestform'] },
        //    { specimentype: '812', tests:['microscopytestform'] },
        //    { specimentype: '813', tests:['gramstaintestform'] },
        //    { specimentype: '817', tests:['gramstaintestform', 'znstaintestform'] },
        //    { specimentype: '818', tests:['gramstaintestform'] },
        //    { specimentype: '820', tests:['microscopytestform', 'dipsticktestform'] }
        //]
