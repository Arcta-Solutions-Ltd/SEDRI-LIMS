using arc.app.Common;

namespace arc.app.Config.Mapper
{
    internal class CultureByIdMapper : IDefinition
    {
        public string Get()
        {
            return @"{
                        'Name': 'culturebyidmapper', 
                        'Type': 'Standard',
                        'Rules': [
                            { Key: '<:2:>', Type: 'Mapping', Source: 'SpecimenOrganismId', Value: 'SpecimenOrganismId' },
                            { Key: '<:3:>', Type: 'Mapping', Source: 'SpecimenQuantityId', Value: 'SpecimenQuantityId' },
                            { Key: '<:6:>', Type: 'Mapping', Source: 'SpecimenApiIdPanelId', Value: 'SpecimenApiIdPanelId' },
                            { Key: '<:7:>', Type: 'Mapping', Source: 'IdProfile', Value: 'IdProfile' },
                            { Key: '<:8:>', Type: 'Mapping', Source: 'IdPercentage', Value: 'IdPercentage' },
                            { Key: '<:9:>', Type: 'Mapping', Source: 'PositiveDate', Value: 'PositiveDate' },
                            { Key: '<:10:>', Type: 'Mapping', Source: 'PositiveTime', Value: 'PositiveTime' },
                            { Key: '<:11:>', Type: 'Mapping', Source: 'CommentOneId', Value: 'CommentOneId' },
                            { Key: '<:12:>', Type: 'Mapping', Source: 'CommentTwoId', Value: 'CommentTwoId' },
                            { Key: '<:13:>', Type: 'Mapping', Source: 'AdditionalNotes', Value: 'AdditionalNotes' },
                            { Key: '<:14:>', Type: 'Mapping', Source: 'AloquatId', Value: 'AloquatId' },
                            { Key: '<:15:>', Type: 'Mapping', Source: 'DisplayOnReport', Value: 'DisplayOnReport' },
                            { Key: '<:16:>', Type: 'Mapping', Source: 'CultureBottleWeight', Value: 'CultureBottleWeight' },
                            { Key: '<:17:>', Type: 'Mapping', Source: 'CultureBloodAndBottleWeight', Value: 'CultureBloodAndBottleWeight' }
                        ],
                        'Target': 
                            {
                                'Organism':'<:2:>',
                                'Quantity':'<:3:>',
                                'APIIDPanel':'<:6:>',
                                'IdProfile':'<:7:>',
                                'IdPercentage':'<:8:>',
                                'PositiveDate':'<:9:>',
                                'PositiveTime':'<:10:>',
                                'Comment1':'<:11:>',
                                'Comment2':'<:12:>',
                                'AdditionalNotes':'<:13:>',
                                'AliquotID':'<:14:>',
                                'DisplayInReport':'<:15:>',
                                'CultureBottleWeight':'<:16:>',
                                'CultureBloodAndBottleWeight':'<:17:>'
                            }
                     }";
        }
    }
}
