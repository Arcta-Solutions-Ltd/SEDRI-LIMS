using arc.app.Common;

namespace arc.app.Config.Events
{
    public class DeleteCommentEventConfig : IDefinition
    {
        public string Get()
        {
            return @"{ 
                        EventName: 'deletecomment', 
                        Description: '@GenComN@',
                        EventType : 'special', 
                        Topic : 'Specimen', 
                        TableName: 'SpecimenComment'
                    }";
        }
    }
}
