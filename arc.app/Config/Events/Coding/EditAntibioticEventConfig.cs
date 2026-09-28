using arc.app.Common;

namespace arc.app.Config.Events
{
    internal class EditAntibioticEventConfig : IDefinition
    {
        public string Get()
        {
            return @"{ 
                        EventName: 'editAntibiotic', 
                        Description: '@AntManEdi@',
                        EventType: 'editdata', 
                        TableName: 'antibiotic',
                        Topic: 'Coding',
                        ValidationRules: [
                            { field: 'AntibioticName', rule: 'required', message: '@AntAddReqNam@' },
                            { field: 'Code', rule: 'required', message: '@AntAddReqCod@' }
                        ],
                        DataRules: [ 
                            { type: 'NoRecord', query: 'antibioticbyalreadyexistsforedit', message: '@AntAlrExi@' }
                        ]
                    }";
        }
    }
}
