using arc.app.Common;

namespace arc.app.Config.Pages
{
    internal class AddBreakpointCriteriaPageConfig : IDefinition
    {
        public string Get()
        {
            var page = @"{ 
                            name: 'addbreakpointcriteriapage',
                            pageTitle: '@BreAddH@',
                            text: '@BreAddI@.',
                            required: 'antibioticid,testmethodid,specificationid,hostid,specialconsiderid',
                            requiredRule: 'and',
                            columns: [
                                { 
                                    key: 'col1',
                                    fieldWidth: 'wide',
                                    itemWidth: 'wide',
                                    formGroups: [
                                        { 
                                            key: 'fg1',
                                            fields: [
                                                { id: 'antibioticid', type: 'combobox', label: '@GenAnt@', optionsName: 'antibiotic', required: true},
                                                { id: 'testmethodid', type: 'dropdown', label: '@BreTes@', optionsName: 'testmethod', dynamic: true, required: true},
                                                { id: 'Dosage', type: 'number', label: '@GenDos@', Min: '0', Max: '999', MaxDPs: '0', required: false},
                                                { id: 'specificationid', type: 'dropdown', label: '@BreSpf@', optionsName: 'specification', dynamic: true, required: true},
                                                { id: 'hostid', type: 'dropdown', label: '@GenHos@', optionsName: 'host', dynamic: true, required: true},
                                                { id: 'specimentypeid', type: 'dropdown', multiselect: true, label: '@SpeSpeB@', optionsName: 'specimentype'},
                                                { id: 'specialconsiderid', type: 'combobox', label: '@BreSpe@', optionsName: 'specialconsiderations', dynamic: false, defaultValue: '973' }
                                            ]
                                        }
                                    ]
                                }
                            ]
                        }";

            return page;
        }
    }
}


