
using arc.app.Common;

namespace arc.app.Config.Events
{
    internal class DeleteTestPatternEventConfig : IDefinition
    {
        public string Get()
        {
            return @"{ 
                        EventName: 'deleteTestPattern', 
                        Description: '@TesDelA@',
                        EventType : 'special', 
                        Topic : 'TestPattern'
                    }";
        }
    }
}
