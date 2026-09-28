using arc.app.Common;

namespace arc.app.Config.UIEvents
{
    internal class SpecimenDiaryUIEventConfig : IDefinition
    {
        public string Get()
        {
            var newEvent = @"{
                        name: 'specimendiaryuievent',
                        description: 'Specimen Diary',
                        type: 'diary',
                        action: 'specimendiary'
                    }";

            return newEvent;
        }
    }
}
