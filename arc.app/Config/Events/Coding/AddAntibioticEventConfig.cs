using arc.app.Common;

namespace arc.app.Config.Events
{
    internal class AddAntibioticEventConfig : IDefinition
    {
        public string Get()
        {
            return @"{ 
                        EventName: 'addantibiotic',
                        Description: '@RepAntB@',
                        EventType: 'specialadddata',
                        TableName: 'antibiotic',
                        Topic: 'Coding',
                        ValidationRules: [
                            { field: 'AntibioticName', rule: 'required', message: '@AntAddReqNam@' },
                            { field: 'Code', rule: 'required', message: '@AntAddReqCod@' }
                        ],
                        DataRules: [ 
                            { type: 'NoRecord', query: 'antibioticbyalreadyexistsforadd', message: '@AntAlrExi@' }
                        ]
                    }";
        }
    }
}
