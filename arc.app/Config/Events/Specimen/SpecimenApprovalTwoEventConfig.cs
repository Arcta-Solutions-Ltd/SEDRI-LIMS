using arc.app.Common;

namespace arc.app.Config.Events
{
    public class SpecimenApprovalTwoEventConfig : IDefinition
    {
        public string Get()
        {
            return @"{ 
                        EventName: 'specimenapprovaltwo', 
                        Description: '@SpeSpeA@',
                        EventType : 'special', 
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
