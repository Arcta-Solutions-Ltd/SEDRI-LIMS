using arc.app.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace arc.app.Config.Events.Specimen
{
    internal class EditCommentForSelectorEventConfig : IDefinition
    {
        public string Get()
        {
            return @"{ 
                        EventName: 'editcommentforselector', 
                        Description: 'Edit Comment',
                        EventType : 'editdata', 
                        Topic : 'Specimen', 
                        TableName: 'SpecimenComment',
                        Mapping: 'editcommentforselectoreventmapper',
                        Display: [                           
                            { Label: 'comment', Translation: '@GenComT@', List: 'No' },
                            { Label: 'cannedcomment', Translation: '@GenComM@', List: 'Yes' },
                        ]
                    }";
        }
    }
}
