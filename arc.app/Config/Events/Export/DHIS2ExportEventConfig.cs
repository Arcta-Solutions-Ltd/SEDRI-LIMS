using arc.app.Common;

namespace arc.app.Config.Events
{
    internal class DHIS2ExportEventConfig : IDefinition
    {
        public string Get()
        {
            return @"{ 
                        EventName: 'dhis2export',
                        Description: '@ExpGen@',
                        EventType : 'export',
                        Topic : 'Export',
                        TableName: 'Export',
                        ExportQuery: 'astlistwide',
                        ValidationRules: [
                            { field: 'StartDate', rule: 'required', message: '@ExpYou@'}
                        ]
                    }";
        }
    }
}
