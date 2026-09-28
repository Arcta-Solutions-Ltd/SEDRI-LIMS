using arc.app.Common;

namespace arc.app.Config.Events
{
    internal class BatchSubmitConfirmationEventConfig : IDefinition
    {
        public string Get()
        {
            return @"{ 
                        EventName: 'batchsubmitconfirmation', 
                        Description: '@SpeBatD@',
                        EventType: 'batch', 
                        Topic : 'Specimen', 
                        TableName: 'Specimen',
                        BatchEvent: 'submitspecimen'
                    }";
        }
    }
}
