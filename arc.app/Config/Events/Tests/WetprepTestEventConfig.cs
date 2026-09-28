using arc.app.Common;

namespace arc.app.Config.Events
{
    internal class WetprepTestEventConfig : IDefinition
    {
        public string Get()
        {
            return @"{ 
                        EventName: 'WetprepTest', 
                        Description: '@TesWet@',
                        EventType : 'editdata', 
                        Mapping: 'wetpreptestmapper',
                        StringFields: 'TestResults',
                        Topic : 'Tests', 
                        TableName: 'Tests',
                        Display: [
                            { Label: 'rbcwetprep', Translation: '@TesRbcB@', List: 'Yes' },
                            { Label: 'wbcwetprep', Translation: '@TesWbcB@', List: 'Yes' },
                            { Label: 'parasitegrid', Translation: '@TesPar@', List: 'No' },
                            { Label: 'parasite', Translation: '@TesParA@', List: 'Yes' },
                            { Label: 'parasitetype', Translation: '@GenTyp@', List: 'Yes' },
                            { Label: 'foundparasite', Translation: '@GenFou@', List: 'Yes' },
                            { Label: 'printonreport', Translation: '@GenDis@', List: 'No' }
                        ]
                    }";
        }
    }
}
