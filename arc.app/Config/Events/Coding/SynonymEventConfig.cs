using arc.app.Common;

namespace arc.app.Config.Events
{
    internal class SynonymEventConfig : IDefinition
    {
        public string Get()
        {
            return @"{ 
                        EventName: 'synonym',
                        Description: '@OrgManD@',
                        EventType: 'special',
                        TableName: 'organism',
                        Topic: 'Coding'
                    }";
        }
    }
}
