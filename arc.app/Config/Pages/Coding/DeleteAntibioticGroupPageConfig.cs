using arc.app.Common;

namespace arc.app.Config.Pages.Coding
{
    internal class DeleteAntibioticGroupPageConfig : IDefinition
    {
        public string Get()
        {
            var page = @"{ 
                            name: 'deleteantibioticgrouppage',
                            pageTitle: '@AntDel@',
                            text: '@AntDelA@.',
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
                                                { id: 'Id', type: 'dropdown', label: '@GenNam@', required: true, placeholder: '@GenGroA@', optionsName: 'antibioticgroup', dynamic: true, removeFixed: true }
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
