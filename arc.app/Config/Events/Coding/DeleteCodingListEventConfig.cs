using arc.app.Common;

namespace arc.app.Config.Events
{
    internal class DeleteCodingListEventConfig : IDefinition
    {
        public string Get()
        {
            return @"{ 
                        EventName: 'deleteCodingList', 
                        Description: '@CodDelE@',
                        EventType : 'special', 
                        Topic : 'Coding',
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
