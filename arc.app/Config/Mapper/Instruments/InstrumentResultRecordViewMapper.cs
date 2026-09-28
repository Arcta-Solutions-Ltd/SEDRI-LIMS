using arc.app.Common;

namespace arc.app.Config.Mapper.Instruments;

/// <summary>
/// Maps a single instrument result row to GeneralViewer sections (config-driven labels).
/// </summary>
internal class InstrumentResultRecordViewMapper : IDefinition
{
    public string Get()
    {
        return @"{
                        'Name': 'instrumentresultrecordviewmapper',
                        'Type': 'standard',
                        'Rules': [
                            { Key: '<:1:>', Type: 'Mapping', Source: 'InstrumentProfile', Value: 'InstrumentProfile' },
                            { Key: '<:2:>', Type: 'Mapping', Source: 'AccessionNumber', Value: 'AccessionNumber' },
                            { Key: '<:3:>', Type: 'Mapping', Source: 'PatientName', Value: 'PatientName' },
                            { Key: '<:4:>', Type: 'Mapping', Source: 'SpecimenType', Value: 'SpecimenType' },
                            { Key: '<:5:>', Type: 'Mapping', Source: 'CultureType', Value: 'CultureType' },
                            { Key: '<:6:>', Type: 'Mapping', Source: 'Barcode', Value: 'Barcode' },
                            { Key: '<:7:>', Type: 'Mapping', Source: 'RequestMade', Value: 'RequestMade' },
                            { Key: '<:8:>', Type: 'Mapping', Source: 'ResultReceived', Value: 'ResultReceived' },
                            { Key: '<:9:>', Type: 'Mapping', Source: 'Status', Value: 'Status' },
                            { Key: '<:10:>', Type: 'Mapping', Source: 'LastModifiedDate', Value: 'LastModifiedDate' }
                        ],
                        'Target':
                            {
                                Sections: [
                                    {
                                        Id: 'instrumentresultdetails',
                                        Title: '@InsIns@',
                                        Fields: [
                                            { Id: 'instrumentprofile', Label: '@InsProNam@', Value: '<:1:>' },
                                            { Id: 'accessionnumber', Label: '@SpeAcc@', Value: '<:2:>' },
                                            { Id: 'patientname', Label: '@PatPatA@', Value: '<:3:>' },
                                            { Id: 'specimentype', Label: '@SpeSpeB@', Value: '<:4:>' },
                                            { Id: 'culturetype', Label: '@CulTyp@', Value: '<:5:>' },
                                            { Id: 'barcode', Label: '@InsManD@', Value: '<:6:>' },
                                            { Id: 'requestmade', Label: '@GenReq@', Value: '<:7:>' },
                                            { Id: 'resultreceived', Label: '@InsRes@', Value: '<:8:>' },
                                            { Id: 'status', Label: '@GenStaA@', Value: '<:9:>' },
                                            { Id: 'lastmodifieddate', Label: '@ExpProMd@', Value: '<:10:>' }
                                        ]
                                    }
                                ]
                            }
                     }";
    }
}
