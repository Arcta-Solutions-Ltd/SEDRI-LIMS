using arc.app.Common;

namespace arc.app.Config.Events
{
    internal class CultureTestEntryEventConfig : IDefinition
    {
        public string Get()
        {
            return @"{ 
                        EventName: 'CultureTestEntry', 
                        Description: '@TesCarA@',
                        Topic : 'CultureTests', 
                        TableName: 'CultureTests'
                    }";
        }
    }
}
