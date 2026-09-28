using arc.app.Common;

namespace arc.app.Config.Events
{
    internal class EditSpecimenTypeCultureTypeEventConfig : IDefinition
    {
        public string Get()
        {
            return @"{ 
                        EventName: 'editSpecimenTypeCultureType',
                        Description: '@ConEdiD@',
                        EventType : 'special',
                        Topic : 'Laboratory',
                        ValidationRules: [
                            { field: 'GroupId', rule: 'required', message: '@BreA@'},
                            { field: 'AssociatedListId', rule: 'required', message: '@ConA@'}
                        ]
                    }";
        }
    }
}
