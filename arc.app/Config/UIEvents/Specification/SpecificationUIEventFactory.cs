using arc.app.Common;

namespace arc.app.Config.UIEvents.Specification;

/// <summary>
/// Factory for Specification UI event configurations (document, version, year add/delete).
/// </summary>
/// <remarks>
/// Definition names are matched case-insensitively. Returns null for unknown names.
/// </remarks>
internal class SpecificationUIEventFactory : IDefinitionFactory
{
    /// <inheritdoc />
    public IDefinition Create(string definitionName)
    {
        return definitionName.ToLower() switch
        {
            "adddocumentuievent" => new AddDocumentUIEventConfig(),
            "addspecificationuievent" => new AddSpecificationUIEventConfig(),
            "deletedocumentuievent" => new DeleteDocumentUIEventConfig(),
            "deletespecificationuievent" => new DeleteSpecificationUIEventConfig(),
            "addversionuievent" => new AddVersionUIEventConfig(),
            "deleteversionuievent" => new DeleteVersionUIEventConfig(),
            "addyearuievent" => new AddYearUIEventConfig(),
            "deleteyearuievent" => new DeleteYearUIEventConfig(),
            "editspecificationuievent" => new EditSpecificationUIEventConfig(),
            _ => null,
        };
    }
}
