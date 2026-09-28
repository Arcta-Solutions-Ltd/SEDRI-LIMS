using arc.app.Common;

namespace arc.app.Config.Events
{
    internal class DeleteAntibioticGroupEventConfig : IDefinition
    {
        public string Get()
        {
            return @"{ 
                        EventName: 'deleteAntibioticGroup', 
                        Description: '@AntDel@',
                        EventType : 'special', 
                        Topic : 'Coding',
                        ValidationRules: [
                            { field: 'Id', rule: 'required', message: '@AntAnaA@'}
                        ],
                        DataRules: [
                            { type: 'NoRecord', query: 'checkwhetherlistitemisfixed', message: '@CodThiA@' },
                            { type: 'NoRecord', query: 'checkwhethercodinglistisempty', message: '@CodThiG@'}
                        ]
                    }";
        }
    }
}
