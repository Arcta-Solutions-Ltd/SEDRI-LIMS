using arc.app.Common;

namespace arc.app.Config.Mapper;

/// <summary>
/// Provides a mapping definition for displaying isolate-related specimen data.
/// </summary>
/// <remarks>
/// This mapper transforms raw isolate data into a structured format suitable for UI rendering.
/// It includes mappings for organism details, quantity, comments, AST notes, and metadata such as barcode and report visibility.
/// The output is organized into sections and subsections, each with labeled fields and optional highlights.
/// </remarks>
internal class IsolateViewMapper : IDefinition
{
    /// <summary>
    /// Returns the isolate view mapping definition as a JSON-formatted string.
    /// </summary>
    /// <returns>
    /// A JSON string describing the mapping rules and target layout for isolate data presentation.
    /// </returns>
    public string Get()
    {
        return @"{  
            'Name': 'isolateviewmapper',  
            'Type': 'Standard',
            'Rules': [
                { Key: '<:1:>', Type: 'Mapping', Source: 'SpecimenOrganism', Value: 'SpecimenOrganism' },
                { Key: '<:2:>', Type: 'Mapping', Source: 'SpecimenQuantity', Value: 'SpecimenQuantity' },
                { Key: '<:6:>', Type: 'Mapping', Source: 'PositiveDate', Value: 'PositiveDate' },
                { Key: '<:7:>', Type: 'Mapping', Source: 'PositiveTime', Value: 'PositiveTime' },
                { Key: '<:10:>', Type: 'Mapping', Source: 'CommentOne', Value: 'CommentOne' },
                { Key: '<:11:>', Type: 'Mapping', Source: 'CommentTwo', Value: 'CommentTwo' },
                { Key: '<:12:>', Type: 'Mapping', Source: 'AdditionalNotes', Value: 'AdditionalNotes' },
                { Key: '<:13:>', Type: 'Mapping', Source: 'AloquatId', Value: 'AloquatId' },
                { Key: '<:14:>', Type: 'Mapping', Source: 'DisplayOnReport', Value: 'DisplayOnReport' },
                { Key: '<:15:>', Type: 'Mapping', Source: 'ESBL', Value: 'ESBL' },
                { Key: '<:16:>', Type: 'Mapping', Source: 'Type', Value: 'Type' },
                { Key: '<:17:>', Type: 'Mapping', Source: 'ManufacturersBarcode', Value: 'ManufacturersBarcode' },
                { Key: '<:18:>', Type: 'Mapping', Source: 'ASTAdditionalNotes', Value: 'ASTAdditionalNotes' },
                { Key: '<:19:>', Type: 'Mapping', Source: 'ASTCommentOne', Value: 'ASTCommentOne' },
                { Key: '<:20:>', Type: 'Mapping', Source: 'ASTCommentTwo', Value: 'ASTCommentTwo' },
                { Key: '<:21:>', Type: 'Mapping', Source: 'IdPercentage', Value: 'IdPercentage' },
                { Key: '<:22:>', Type: 'Mapping', Source: 'CultureBottleWeight', Value: 'CultureBottleWeight' },
                { Key: '<:23:>', Type: 'Mapping', Source: 'CultureBloodAndBottleWeight', Value: 'CultureBloodAndBottleWeight' },
            ],
            'Target': {
                Sections: [
                    {
                        Id: 'isolatedetails',
                        Title: '@CulIsoA@',
                        Fields: [
                            { Id: 'organism', Label: '@GenOrgA@', Highlight: true, Value: '<:1:>' },
                            { Id: 'quantity', Label: '@GenQua@', Highlight: true, Value: '<:2:>' },
                            { Id: 'ManufacturersBarcode', Label: '@InsManD@', Highlight: true, Value: '<:17:>' },
                            { Id: 'IdPercentage', Label: '@CulIdeCer@', Highlight: true, Value: '<:21:>' }
                        ],
                        SubSections: [
                            {
                                Id: 'additionalguidance', Title: '',
                                Fields: [
                                    { Id: 'comment1', Label: '@GenCom@', Value: '<:10:>' },
                                    { Id: 'comment2', Label: '@GenComA@', Value: '<:11:>' },
                                    { Id: 'ASTCommentOne', Label: '@AstCom1@', Value: '<:19:>' },
                                    { Id: 'ASTCommentTwo', Label: '@AstCom2@', Value: '<:20:>' },
                                    { Id: 'additionalnotes', Label: '@GenAdd@', Value: '<:12:>' },
                                    { Id: 'astadditionalnotes', Label: '@GenNot@', Value: '<:18:>' }
                                ]
                            },
                            {
                                Id: 'otherinfo', Title: '',
                                Fields: [
                                    { Id: 'aliquotid', Label: '@SpeAli@', Value: '<:13:>' },
                                    { Id: 'displayinreport', Label: '@GenDis@', Value: '<:14:>' }
                                ]
                            },
                            {
                                Id: 'otherinfo', Title: '',
                                Fields: [ ]
                            }
                        ]
                    }
                ]
            }
        }";
    }
}

