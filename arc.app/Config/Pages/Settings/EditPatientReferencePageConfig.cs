using arc.app.Common;

namespace arc.app.Config.Pages
{
    internal class EditPatientReferencePageConfig : IDefinition
    {
        public string Get()
        {
            var page = @"{  
                            name: 'editpatientreferencepage',
                            pageTitle: '@SetOrdPat@',
                            text: '@SetSetPat@',
                            columns: [
                                { 
                                    key: 'col1',
                                    formGroups: [
                                        { 
                                            key: 'fg1',
                                            fields: [
                                                { id: 'FieldList', type: 'fieldselector', label: '@GenSec@', CanMoveEntries: true, IncludeOptions: false, CanEnable: false}
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
