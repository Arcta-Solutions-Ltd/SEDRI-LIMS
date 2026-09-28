using arc.app.Common;

namespace arc.app.Config.Reports.DataSection.Specimen;

/// <summary>
/// Defines the configuration for the Specimen Core data section.
/// This data section exposes core specimen, patient, and request fields available in report
/// data.Standard but not declared in test-specific or other sectional data sections.
/// Field Value keys must match SpecimenViewMapper / report Standard keys exactly.
/// </summary>
internal class SpecimenCoreDataSection : IDefinition
{
    /// <summary>
    /// Gets the JSON definition of the Specimen Core data section.
    /// </summary>
    /// <returns>A JSON string containing the data section configuration with available fields.</returns>
    public string Get()
    {
        return """
            {
                "Name": "SpecimenCoreDataSection",
                "Title": "@SpeDet@",
                "Fields": [
                    { "Label": "@GenLabA@", "Value": "LaboratoryName" },
                    { "Label": "@SpeAcc@", "Value": "AccessionNumber" },
                    { "Label": "@GenSta@", "Value": "State" },
                    { "Label": "@PatAdm@", "Value": "AdmissionDate" },
                    { "Label": "@PatCli@", "Value": "ClinicalContactNo" },
                    { "Label": "@GenDia@", "Value": "Diagnosis" },
                    { "Label": "@SpeColC@", "Value": "CollectionDate" },
                    { "Label": "@SpeColD@", "Value": "CollectionTime" },
                    { "Label": "@SpeRecD@", "Value": "ReceivedDate" },
                    { "Label": "@SpeRecE@", "Value": "ReceivedTime" },
                    { "Label": "@SpeSpeB@", "Value": "SpecimenType" },
                    { "Label": "@SpeSpeC@", "Value": "SpecimenSite" },
                    { "Label": "@SpeSpeE@", "Value": "ReceivedCondition" },
                    { "Label": "@SpeSpeF@", "Value": "SpecimenAppearance" },
                    { "Label": "@SpeExi@", "Value": "ExistingBarcode" },
                    { "Label": "@SpeBot@", "Value": "BottleOnlyWeight" },
                    { "Label": "@SpeBlo@", "Value": "BloodAndBottleWeight" },
                    { "Label": "@RepPatLoc@", "Value": "PatientLocation" },
                    { "Label": "@CusAdd@", "Value": "AdditionalClinicalInformation" },
                    { "Label": "@CusFur@", "Value": "FurtherInformation" },
                    { "Label": "@CusReq@", "Value": "RequestCultureTests" },
                    { "Label": "@CusMel@", "Value": "MelioidosisCultureTests" },
                    { "Label": "@SpeReaH@", "Value": "RejectionReason" },
                    { "Label": "@SpeReaB@", "Value": "ReasonOne" },
                    { "Label": "@SpeReaC@", "Value": "ReasonTwo" },
                    { "Label": "@SpeCanC@", "Value": "CancellationReason" },
                    { "Label": "@SpeReaG@", "Value": "SelectReason" },
                    { "Label": "@SpeReaD@", "Value": "ApprovalCommentL1" },
                    { "Label": "@SpeReaE@", "Value": "ApprovalCommentL2" },
                    { "Label": "@GenTesH@", "Value": "TestCategory" },
                    { "Label": "@GenCulA@", "Value": "CultureTypeCategory" },
                    { "Label": "@SpeAge@", "Value": "AgeYears" },
                    { "Label": "@GenTagA@", "Value": "tags" },
                    { "Label": "@NeoAdmDat@", "Value": "DateOfAdmission" },
                    { "Label": "@NeoAdmTim@", "Value": "TimeOfAdmission" },
                    { "Label": "@NeoReqRef@", "Value": "RequestReference" },
                    { "Label": "@NeoReqDat@", "Value": "RequestDate" },
                    { "Label": "@NeoReqTim@", "Value": "RequestTime" },
                    { "Label": "@NeoCliNam@", "Value": "RequestingClinician" },
                    { "Label": "@NeoWar@", "Value": "Ward" },
                    { "Label": "@NeoCot@", "Value": "Cot" },
                    { "Label": "@NeoUrg@", "Value": "Urgency" },
                    { "Label": "@NeoInd@", "Value": "Indication" },
                    { "Label": "@NeoCurWei@", "Value": "CurrentWeight" },
                    { "Label": "@NeoCurWeiDat@", "Value": "CurrentWeightDate" },
                    { "Label": "@NeoAntTim@", "Value": "AntibioticTiming" },
                    { "Label": "@NeoAntAge@", "Value": "AntibioticAgents" },
                    { "Label": "@NeoAntStaDat@", "Value": "AntibioticStartDate" },
                    { "Label": "@NeoAntStaTim@", "Value": "AntibioticStartTime" },
                    { "Label": "@NeoRecSur@", "Value": "RecentSurgery" },
                    { "Label": "@NeoCenLinTyp@", "Value": "CentralLineType" },
                    { "Label": "@NeoResSup@", "Value": "RespiratorySupport" },
                    { "Label": "@NeoBodSid@", "Value": "BodySide" },
                    { "Label": "@NeoColMet@", "Value": "CollectionMethod" },
                    { "Label": "@NeoColBy@", "Value": "CollectedBy" },
                    { "Label": "@NeoSpeLab@", "Value": "SpecimenLabelled" },
                    { "Label": "@NeoVolMet@", "Value": "VolumeMethod" },
                    { "Label": "@NeoBotTyp@", "Value": "BottleType" },
                    { "Label": "@NeoEstVol@", "Value": "EstimatedBloodVolume" },
                    { "Label": "@CusAnt@", "Value": "AntibioticsInLast24hrs" },
                    { "Label": "@CusTem@", "Value": "TempInLast24hrs" }
                ]
            }
            """;
    }
}
