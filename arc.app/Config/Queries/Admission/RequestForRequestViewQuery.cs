using arc.app.Common;

namespace arc.app.Config.Queries.Admission;

/// <summary>
/// Query definition returning a single request for the request record view header.
/// </summary>
internal class RequestForRequestViewQuery : IDefinition
{
    /// <summary>
    /// Retrieves the query definition.
    /// </summary>
    /// <returns>A JSON string containing the query configuration.</returns>
    public string Get()
    {
        return @"{
                        Query: 'RequestForRequestView',
                        TableName: 'Request',
                        Type: 'Single',
                        Translate: true,
                        ResultMapping: 'requestviewmapper',
                        Fields: [
                            { Name: 'Id', Type: 'int' },
                            { Name: 'PatientId', Type: 'int' },
                            { Name: 'AdmissionId', Type: 'int' },
                            { Name: 'RequestId', Type: 'string' },
                            { Name: 'RequestDate', Type: 'date' },
                            { Name: 'RequestTime', Type: 'string' },
                            { Name: 'RequestingClinician', Type: 'string' },
                            { Name: 'CurrentWeight', Type: 'numeric' },
                            { Name: 'CurrentWeightDate', Type: 'date' },
                            { Name: 'AntibioticStartDate', Type: 'date' },
                            { Name: 'AntibioticStartTime', Type: 'string' }
                        ],
                        Joins: [
                            {
                                Table: 'Admission',
                                Type: 'Left',
                                Fields: [
                                    { Name: 'DateOfAdmission', FromMoreData: true },
                                    { Name: 'TimeOfAdmission', FromMoreData: true }
                                ]
                            }
                        ],
                        ListItems: 'Ward, Cot, Urgency, Indication, RecentSurgery, CentralLineType, RespiratorySupport, AntibioticTiming, AntibioticAgents, CotAvailable, AntibioticReceived, CentralLineInPlace',
                        MultiSelectItems: 'AntibioticAgents, WeightKnown, AntibioticStartKnown',
                        Where: [
                            { Field: 'Id', Comparison: '=' }
                        ]
                    }";
    }
}
