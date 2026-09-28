using arc.app.Common;

namespace arc.app.Config.Mapper
{
    internal class AddExpertRuleConditionMapper : IDefinition
    {
        public string Get()
        {
            return @"{  
                        'Name': 'addexpertruleconditionmapper', 
                        'Type': 'Standard',
                        'Rules': [
                            { Key: '<:1:>', Type: 'Mapping', Source: 'Id', Value: 'Id' },
                            { Key: '<:2:>', Type: 'Mapping', Source: 'antibioticid', Value: 'antibioticid' },
                            { Key: '<:3:>', Type: 'Mapping', Source: 'antibioticgroupid', Value: 'antibioticgroupid' },
                            { Key: '<:4:>', Type: 'Mapping', Source: 'SusceptibilityId', Value: 'SusceptibilityId' },
                            { Key: '<:5:>', Type: 'Mapping', Source: 'testmethodid', Value: 'testmethodid' },
                            { Key: '<:6:>', Type: 'Mapping', Source: 'startval', Value: 'startval' },
                            { Key: '<:7:>', Type: 'Mapping', Source: 'endval', Value: 'endval' }
                        ],
                        'Target' : {
                            ExpertRuleId: '<:1:>',
                            AntibioticId: '<:2:>',
                            AntibioticGroupId: '<:3:>',
                            SusceptibilityId: '<:4:>',
                            testmethodid: '<:5:>',
                            startval: '<:6:>',
                            endval: '<:7:>'
                        }
                     }";
        }
    }
}
