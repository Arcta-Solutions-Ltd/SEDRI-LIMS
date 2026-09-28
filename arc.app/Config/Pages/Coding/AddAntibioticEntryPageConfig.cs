using arc.app.Common;

namespace arc.app.Config.Pages.Coding
{
    internal class AddAntibioticEntryPageConfig : IDefinition
    {
        public string Get()
        {
            var page = @"{ 
                            name: 'addantibioticentrypage',
                            pageTitle: '@AntAddD@',
                            text: '@AntAddE@.',
                            columns: [
                                { 
                                    key: 'col1',
                                    formGroups: [
                                        { 
                                            key: 'fg1',
                                            fields: [
                                                { id: 'AntibioticId', type: 'combobox', label: '@GenNam@', required: true, placeholder: '@GenAnt@', optionsName: 'antibiotic', dynamic: false, removeFixed: false },
                                                { id: 'Code', type: 'singleline', label: '@GenCodA@', required: true, Max: 10}
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
