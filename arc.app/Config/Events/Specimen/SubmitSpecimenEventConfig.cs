using arc.app.Common;

namespace arc.app.Config.Events
{
    internal class SubmitSpecimenEventConfig : IDefinition
    {
        public string Get()
        {
            return @"{ 
                        EventName: 'submitspecimen', 
                        Description: '@SpeSub@',
                        EventType : 'editdata', 
                        Topic : 'Specimen', 
                        Mapping: 'submitspecimenmapper',
                        TableName: 'Specimen'
                    }";
        }

    }
}

                        //ValidationRules: [
                        //    { field: 'GrowthId', rule: 'required', message: '@SpeGro@'}
                        //],
                        //Display: [
                        //    { Label: 'growthid', Translation: '@SpeGroA@', List: 'No' }
                        //]
