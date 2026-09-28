using arc.app.Common;

namespace arc.app.Config.Events.Export
{
    internal class AddExportProfileEventConfig : IDefinition
    {
        public string Get()
        {
            return @"{ 
                        EventName: 'addexportprofile',
                        Description: '@ExpAdd@',
                        EventType : 'specialadddata',
                        Topic : 'Export',
                        TableName: 'ExportProfile',
                        ExportQuery: 'astlistwide',
                        ValidationRules: [
                            { field: 'Name', rule: 'required', message: '@QuaAddProC@'},
                            { field: 'Description', rule: 'required', message: '@ConAde@'}
                        ],
                        DataRules: [ 
                            { type: 'NoRecord', query: 'exportprofilealreadyexistsforadd', message: '@ExpProAlrExi@' }
                        ]
                    }";
        }
    }
}
