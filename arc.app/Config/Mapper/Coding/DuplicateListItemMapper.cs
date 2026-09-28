using arc.app.Common;

namespace arc.app.Config.Mapper
{
    /// <summary>
    /// Supplies the parameters for duplicatelistitemquery, which counts other entries holding the same value in
    /// the same table.
    /// </summary>
    /// <remarks>
    /// The entry's value is read as a Key/value pair out of the crafted contents rather than by the name 'Value'.
    /// Source matching is case insensitive and takes the first field in document order, so a rule of
    /// Source: 'Value' matches the 'value' property of whichever Key/value pair the form happens to send first
    /// and the real value is missed. When that happens the Value parameter is dropped and the count matches
    /// every entry in the table, so any table that already holds an entry reports a duplicate.
    /// </remarks>
    internal class DuplicateListItemMapper : IDefinition
    {
        public string Get()
        {
            return @"{  
                        'Name': 'duplicatelistitemmapper', 
                        'Type': 'Standard',
                        'Rules': [
                            { Key: '<:1:>', Type: 'Mapping', Source: 'Key', Where: 'Value', Value: 'Value' },
                            { Key: '<:2:>', Type: 'Mapping', Source: 'metaflistid', Value: 'metaflistid' },
                            { Key: '<:3:>', Type: 'Mapping', Source: 'id', Value: 'id' }
                        ],
                        'Target' : { 
                            'Name': 'codinglistexists', 
                            'Parameters': [ 
                                {'Key': 'ListId', 'Value': '<:2:>'},
                                {'Key': 'Value', 'Value': '<:1:>'},
                                {'Key': 'Deleted', 'Value': 'false'},
                                {'Key': 'Id', 'Value': '<:3:>'}
                            ] 
                        }
                     }";
        }
    }
}
