using arc.app.Common;

namespace arc.app.Config.Events.Export
{
    internal class EditExportProfileFieldsConfig:IDefinition
    {
        public string Get()
        {
            return @"{ 
                        EventName: 'editExportProfileFields', 
                        Description: '@ExpEdiA@',
                        EventType : 'special', 
                        Topic : 'Export'
                    }";
        }
    }
}
