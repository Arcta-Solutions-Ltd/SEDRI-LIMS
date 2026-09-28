using arc.app.Common;

namespace arc.app.Config.Events
{
    internal class DeleteCultureTestEventConfig : IDefinition
    {
        public string Get()
        {
            return @"{ 
                        EventName: 'deleteCultureTest', 
                        Description: '@TesDelC@',
                        EventType : 'deletedata', 
                        Topic : 'CultureTests', 
                        TableName: 'CultureTests'
                    }";
        }

    }
}
