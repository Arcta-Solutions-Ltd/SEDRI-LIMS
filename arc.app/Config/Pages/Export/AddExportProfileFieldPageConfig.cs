using arc.app.Common;

namespace arc.app.Config.Pages.Export
{
    internal class AddExportProfileFieldPageConfig : IDefinition
    {
        public string Get()
        {
            var page = @"{ 
                            name: 'addexportprofilefieldpage',
                            pageTitle: '@ExpProG@',
                            text: '@ExpProH@.',
                            columns: [
                                { 
                                    key: 'col1',
                                    fieldWidth: 'wide',
                                    itemWidth: 'wide',
                                    formGroups: [
                                        {
                                            key: 'fg1',
                                            fields: [
                                                { id: 'Name', type: 'picker', label: '@GenFieB@', required: true, placeholder: '@ExpProFieldA@', optionsName:'ExportProfileFieldList' },
                                                { id: 'Heading', type: 'singleline', label: '@ExpProFielHead@', required: true, placeholder: '@ExpProFieldB@', Max: 100}
                                            ]
                                        },
                                        { key: 'fg4',
                                            fields: [                                        
                                                    {id: 'Mapping', type: 'combobox', label: '@MapTit@', placeholder: '@MapSel@', optionsName: 'MappingList'}
                                            ]
                                        },
                                        { key: 'fg2',
                                            rules:[{ effect: 'visible', field: 'Name', rule: '=', value: 'SpecimenComments|Custom|Specimen|Mulitcolumn, CultureComments|Custom|Culture|Mulitcolumn'}],
                                            fields: [                                        
                                                    { id: 'CommentFormat', type: 'combobox', label: '@GenComR@', multiselect: true, placeholder: '@GenComS@', optionsName: 'CommentFormat'}
                                            ]
                                        },
                                        { key: 'fg3',
                                            rules:[{ effect: 'visible', field: 'Name', rule: '=', value: 'SpecimenComments|Custom|Specimen|Mulitcolumn, CultureComments|Custom|Culture|Mulitcolumn'}],
                                            fields: [                                        
                                                    {id: 'CommentType', type: 'combobox', label: '@GenComF@', multiselect: true, placeholder: '@GenComL@', optionsName: 'CommentType'}
                                            ]
                                        },
                                        { key: 'fg5',
                                            rules:[{ effect: 'visible', field: 'Name', rule: '=', value: 'QualitativeAntibioticSusceptibility|Custom|Culture|Custom, AntibioticMeasurement|Custom|Culture|Custom'}],
                                            fields: [                                        
                                                    { id: 'Antibiotic', type: 'combobox', label: '@GenAnt@', placeholder: '@ExpFldAntiA@', optionsName: 'Antibiotic'}
                                            ]
                                        },
                                        { key: 'fg6',
                                            rules:[{ effect: 'visible', field: 'Name', rule: '=', value: 'QualitativeAntibioticSusceptibility|Custom|Culture|Custom, AntibioticMeasurement|Custom|Culture|Custom'}],
                                            fields: [                                        
                                                    { id: 'TestMethod', type: 'combobox', multiselect: true, label: '@BreTesA@', placeholder: '@BreTesA@', optionsName: 'TestMethod'}
                                            ]
                                        },
                                        { key: 'fg7',
                                            rules:[{ effect: 'visible', field: 'Name', rule: '=', value: 'QualitativeAntibioticSusceptibility|Custom|Culture|Custom, AntibioticMeasurement|Custom|Culture|Custom'}],
                                            fields: [                                        
                                                    { id: 'TestMethodPrecedence', type: 'combobox', label: '@BreTesB@', placeholder: '@BreTesA@', optionsName: 'TestMethod'}
                                            ]
                                        },
                                        { key: 'fg8',
                                            rules:[{ effect: 'visible', field: 'Name', rule: '=', value: 'WhonetAntibiotic|Custom|Culture|Multicolumn'}],
                                            fields: [                                        
                                                    { id: 'UseQualitativeValues', type: 'toggle', label: '@WhoQualVal@', defaultValue: 'No'}
                                            ]
                                        },
                                        { key: 'fg9',
                                            rules:[{ effect: 'visible', field: 'Name', rule: '=', value: 'BlankColumn|Custom|Custom|Custom'}],
                                            fields: [                                        
                                                    { id: 'BlankColumnDefault', type: 'singleline', label: '@ExpProFielDef@', defaultValue: 'No'}
                                            ]
                                        },
                                        { key: 'fg10',
                                            rules:[{ effect: 'visible', field: 'Name', rule: '=', value: 'Measurement|Custom|AST|Custom, AntibioticMeasurement|Custom|Culture|Custom'}],
                                            fields: [                                        
                                                    { id: 'IncludeMICExpressions', type: 'toggle', label: '@ExpASTMeasExp@', defaultValue: 'No'}
                                            ]
                                        },
                                        //{ key: 'fg11',
                                        //    rules:[{ effect: 'visible', field: 'Name', rule: '=', value: 'QualitativeAntibioticSusceptibility|Custom|Culture|Custom'}],
                                        //    fields: [                                        
                                        //            { id: 'SpecialConsiderationId', type: 'combobox', label: '@ExpASTSpeConA@', optionsName: 'SpecialConsiderations'}
                                        //    ]
                                        //},
                                    ]
                                }
                            ]
                        }";

            return page;
        }
    }
}
