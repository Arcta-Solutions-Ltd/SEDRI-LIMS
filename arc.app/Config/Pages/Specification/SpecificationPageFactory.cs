using arc.app.Common;

namespace arc.app.Config.Pages.Specification;

/// <summary>
/// Factory for Specification page configurations (document, version, year add/delete).
/// </summary>
/// <remarks>
/// Definition names are matched case-insensitively. Returns null for unknown names.
/// </remarks>
internal class SpecificationPageFactory : IDefinitionFactory
{
    /// <inheritdoc />
    public IDefinition Create(string definitionName)
    {
        return definitionName.ToLower() switch
        {
            "adddocumentpage" => new AddDocumentPageConfig(),
            "addspecificationpage" => new AddSpecificationPageConfig(),
            "deletedocumentpage" => new DeleteDocumentPageConfig(),
            "deletespecificationpage" => new DeleteSpecificationPageConfig(),
            "addversionpage" => new AddVersionPageConfig(),
            "deleteversionpage" => new DeleteVersionPageConfig(),
            "addyearpage" => new AddYearPageConfig(),
            "deleteyearpage" => new DeleteYearPageConfig(),
            "editspecificationpage" => new EditSpecificationPageConfig(),
            _ => null,
        };
    }
}
