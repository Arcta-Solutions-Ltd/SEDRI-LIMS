using arc.app.Common;

namespace arc.app.Config.Pages
{
    internal class EditStatePageConfig : IDefinition
    {
        public string Get()
        {
            var page = @"{ 
                            name: 'editstatepage',
                            pageTitle: '@ConEdiP@',
                            text: '@ConEdiR@.',
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
                                                { id: 'stateid', type: 'dropdown', label: '@GenSta@', optionsName: 'specimenworkflowitems', dynamic: true},
                                                { id: 'Name', type: 'singleline', label: '@GenNam@', required: true, placeholder: '@GenLisA@' }
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
