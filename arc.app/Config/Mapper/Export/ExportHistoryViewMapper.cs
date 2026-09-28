using arc.app.Common;

namespace arc.app.Config.Mapper.Export;

/// <summary>
/// Maps export history record to the Sections format for GeneralViewer.
/// Displays each filter criterion as a separate field. Attachments appear in a separate region.
/// </summary>
internal class ExportHistoryViewMapper : IDefinition
{
    public string Get()
    {
        return @"{
            'Name': 'exporthistoryviewmapper',
            'Type': 'Standard',
            'Rules': [
                { Key: '<:1:>', Type: 'Mapping', Source: 'exportprofilename', Value: 'exportprofilename' },
                { Key: '<:1a:>', Type: 'Mapping', Source: 'schedulename', Value: 'schedulename' },
                { Key: '<:2:>', Type: 'Mapping', Source: 'runat', Value: 'runat' },
                { Key: '<:3:>', Type: 'Mapping', Source: 'startdate', Value: 'startdate' },
                { Key: '<:4:>', Type: 'Mapping', Source: 'enddate', Value: 'enddate' },
                { Key: '<:5:>', Type: 'Mapping', Source: 'specimentypeids', Value: 'specimentypeids' },
                { Key: '<:6:>', Type: 'Mapping', Source: 'specimenstateids', Value: 'specimenstateids' },
                { Key: '<:7:>', Type: 'Mapping', Source: 'tagids', Value: 'tagids' },
                { Key: '<:8:>', Type: 'Mapping', Source: 'organisationids', Value: 'organisationids' },
                { Key: '<:9:>', Type: 'Mapping', Source: 'locationids', Value: 'locationids' },
                { Key: '<:10:>', Type: 'Mapping', Source: 'testids', Value: 'testids' },
                { Key: '<:11:>', Type: 'Mapping', Source: 'organismids', Value: 'organismids' },
                { Key: '<:12:>', Type: 'Mapping', Source: 'astexclusive', Value: 'astexclusive' }
            ],
            'Target': {
                Sections: [
                    {
                        Id: 'exporthistorydetails',
                        Title: '@ExpExpHis@',
                        Fields: [
                            { Id: 'exportprofilename', Label: '@ExpProN@', Highlight: true, Value: '<:1:>' },
                            { Id: 'schedulename', Label: '@ExpSchNam@', Highlight: true, Value: '<:1a:>' },
                            { Id: 'runat', Label: '@GenDat@', Highlight: true, Value: '<:2:>' },
                            { Id: 'startdate', Label: '@GenStaB@', Highlight: true, Value: '<:3:>' },
                            { Id: 'enddate', Label: '@GenEnd@', Highlight: true, Value: '<:4:>' },
                            { Id: 'specimentypeids', Label: '@SpeTyp@', Highlight: true, Value: '<:5:>' },
                            { Id: 'specimenstateids', Label: '@GenSta@', Highlight: true, Value: '<:6:>' },
                            { Id: 'tagids', Label: '@GenTag@', Highlight: true, Value: '<:7:>' },
                            { Id: 'organisationids', Label: '@GenOrg@', Highlight: true, Value: '<:8:>' },
                            { Id: 'locationids', Label: '@GenLoc@', Highlight: true, Value: '<:9:>' },
                            { Id: 'testids', Label: '@GenTes@', Highlight: true, Value: '<:10:>' },
                            { Id: 'organismids', Label: '@GenOrgA@', Highlight: true, Value: '<:11:>' },
                            { Id: 'astexclusive', Label: '@ExpASTExc@', Highlight: true, Value: '<:12:>' }
                        ]
                    }
                ]
            }
        }";
    }
}
