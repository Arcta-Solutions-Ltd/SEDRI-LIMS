using arc.app.Common;

namespace arc.app.Config.Events.Export
{
    internal class EditExportProfileEventConfig : IDefinition
    {
        public string Get()
        {
            return @"{ 
                        EventName: 'editexportprofile',
                        Description: '@ExpEdi@',
                        EventType : 'special',
                        Topic : 'Export',
                        TableName: 'ExportProfile',
                        ExportQuery: 'astlistwide'
                    }";
        }
    }
}
