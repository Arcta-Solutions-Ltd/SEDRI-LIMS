using arc.app.Common;

namespace arc.app.Config.Mapper.Admission;

/// <summary>
/// Maps request query results to the request record view header sections.
/// </summary>
internal class RequestViewMapper : IDefinition
{
    /// <summary>
    /// Returns the JSON mapping definition for the request record view.
    /// </summary>
    /// <returns>Mapper definition with field mappings and section layout.</returns>
    public string Get()
    {
        return @"{
            Name: 'requestviewmapper',
            Type: 'Standard',
            Rules: [
                { Key: '<:1:>', Type: 'Mapping', Source: 'Id', Value: 'Id' },
                { Key: '<:2:>', Type: 'Mapping', Source: 'RequestId', Value: 'RequestId' },
                { Key: '<:3:>', Type: 'Mapping', Source: 'RequestDate', Value: 'RequestDate' },
                { Key: '<:4:>', Type: 'Mapping', Source: 'RequestTime', Value: 'RequestTime' },
                { Key: '<:5:>', Type: 'Mapping', Source: 'RequestingClinician', Value: 'RequestingClinician' },
                { Key: '<:6:>', Type: 'Mapping', Source: 'DateOfAdmission', Value: 'DateOfAdmission' },
                { Key: '<:7:>', Type: 'Mapping', Source: 'TimeOfAdmission', Value: 'TimeOfAdmission' },
                { Key: '<:8:>', Type: 'Mapping', Source: 'Ward', Value: 'Ward' },
                { Key: '<:9:>', Type: 'Mapping', Source: 'CotAvailable', Value: 'CotAvailable' },
                { Key: '<:10:>', Type: 'Mapping', Source: 'Cot', Value: 'Cot' },
                { Key: '<:11:>', Type: 'Mapping', Source: 'Urgency', Value: 'Urgency' },
                { Key: '<:12:>', Type: 'Mapping', Source: 'Indication', Value: 'Indication' },
                { Key: '<:13:>', Type: 'Mapping', Source: 'WeightKnown', Value: 'WeightKnown' },
                { Key: '<:14:>', Type: 'Mapping', Source: 'CurrentWeight', Value: 'CurrentWeight' },
                { Key: '<:15:>', Type: 'Mapping', Source: 'CurrentWeightDate', Value: 'CurrentWeightDate' },
                { Key: '<:16:>', Type: 'Mapping', Source: 'AntibioticReceived', Value: 'AntibioticReceived' },
                { Key: '<:17:>', Type: 'Mapping', Source: 'AntibioticTiming', Value: 'AntibioticTiming' },
                { Key: '<:18:>', Type: 'Mapping', Source: 'AntibioticAgents', Value: 'AntibioticAgents' },
                { Key: '<:19:>', Type: 'Mapping', Source: 'AntibioticStartKnown', Value: 'AntibioticStartKnown' },
                { Key: '<:20:>', Type: 'Mapping', Source: 'AntibioticStartDate', Value: 'AntibioticStartDate' },
                { Key: '<:21:>', Type: 'Mapping', Source: 'AntibioticStartTime', Value: 'AntibioticStartTime' },
                { Key: '<:22:>', Type: 'Mapping', Source: 'RecentSurgery', Value: 'RecentSurgery' },
                { Key: '<:23:>', Type: 'Mapping', Source: 'CentralLineInPlace', Value: 'CentralLineInPlace' },
                { Key: '<:24:>', Type: 'Mapping', Source: 'CentralLineType', Value: 'CentralLineType' },
                { Key: '<:25:>', Type: 'Mapping', Source: 'RespiratorySupport', Value: 'RespiratorySupport' }
            ],
            Target: {
                Sections: [
                    {
                        Id: 'requestdetails',
                        Title: '@NeoReq@',
                        Fields: [
                            { Id: 'RequestId', Label: '@NeoReqRef@', Value: '<:2:>', Highlight: true },
                            { Id: 'RequestDate', Label: '@NeoReqDat@', Value: '<:3:>' },
                            { Id: 'RequestTime', Label: '@NeoReqTim@', Value: '<:4:>' },
                            { Id: 'RequestingClinician', Label: '@NeoCliNam@', Value: '<:5:>' },
                            { Id: 'DateOfAdmission', Label: '@NeoAdmDat@', Value: '<:6:>' },
                            { Id: 'TimeOfAdmission', Label: '@NeoAdmTim@', Value: '<:7:>' },
                            { Id: 'WardId', Label: '@NeoWar@', Value: '<:8:>' },
                            { Id: 'CotAvailable', Label: '@NeoCotAva@', Value: '<:9:>' },
                            { Id: 'CotId', Label: '@NeoCot@', Value: '<:10:>' },
                            { Id: 'UrgencyId', Label: '@NeoUrg@', Value: '<:11:>' },
                            { Id: 'IndicationId', Label: '@NeoInd@', Value: '<:12:>' },
                            { Id: 'WeightKnown', Label: '@NeoWeiKno@', Value: '<:13:>' },
                            { Id: 'CurrentWeight', Label: '@NeoCurWei@', Value: '<:14:>' },
                            { Id: 'CurrentWeightDate', Label: '@NeoCurWeiDat@', Value: '<:15:>' },
                            { Id: 'AntibioticReceived', Label: '@NeoAntRec@', Value: '<:16:>' },
                            { Id: 'AntibioticTimingId', Label: '@NeoAntTim@', Value: '<:17:>' },
                            { Id: 'AntibioticAgentsId', Label: '@NeoAntAge@', Value: '<:18:>' },
                            { Id: 'AntibioticStartKnown', Label: '@NeoAntStaKno@', Value: '<:19:>' },
                            { Id: 'AntibioticStartDate', Label: '@NeoAntStaDat@', Value: '<:20:>' },
                            { Id: 'AntibioticStartTime', Label: '@NeoAntStaTim@', Value: '<:21:>' },
                            { Id: 'RecentSurgeryId', Label: '@NeoRecSur@', Value: '<:22:>' },
                            { Id: 'CentralLineInPlace', Label: '@NeoCenLin@', Value: '<:23:>' },
                            { Id: 'CentralLineTypeId', Label: '@NeoCenLinTyp@', Value: '<:24:>' },
                            { Id: 'RespiratorySupportId', Label: '@NeoResSup@', Value: '<:25:>' }
                        ]
                    }
                ]
            }
        }";
    }
}
