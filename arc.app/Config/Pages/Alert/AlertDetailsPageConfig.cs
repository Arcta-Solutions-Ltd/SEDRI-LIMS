using arc.app.Common;

namespace arc.app.Config.Pages
{
    internal class AlertDetailsPageConfig : IDefinition
    {
        public string Get()
        {
            var page = @"{ 
                            name: 'alertdetailspage',
                            pageTitle: '@AleAleC@',
                            text: '@AleManA@.',
                            required: 'AlertName,AlertMessage,AlertTypeId',
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
                                                { id: 'AlertName', type: 'singleline', label: '@AleAle@', required: true, placeholder: '@AleEnt@', Max: 100 },
                                                { id: 'AlertMessage', type: 'multiline', label: '@GenMes@', required: true, placeholder: '@AleEntA@', Max: 500},
                                                { id: 'AlertTypeId', type: 'combobox', optionsName: 'AlertCategory', label: '@AleTyp@', required: true, dynamic: true },
                                                { id: 'SpecificationId', type: 'combobox', label: '@BreSpf@', optionsName: 'specification', required: false, dynamic: true },
                                                { id: 'TagId', type: 'combobox', label: '@GenTag@', optionsName: 'tag', required: false, multiselect: true, dynamic: true }
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
