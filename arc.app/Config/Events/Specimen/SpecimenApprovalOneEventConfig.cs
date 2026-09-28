using arc.app.Common;

namespace arc.app.Config.Events
{
    public class SpecimenApprovalOneEventConfig : IDefinition
    {
        public string Get()
        {
            return @"{ 
                        EventName: 'specimenapprovalone', 
                        Description: '@SpeSpe@',
                        EventType: 'special', 
                        Topic : 'Specimen', 
                        TableName: 'Specimen',
                        ValidationRules: [
                            { field: 'Decision', rule: 'required',message: '@SpeDec@'}
                        ],
                        Display: [
                            { Label: 'decision', Translation: '@GenDec@', List: 'Yes' },
                            { Label: 'reason', Translation: '@GenRea@', List: 'No' }
                        ]
                    }";
        }
    }
}
