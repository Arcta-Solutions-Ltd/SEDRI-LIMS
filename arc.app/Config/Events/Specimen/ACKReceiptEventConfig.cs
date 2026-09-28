using arc.app.Common;

namespace arc.app.Config.Events
{
    /// <summary>
    /// Represents the configuration for the ACK Receipt event.
    /// </summary>
    internal class ACKReceiptEventConfig : IDefinition
    {
        /// <summary>
        /// Retrieves the configuration for the ACK Receipt event in JSON format.
        /// </summary>
        /// <returns>A JSON string containing the event configuration.</returns>
        public string Get()
        {
            return @"{
                    EventName: 'ACKReceipt',
                    Description: '@SpeAck@',
                    EventType : 'special',
                    Topic : 'Specimen',
                    TableName: 'Specimen',
                    ValidationRules: [
                        { field: 'ReceivedDate', rule: 'required', message: '@SpeRec@'},
                        { field: 'ReceivedTime', rule: 'required', message: '@SpeRecA@'},
                        { field: 'ReceivedConditionId', rule: 'required', message: '@SpeRecB@'}
                    ],
                    Display: [
                        { Label: 'receiveddate', Translation: '@SpeRecD@', List: 'No', Date: true },
                        { Label: 'receivedtime', Translation: '@SpeRecE@', List: 'No' },
                        { Label: 'receivedcondition', Translation: '@SpeSpeE@', List: 'Yes' },
                        { Label: 'specimenappearance', Translation: '@SpeSpeF@', List: 'No' },
                        { Label: 'action', Translation: '@SpeImm@', List: 'No' },
                        { Label: 'rejectionreason', Translation: '@SpeReaH@', List: 'No' },
                        { Label: 'selectreasonid', Translation: '@SpeReaG@', List: 'Yes' }
                    ]
                }";
        }
    }
}
