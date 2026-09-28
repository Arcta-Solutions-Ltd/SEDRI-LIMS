using arc.app.Common;

namespace arc.app.Config.Events
{
    internal class RemoveCultureTestEventConfig : IDefinition
    {
        public string Get()
        {
            return @"{
                        EventName: 'removeculturetest',
                        Description: '@ConDelA@',
                        EventType : 'deletedata',
                        Topic : 'CultureTests',
                        TableName: 'CultureTests',
                        ValidationRules: [
                            { field: 'Id', rule: 'required', message: '@SpeAte@'}
                        ],
                        Display: [
                            { Label: 'Name', Translation: '@GenTesC@', List: 'No'}
                        ]
                    }";
        }
    }
}
