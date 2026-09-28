using arc.app.Common;

namespace arc.app.Config.UIEvents
{
    internal class SpecimenCommentUIEventConfig : IDefinition
    {
        public string Get()
        {
            var newEvent = @"{
                        name: 'specimencommentuievent',
                        description: 'Specimen Comment',
                        type: 'form',
                        action: 'specimencommentform'
                    }";

            return newEvent;
        }
    }
}
