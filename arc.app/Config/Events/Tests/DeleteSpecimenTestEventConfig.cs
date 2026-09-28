using arc.app.Common;

namespace arc.app.Config.Events
{
    internal class DeleteSpecimenTestEventConfig : IDefinition
    {
        public string Get()
        {
            return @"{ 
                        EventName: 'deleteSpecimenTest', 
                        Description: '@TesDel@',
                        EventType : 'deletedata', 
                        Topic : 'Tests', 
                        TableName: 'Tests',
                        Display: [
                            { Label: 'key', Translation: '@GenKey@', List: 'No' },
                            { Label: 'name', Translation: '@GenNam@', List: 'No' }
                        ]
                    }";
        }

    }
}
