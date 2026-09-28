using arc.app.Common;

namespace arc.app.Config.Pages
{
    internal class EditReportSectionPageConfig : IDefinition
    {
        public string Get()
        {
            var page = @"{  
                            name: 'editreportsectionpage',
                            pageTitle: '@ConEdiN@',
                            text: '@ConEdiO@',
                            columns: [
                                { 
                                    key: 'col1',
                                    formGroups: [
                                        { 
                                            key: 'fg1',
                                            fields: [
                                                { id: 'SectionList', type: 'fieldselector', label: '@GenSec@', CanMoveEntries: true, IncludeOptions: false, CanEnable: false}
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
