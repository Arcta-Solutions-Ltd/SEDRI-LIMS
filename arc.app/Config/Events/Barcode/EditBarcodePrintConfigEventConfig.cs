using arc.app.Common;

namespace arc.app.Config.Events
{
    internal class EditBarcodePrintConfigEventConfig : IDefinition
    {
        public string Get()
        {
            return @"{ 
                        EventName: 'editbarcodeprintconfig',
                        Description: '@LabEdiB@',
                        EventType : 'special',
                        Topic : 'Configuration'
                    }";
        }
    }
}
