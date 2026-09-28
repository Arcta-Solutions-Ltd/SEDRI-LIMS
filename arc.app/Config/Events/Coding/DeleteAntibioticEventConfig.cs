using arc.app.Common;

namespace arc.app.Config.Events
{
    internal class DeleteAntibioticEventConfig : IDefinition
    {
        public string Get()
        {
            return @"{ 
                        EventName: 'deleteAntibiotic', 
                        Description: '@AntManDel@',
                        EventType: 'deletedata',
                        TableName: 'antibiotic',
                        Topic: 'Coding',
                        ValidationRules: [
                            { field: 'Id', rule: 'required', message: '@AntDelIdReq@'}
                        ],
                        DataRules: [ 
                            { type: 'NoRecord', query: 'antibioticinastquery', message: '@AntInUseAst@' },
                            { type: 'NoRecord', query: 'antibioticintestpatternlinequery', message: '@AntInUseTes@' },
                            { type: 'NoRecord', query: 'antibioticinbreakpointquery', message: '@AntInUseBre@' }
                        ]
                    }";
        }
    }
}
