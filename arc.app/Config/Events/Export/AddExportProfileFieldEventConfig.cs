using arc.app.Common;

namespace arc.app.Config.Events.Export
{
    internal class AddExportProfileFieldEventConfig:IDefinition
    {
        public string Get()
        {
            return @"{ 
                        EventName: 'addexportprofilefield',
                        Description: '@ExpAddA@',
                        EventType : 'specialadddata',
                        Topic : 'Export',
                        TableName: 'ExportProfileRecord',
                        ExportQuery: 'astlistwide',
                    }";
        }
    }
}
