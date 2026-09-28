using arc.app.Common;

namespace arc.app.Config.Queries.Specification;

/// <summary>
/// Factory for Specification query configurations (document, version, year list item validation).
/// </summary>
/// <remarks>
/// Definition names are matched case-insensitively. Returns null for unknown names.
/// </remarks>
internal class SpecificationQueryFactory : IDefinitionFactory
{
    /// <inheritdoc />
    public IDefinition Create(string definitionName)
    {
        return definitionName.ToLower() switch
        {
            "documentlistitemexists" => new DocumentListItemExistsQuery(),
            "checkwhetherdocumentinspecification" => new CheckWhetherDocumentInSpecificationQuery(),
            "deletespecificationquery" => new DeleteSpecificationQuery(),
            "editspecificationquery" => new EditSpecificationQuery(),
            "versionlistitemexists" => new VersionListItemExistsQuery(),
            "checkwhetherversioninspecification" => new CheckWhetherVersionInSpecificationQuery(),
            "yearlistitemexists" => new YearListItemExistsQuery(),
            "checkwhetheryearinspecification" => new CheckWhetherYearInSpecificationQuery(),
            "singlespecificationforspecificationlist" => new SingleSpecificationForSpecificationListQuery(),
            "specificationexists" => new SpecificationExistsQuery(),
            "specificationinuse" => new SpecificationInUseQuery(),
            "specificationlist" => new SpecificationListQuery(),
            _ => null,
        };
    }
}
