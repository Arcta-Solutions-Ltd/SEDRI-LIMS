using arc.app.Common;

namespace arc.app.Config.Events
{
    internal class DipstickTestEventConfig : IDefinition
    {
        public string Get()
        {
            return @"{ 
                        EventName: 'DipstickTest', 
                        Description: '@TesDip@',
                        EventType : 'editdata', 
                        Mapping: 'dipsticktestmapper',
                        StringFields: 'TestResults',
                        Topic : 'Tests', 
                        TableName: 'Tests',
                        Display: [
                            { Label: 'phId', Translation: '@TesPh@', List: 'Yes' },
                            { Label: 'specificGravityId', Translation: '@TesSpe@', List: 'Yes' },
                            { Label: 'proteinId', Translation: '@TesProA@', List: 'Yes' },
                            { Label: 'ketonesId', Translation: '@TesKet@', List: 'Yes' },
                            { Label: 'glucoseId', Translation: '@TesGluA@', List: 'Yes' },
                            { Label: 'bloodId', Translation: '@GenBlo@', List: 'Yes' },
                            { Label: 'leucocytesId', Translation: '@TesLeu@', List: 'Yes' },
                            { Label: 'nitritesId', Translation: '@TesNit@', List: 'Yes' },
                            { Label: 'printonreport', Translation: '@GenDis@', List: 'No' }
                        ]
                    }";
        }
    }
}

