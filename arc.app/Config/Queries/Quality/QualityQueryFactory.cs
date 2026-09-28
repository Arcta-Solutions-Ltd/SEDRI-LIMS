using arc.app.Common;

namespace arc.app.Config.Queries;

internal class QualityQueryFactory : IDefinitionFactory
{
    public IDefinition Create(string definitionName)
    {
        return definitionName.ToLower() switch
        {
            "addiqctestquery" => new AddIqcTestQuery(),
            "deleteiqcresultquery" => new DeleteIqcResultQuery(),
            "duplicateiqctestprofilenamequery" => new DuplicateIqcTestProfileNameQuery(),
            "editiqcresult" => new EditIqcResultQuery(),
            "editiqctestprofileqcorganismquery" => new EditIqcTestProfileQcOrganismQuery(),
            "editiqctestqcorganismsquery" => new EditIqcTestQcOrganismsQuery(),
            "iqcresultsgridquery" => new IqcResultsGridQuery(),
            "iqctestfordeletequery" => new IqcTestForDeleteQuery(),
            "iqctestforrecordview" => new IqcTestForRecordViewQuery(),
            "iqctestprofilelistquery" => new IqcTestProfileListQuery(),
            "iqctestprofileqcantibioticsdisk" => new IqcTestProfileQcAntibioticsDiskQuery(),
            "iqctestprofileqcantibioticsmic" => new IqcTestProfileQcAntibioticsMicQuery(),
            "iqctestprofilesinglequery" => new IqcTestProfileSingleQuery(),
            "iqctestslist" => new IqcTestsListQuery(),
            "markiqctestcompletequery" => new MarkIqcTestCompleteQuery(),
            "qcantimicrobiallistbyqcorganismid" => new QcAntimicrobialListByQcOrganismIdQuery(),
            "qcorganismforiqctestprofileview" => new QcOrganismForIqcTestProfileViewQuery(),
            "runiqctestquery" => new RunIqcTestQuery(),
            "singleiqctestforiqctestslistquery" => new SingleIqcTestForIqcTestsListQuery(),
            _ => null
        };
    }
}
