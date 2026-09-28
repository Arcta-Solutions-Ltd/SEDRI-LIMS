using arc.app.Common;

namespace arc.app.Config.Queries.Specification;

/// <summary>
/// Query configuration for the edit specification form initial load.
/// </summary>
internal class EditSpecificationQuery : IDefinition
{
    public string Get()
    {
        return @"{ 
                        'Query': 'EditSpecification',
                        'TableName': 'Specification',
                        'Type': 'Special'
                    }";
    }
}
