using arc.app.Common;

namespace arc.app.Config.Events
{
    internal class AddSpecimenTypeDirectTestEventConfig : IDefinition
    {
        public string Get()
        {
            return @"{ 
                        EventName: 'addSpecimenTypeDirectTest',
                        Description: '@ConAddE@',
                        EventType : 'specialadddata',
                        Topic : 'Laboratory',
                        ValidationRules: [
                            { field: 'GroupId', rule: 'required', message: '@BreA@'},
                            { field: 'AssociatedListId', rule: 'required', message: '@ConAA@'}
                        ]
                    }";
        }
    }
}
