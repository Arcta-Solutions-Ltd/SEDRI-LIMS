using arc.app.Common;

namespace arc.app.Config.Events.Specification;

/// <summary>
/// Factory for Specification event configurations (document, version, year add/delete).
/// </summary>
/// <remarks>
/// Definition names are matched case-insensitively. Returns null for unknown names.
/// </remarks>
internal class SpecificationEventFactory : IDefinitionFactory
{
    /// <inheritdoc />
    public IDefinition Create(string definitionName)
    {
        return definitionName.ToLower() switch
        {
            "adddocument" => new AddDocumentEventConfig(),
            "addspecification" => new AddSpecificationEventConfig(),
            "deletedocument" => new DeleteDocumentEventConfig(),
            "deletespecification" => new DeleteSpecificationEventConfig(),
            "addversion" => new AddVersionEventConfig(),
            "deleteversion" => new DeleteVersionEventConfig(),
            "addyear" => new AddYearEventConfig(),
            "deleteyear" => new DeleteYearEventConfig(),
            "editspecification" => new EditSpecificationEventConfig(),
            _ => null,
        };
    }
}
