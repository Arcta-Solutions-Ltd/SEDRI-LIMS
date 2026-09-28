using arc.app.Common;

namespace arc.app.Config.Events
{
    internal class AddAntibioticEntryEventConfig : IDefinition
    {
        public string Get()
        {
            return @"{ 
                        EventName: 'addAntibioticEntry', 
                        Description: '@AntAddD@',
                        EventType : 'adddata', 
                        Topic : 'Coding', 
                        TableName: 'AntibioticCoding',
                        Mapping: 'antibioticcodingeventmapper',
                        ValidationRules: [
                            { field: 'AntibioticId', rule: 'required', message: '@AntAnaA@'},
                            { field: 'Code', rule: 'required', message: '@CodAC@'}
                        ],
                        DataRules: [
                            { type: 'FindRecord', query: 'antibioticexistsquery', message: '@AntThe@' },
                            { type: 'NoRecord', query: 'antibioticcodingexistsquery', message: '@AntTheA@' }
                        ]
                    }";
        }
    }
}
