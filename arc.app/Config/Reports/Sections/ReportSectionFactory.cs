using arc.app.Common;

namespace arc.app.Config.Reports.Sections;

/// <summary>
/// Factory class to create instances of report sections based on the definition name.
/// </summary>
internal class ReportSectionFactory : IDefinitionFactory
{
    /// <summary>
    /// Creates an instance of a report section based on the provided definition name.
    /// </summary>
    /// <param name="definitionName">The name of the report section definition.</param>
    /// <returns>An instance of the corresponding report section, or null if the definition name is not recognized.</returns>
    public IDefinition Create(string definitionName)
    {
        return definitionName.ToLower() switch
        {
            "apipanelsection" => new ApiPanelSection(),
            "approvalsection" => new ApprovalSection(),
            "astsection" => new AstSection(),
            "auraminesection" => new AuramineSection(),
            "betalactamasesection" => new BetalactamaseSection(),
            "biochemistrysection" => new BiochemistrySection(),
            "bloodandbottleweightsection" => new BloodAndBottleWeightSection(),
            "carbapenemasesection" => new CarbapenemaseSection(),
            "catalasesection" => new CatalaseSection(),
            "cellcountsection" => new CellCountSection(),
            "culturecommentssection" => new CultureCommentsSection(),
            "culturedatesection" => new CultureDateSection(),
            "cultureresultsection" => new CultureResultSection(),
            "dipsticksection" => new DipstickSection(),
            "esblsection" => new EsblSection(),
            "gramculturesection" => new GramCultureSection(),
            "gramstainsection" => new GramStainSection(),
            "hpyloriantigensection" => new HpyloriAntigenSection(),
            "imagesection" => new AccreditationImageSection(),
            "indiainksection" => new IndiaInkSection(),
            "jevserologysection" => new JevSerologySection(),
            "kohprepsection" => new KohPrepSection(),
            "locationsection" => new LocationSection(),
            "microscopysection" => new MicroscopySection(),
            "organismlistsection" => new OrganismListSection(),
            "oxidasesection" => new OxidaseSection(),
            "patientdetailssection" => new PatientDetailsSection(),
            "precultureresultssection" => new PrecultureResultsSection(),
            "pregnancysection" => new PregnancySection(),
            "specimencommentssection" => new SpecimenCommentsSection(),
            "wetprepsection" => new WetPrepSection(),
            "wrightsstainsection" => new WrightsStainSection(),
            "znstainsection" => new ZnStainSection(),
            _ => null,
        };
    }
}
