using arc.app.Common;

namespace arc.app.Config.Events
{
    internal class MovePatientEventConfig : IDefinition
    {
        public string Get()
        {
            return @"{ 
                        EventName: 'movepatient', 
                        Description: '@PatMov@',
                        EventType : 'special', 
                        Topic : 'Specimen', 
                        TableName: 'Specimen'
                    }";
        }
    }
}
