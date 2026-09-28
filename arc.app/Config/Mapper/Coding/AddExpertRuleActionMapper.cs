using arc.app.Common;

namespace arc.app.Config.Mapper
{
    internal class AddExpertRuleActionMapper : IDefinition
    {
        /// <summary>
        /// Returns the mapper configuration for adding an expert rule action.
        /// Maps form fields including antibiotic, antibiotic group, and DisplayOnReport (Yes/No) to the ExpertRuleAction table.
        /// Antibiotic and antibiotic group are mutually exclusive in the UI; only one is stored per action.
        /// </summary>
        public string Get()
        {
            return @"{  
                        'Name': 'addexpertruleactionmapper', 
                        'Type': 'Standard',
                        'Rules': [
                            { Key: '<:1:>', Type: 'Mapping', Source: 'Id', Value: 'Id' },
                            { Key: '<:2:>', Type: 'Mapping', Source: 'antibioticid', Value: 'antibioticid' },
                            { Key: '<:3:>', Type: 'Mapping', Source: 'antibioticgroupid', Value: 'antibioticgroupid' },
                            { Key: '<:4:>', Type: 'Mapping', Source: 'SusceptibilityId', Value: 'SusceptibilityId' },
                            { Key: '<:5:>', Type: 'Mapping', Source: 'DisplayOnReport', Value: 'DisplayOnReport' }
                        ],
                        'Target' : {
                            ExpertRuleId: '<:1:>',
                            AntibioticId: '<:2:>',
                            AntibioticGroupId: '<:3:>',
                            SusceptibilityId: '<:4:>',
                            DisplayOnReport: '<:5:>'
                        }
                     }";
        }
    }
}
