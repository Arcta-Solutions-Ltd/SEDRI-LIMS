using arc.app.Common;
using arc.app.Config.Reports.DataSection.Specimen;

namespace arc.app.Config.Reports.DataSection;

/// <summary>
/// Factory class to create specimen data section definitions based on the provided definition name.
/// </summary>
internal class SpecimenDataSectionFactory : IDefinitionFactory
{
    /// <summary>
    /// Creates an instance of a specimen data section definition based on the provided definition name.
    /// </summary>
    /// <param name="definitionName">The name of the data section definition to create.</param>
    /// <returns>An instance of the corresponding data section definition, or null if the definition name is not recognized.</returns>
    public IDefinition Create(string definitionName)
    {
        return definitionName.ToLower() switch
        {
            "apipaneldatasection" => new ApiPanelDataSection(),
            "approvaldatasection" => new ApprovalDataSection(),
            "astdatasection" => new AstDataSection(),
            "auraminedatasection" => new AuramineDataSection(),
            "betalactamasedatasection" => new BetalactamaseDataSection(),
            "biochemistrydatasection" => new BiochemistryDataSection(),
            "carbapenemasedatasection" => new CarbapenemaseDataSection(),
            "catalasedatasection" => new CatalaseDataSection(),
            "cellcountdatasection" => new CellCountDataSection(),
            "culturecommentsdatasection" => new CultureCommentsDataSection(),
            "culturedatedatasection" => new CultureDateDataSection(),
            "cultureresultdatasection" => new CultureResultDataSection(),
            "dipstickdatasection" => new DipstickDataSection(),
            "esbldatasection" => new EsblDataSection(),
            "gramculturedatasection" => new GramCultureDataSection(),
            "gramstaindatasection" => new GramStainDataSection(),
            "hpyloriantigendatasection" => new HpyloriAntigenDataSection(),
            "imagedatasection" => new ImageDataSection(),
            "indiainkdatasection" => new IndiaInkDataSection(),
            "jevserologydatasection" => new JevSerologyDataSection(),
            "kohprepdatasection" => new KohPrepDataSection(),
            "locationdatasection" => new LocationDataSection(),
            "microscopydatasection" => new MicroscopyDataSection(),
            "organismlistdatasection" => new OrganismListDataSection(),
            "oxidasedatasection" => new OxidaseDataSection(),
            "patientdetailsdatasection" => new PatientDetailsDataSection(),
            "precultureresultsdatasection" => new PrecultureResultsDataSection(),
            "pregnancydatasection" => new PregnancyDataSection(),
            "specimencoredatasection" => new SpecimenCoreDataSection(),
            "specimencommentsdatasection" => new SpecimenCommentsDataSection(),
            "wetprepdatasection" => new WetPrepDataSection(),
            "wrightsstaindatasection" => new WrightsStainDataSection(),
            "znstaindatasection" => new ZnStainDataSection(),
            _ => null,
        };
    }
}
