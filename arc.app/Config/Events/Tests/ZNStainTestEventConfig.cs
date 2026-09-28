using arc.app.Common;

namespace arc.app.Config.Events
{
    internal class ZNStainTestEventConfig : IDefinition
    {
        public string Get()
        {
            return @"{ 
                        EventName: 'ZNStainTest', 
                        Description: '@TesZnsA@',
                        EventType : 'editdata', 
                        Mapping: 'znstaintestmapper',
                        StringFields: 'TestResults',
                        Topic : 'Tests', 
                        TableName: 'Tests',
                        Display: [
                            { Label: 'AFBQuantity', Translation: '@TesAfb@', List: 'Yes' },
                            { Label: 'printonreport', Translation: '@GenDis@', List: 'No' }
                        ],
                        ValidationRules: [
                            { field: 'AFBQuantity', rule: 'required', message: '@GenResA@'}
                        ]
                    }";
        }
    }
}
