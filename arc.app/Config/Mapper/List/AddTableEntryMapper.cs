using arc.app.Common;

namespace arc.app.Config.Mapper
{
    /// <summary>
    /// Flattens the Add Table Entry form payload into the table entry that AddTableEntryEvent saves.
    /// </summary>
    /// <remarks>
    /// The entry's value is read as a Key/value pair out of the crafted contents. Source matching is case
    /// insensitive and takes the first field in document order, so a rule of Source: 'Value' matches the 'value'
    /// property of whichever Key/value pair the form sends first instead of the entry's value. Enabled, ParentId
    /// and MetafListId are named uniquely in the payload and so are matched directly.
    /// </remarks>
    internal class AddTableEntryMapper : IDefinition
    {
        public string Get()
        {
            return @"{  
                        'Name': 'addtableentrymapper', 
                        'Type': 'Standard',
                        'Rules': [
                            { Key: '<:1:>', Type: 'Mapping', Source: 'Key', Where: 'Value', Value: 'Value' },
                            { Key: '<:2:>', Type: 'Mapping', Source: 'Enabled', Value: 'Enabled' },
                            { Key: '<:3:>', Type: 'Mapping', Source: 'ParentId', Value: 'ParentId' },
                            { Key: '<:4:>', Type: 'Mapping', Source: 'MetafListId', Value: 'MetafListId' }
                        ],
                        'Target' : { 
                            ListId: '<:4:>',
                            Value: '<:1:>',
                            ParentId: '<:3:>',
                            Enabled: '<:2:>',
                            DisplayOrder: 1,
                            Fixed: false
                        }
                     }";
        }
    }
}

