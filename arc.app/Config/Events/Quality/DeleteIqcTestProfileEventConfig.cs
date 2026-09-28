using arc.app.Common;

namespace arc.app.Config.Events
{
    internal class DeleteIqcTestProfileEventConfig : IDefinition
    {
        public string Get()
        {
            return @"{ 
                EventName: 'deleteiqctestprofile', 
                Description: '@QuaDelProA@',
                EventType : 'special', 
                Topic : 'Quality',
                ValidationRules: [
                    { field: 'Id', rule: 'required', message: '@CodAB@'}
                ],
                DataRules: [
                    { type: 'NoRecord', query: 'checkwhetherlistitemisfixed', message: '@CodThiA@' },
                    { type: 'NoRecord', query: 'checkwhethercodinglistisempty', message: '@CodThiG@'}
                ]
            }";
        }
    }
}
