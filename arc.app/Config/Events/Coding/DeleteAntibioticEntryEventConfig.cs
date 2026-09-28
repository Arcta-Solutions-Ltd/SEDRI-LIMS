using arc.app.Common;

namespace arc.app.Config.Events
{
    internal class DeleteAntibioticEntryEventConfig : IDefinition
    {
        public string Get()
        {
            return @"{ 
                        EventName: 'deleteAntibioticEntry',
                        Description: '@AntDelB@',
                        EventType : 'deletedata',
                        Topic : 'Coding',
                        TableName: 'AntibioticCoding'
                    }";
        }
    }
}
