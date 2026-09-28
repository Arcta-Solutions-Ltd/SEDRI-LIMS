using arc.app.Common;
using arc.app.Config.Mapper.Specimen;

namespace arc.app.Config.Mapper;

/// <summary>
/// Factory class for creating <see cref="IDefinition"/> instances based on a string identifier.
/// </summary>
/// <remarks>
/// This implementation supports a wide range of specimen-related mappers, each keyed by a lowercase
/// string name. The factory uses a <c>switch</c> expression to instantiate the appropriate mapper
/// implementation. If the name is not recognized, <c>null</c> is returned.
/// </remarks>
internal class SpecimenMapperFactory : IDefinitionFactory
{
    /// <summary>
    /// Creates an <see cref="IDefinition"/> instance corresponding to the specified definition name.
    /// </summary>
    /// <param name="definitionName">The name of the mapper to create (case-insensitive).</param>
    /// <returns>
    /// An instance of the requested <see cref="IDefinition"/> implementation, or <c>null</c> if the name is unrecognized.
    /// </returns>
    public IDefinition Create(string definitionName)
    {
        return definitionName.ToLower() switch
        {
            "addculturemapper" => new AddCultureMapper(),
            "addspecimentagmapper" => new AddSpecimenTagMapper(),
            "managespecimenattachmentsmapper" => new ManageSpecimenAttachmentsMapper(),
            "managecultureattachmentsmapper" => new ManageCultureAttachmentsMapper(),
            "addisolatemapper" => new AddIsolateMapper(),
            "aliquotforeditmapper" => new AliquotForEditMapper(),
            "astreportmapper" => new ASTReportMapper(),
            "astreportheadermapper" => new ASTReportHeaderMapper(),
            "culturebyidmapper" => new CultureByIdMapper(),
            "culturecommentformquerymapper" => new CultureCommentFormQueryMapper(),
            "culturecommentmapper" => new CultureCommentMapper(),
            "culturereportmapper" => new CultureReportMapper(),
            "cultureattachmentsviewmapper" => new CultureAttachmentsViewMapper(),
            "cultureviewmapper" => new CultureViewMapper(),
            "day0benchreadmapper" => new Day0BenchReadMapper(),
            "day1benchreadmapper" => new Day1BenchReadMapper(),
            "deletecommentmapper" => new DeleteCommentMapper(),
            "editaliquotmapper" => new EditAliquotMapper(),
            "editcommenteventmapper" => new EditCommentEventMapper(),
            "editcommentforselectoreventmapper" => new EditCommentForSelectorEventMapper(),
            "editcommentquerymapper" => new EditCommentQueryMapper(),
            "editculturemapper" => new EditCultureMapper(),
            "editspecimenmapper" => new EditSpecimenMapper(),
            "isolateviewmapper" => new IsolateViewMapper(),
            "rejectspecimenmapper" => new RejectSpecimenMapper(),
            "remotespecimenmapper" => new RemoteSpecimenMapper(),
            "specimencommentmapper" => new SpecimenCommentMapper(),
            "specimendiaryparametermapper" => new SpecimenDiaryParameterMapper(),
            "specimendiaryresultmapper" => new SpecimenDiaryResultMapper(),
            "specimenlabelavailablefieldsmapper" => new SpecimenLabelAvailableFieldsMapper(),
            "specimenlabelfieldsmapper" => new SpecimenLabelFieldsMapper(),
            "specimenreportheadermapper" => new SpecimenReportHeaderMapper(),
            "specimenreportpatientmapper" => new SpecimenReportPatientMapper(),
            "specimenattachmentsviewmapper" => new SpecimenAttachmentsViewMapper(),
            "specimenviewmapper" => new SpecimenViewMapper(),
            "submitspecimenmapper" => new SubmitSpecimenMapper(),
            _ => null,
        };
    }
}