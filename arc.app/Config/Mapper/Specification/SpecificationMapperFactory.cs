using arc.app.Common;

namespace arc.app.Config.Mapper.Specification;

/// <summary>
/// Factory for Specification mapper configurations (document, version, year add/delete and validation).
/// </summary>
/// <remarks>
/// Definition names are matched case-insensitively. Returns null for unknown names.
/// </remarks>
internal class SpecificationMapperFactory : IDefinitionFactory
{
    /// <inheritdoc />
    public IDefinition Create(string definitionName)
    {
        return definitionName.ToLower() switch
        {
            "adddocumentmapper" => new AddDocumentMapper(),
            "addversionmapper" => new AddVersionMapper(),
            "addyearmapper" => new AddYearMapper(),
            "documentlistexistsmapper" => new DocumentListExistsMapper(),
            "versionlistexistsmapper" => new VersionListExistsMapper(),
            "yearlistexistsmapper" => new YearListExistsMapper(),
            "idtodocumentidmapper" => new IdToDocumentIdMapper(),
            "idtoversionnumberidmapper" => new IdToVersionNumberIdMapper(),
            "idtopublicationyearidmapper" => new IdToPublicationYearIdMapper(),
            _ => null,
        };
    }
}
