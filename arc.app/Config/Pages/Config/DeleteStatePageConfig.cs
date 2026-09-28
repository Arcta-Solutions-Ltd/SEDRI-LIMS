using arc.app.Common;

namespace arc.app.Config.Pages
{
    internal class DeleteStatePageConfig : IDefinition
    {
        public string Get()
        {
            var page = @"{ 
                            name: 'deletestatepage',
                            pageTitle: '@ConDelS@',
                            text: '@ConDelT@.',
                            required: 'Name',
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
                                                { id: 'stateid', type: 'dropdown', label: '@GenNam@', required: true, placeholder: '@GenLisA@', optionsName: 'specimenworkflowitems', dynamic: true, removeFixed: true }
                                            ]
                                        }
                                    ]
                                }
                            ],
                            nextButton: { show: true, buttonText: '@GenDelC@' }
                        }";

            return page;
        }
    }
}
