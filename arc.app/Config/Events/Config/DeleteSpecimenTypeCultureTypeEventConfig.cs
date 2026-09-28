using arc.app.Common;

namespace arc.app.Config.Events
{
    internal class DeleteSpecimenTypeCultureTypeEventConfig : IDefinition
    {
        public string Get()
        {
            return @"{ 
                        EventName: 'deleteSpecimenTypeCultureType',
                        Description: '@ConDelD@',
                        EventType : 'deletedata',
                        Topic : 'Laboratory',
                        TableName: 'LaboratoryConfigs'
                    }";
        }
    }
}
