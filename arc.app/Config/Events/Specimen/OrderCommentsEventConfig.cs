using arc.app.Common;

namespace arc.app.Config.Events.Specimen
{
    internal class OrderCommentsEventConfig : IDefinition
    {
        public string Get()
        {
            return @"{ 
                       EventName: 'ordercomments', 
                        Description: 'Order Comments',
                        EventType : 'editdata', 
                        Topic : 'Specimen', 
                        TableName: 'SpecimenComment',        
                        ValidationRules: [
                        ],
                        Display: [
                        ]
                    }";
        }
    }
}
