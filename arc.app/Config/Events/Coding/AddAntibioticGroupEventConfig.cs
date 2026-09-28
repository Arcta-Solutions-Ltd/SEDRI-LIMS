using arc.app.Common;

namespace arc.app.Config.Events
{
    internal class AddAntibioticGroupEventConfig : IDefinition
    {
        public string Get()
        {
            return @"{ 
                        EventName: 'AddAntibioticGroup', 
                        Description: '@AntAddB@',
                        EventType : 'adddata', 
                        Mapping: 'addantibioticgroupmapper',
                        Topic: 'Coding',
                        TableName: 'ListItem',
                        ValidationRules: [
                            { field: 'Name', rule: 'required', message: '@AntAna@'}
                        ],
                        DataRules: [
                            { type: 'NoRecord', query: 'antibioticgroupexistsquery', message: '@AntThi@' }
                        ]
                    }";
        }
    }
}
