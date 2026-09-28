using arc.app.Common;

namespace arc.app.Config.Forms.Specification;

/// <summary>
/// Factory for Specification form configurations (document, version, year add/delete).
/// </summary>
/// <remarks>
/// Definition names are matched case-insensitively. Returns null for unknown names.
/// </remarks>
internal class SpecificationFormFactory : IDefinitionFactory
{
    /// <inheritdoc />
    public IDefinition Create(string definitionName)
    {
        return definitionName.ToLower() switch
        {
            "adddocumentform" => new AddDocumentFormConfig(),
            "addspecificationform" => new AddSpecificationFormConfig(),
            "deletedocumentform" => new DeleteDocumentFormConfig(),
            "deletespecificationform" => new DeleteSpecificationFormConfig(),
            "addversionform" => new AddVersionFormConfig(),
            "deleteversionform" => new DeleteVersionFormConfig(),
            "addyearform" => new AddYearFormConfig(),
            "deleteyearform" => new DeleteYearFormConfig(),
            "editspecificationform" => new EditSpecificationFormConfig(),
            _ => null,
        };
    }
}
