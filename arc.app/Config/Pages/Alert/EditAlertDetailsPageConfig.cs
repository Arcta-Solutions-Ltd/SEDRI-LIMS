using arc.app.Common;

namespace arc.app.Config.Pages
{
    internal class EditAlertDetailsPageConfig : IDefinition
    {
        public string Get()
        {
            var page = @"{ 
                            name: 'editalertdetailspage',
                            pageTitle: '@AleAleC@',
                            text: '@AleManC@.',
                            required: 'AlertName,AlertMessage,AlertTypeId',
                            requiredRule: 'and',
                            nextButton: { onclickstate: { state: 'noorganism',
                                                          rules:[{ effect: 'noorganism', field: 'ContainsOrganism', rule: '=', value: '0' }]
                                                        },
                                          buttontext: '@GenNex@',
                                          show: true
                                        },
                            columns: [
                                { 
                                    key: 'col1',
                                    fieldWidth: 'wide',
                                    itemWidth: 'wide',
                                    formGroups: [
                                        { 
                                            key: 'fg1',
                                            fields: [
                                                { id: 'AlertName', type: 'singleline', label: '@AleAle@', required: true, placeholder: '@AleEnt@', Max: 100 },
                                                { id: 'AlertMessage', type: 'multiline', label: '@GenMes@', required: true, placeholder: '@AleEntA@', Max: 500},
                                                { id: 'AlertTypeId', type: 'combobox', optionsName: 'AlertCategory', label: '@AleTyp@', required: true, dynamic: true  },
                                                { id: 'SpecificationId', type: 'combobox', label: '@BreSpf@', optionsName: 'specification', required: false, dynamic: true },
                                                { id: 'TagId', type: 'combobox', label: '@GenTag@', optionsName: 'tag', required: false, multiselect: true, dynamic: true },
                                                { id: 'Enabled', type: 'toggle', label: '@GenEna@', required: true }
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
