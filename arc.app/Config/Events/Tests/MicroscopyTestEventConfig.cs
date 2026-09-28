using arc.app.Common;

namespace arc.app.Config.Events
{
    internal class MicroscopyTestEventConfig : IDefinition
    {
        public string Get()
        {
            return @"{ 
                        EventName: 'MicroscopyTest', 
                        Description: '@TesMic@',
                        EventType : 'editdata', 
                        Mapping: 'microscopytestmapper',
                        StringFields: 'TestResults',
                        Topic : 'Tests', 
                        TableName: 'Tests',
                        Display: [
                            { Label: 'epitheliumId', Translation: '@TesEpiA@', List: 'Yes' },
                            { Label: 'bacteriaId', Translation: '@GenBacA@', List: 'Yes' },
                            { Label: 'yeastId', Translation: '@GenYea@', List: 'Yes' },
                            { Label: 'crystalgrid', Translation: '@GenCry@', List: 'No' },
                            { Label: 'crystal', Translation: '@GenCry@', List: 'Yes' },
                            { Label: 'crystalseen', Translation: '@GenSee@', List: 'Yes' },
                            { Label: 'castgrid', Translation: '@GenCas@', List: 'No' },
                            { Label: 'cast', Translation: '@GenCas@', List: 'Yes' },
                            { Label: 'castseen', Translation: '@GenSee@', List: 'Yes' },
                            { Label: 'printonreport', Translation: '@GenDis@', List: 'No' }
                        ]
                    }";
        }
    }
}
