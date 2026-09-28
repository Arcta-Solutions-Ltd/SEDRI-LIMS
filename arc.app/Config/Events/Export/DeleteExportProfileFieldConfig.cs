using arc.app.Common;

namespace arc.app.Config.Events.Export
{
    internal class DeleteExportProfileFieldConfig:IDefinition
    {
        public string Get()
        {
            return @"{ 
                        EventName: 'deleteExportProfileField', 
                        Description: '@ExpDelA@',
                        EventType : 'special', 
                        Topic : 'Export'
                    }";
        }
    }
}
