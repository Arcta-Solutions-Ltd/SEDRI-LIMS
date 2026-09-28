using arc.app.Common;

namespace arc.app.Config.Mapper
{
    internal class SpecimenViewMapper : IDefinition
    {
        public string Get()
        {
            return @"{  
                        'Name': 'specimenviewmapper', 
                        'Type': 'Standard',
                        'Rules': [
                            { Key: '<:1:>', Type: 'Mapping', Source: 'AccessionNumber', Value: 'AccessionNumber' },
                            { Key: '<:2:>', Type: 'Mapping', Source: 'Surname', Value: 'Surname' },
                            { Key: '<:3:>', Type: 'Mapping', Source: 'PatientLocation', Value: 'PatientLocation' },
                            { Key: '<:4:>', Type: 'Mapping', Source: 'FullOrganisationName', Value: 'FullOrganisationName' },
                            { Key: '<:5:>', Type: 'Mapping', Source: 'AdmissionDate', Value: 'AdmissionDate' },
                            { Key: '<:6:>', Type: 'Mapping', Source: 'ClinicalContactNo', Value: 'ClinicalContactNo' },
                            { Key: '<:7:>', Type: 'Mapping', Source: 'Diagnosis', Value: 'Diagnosis' },
                            { Key: '<:8:>', Type: 'Mapping', Source: 'CollectionDate', Value: 'CollectionDate' },
                            { Key: '<:9:>', Type: 'Mapping', Source: 'SpecimenType', Value: 'SpecimenType' },
                            { Key: '<:10:>', Type: 'Mapping', Source: 'SpecimenSite', Value: 'SpecimenSite' },
                            { Key: '<:12:>', Type: 'Mapping', Source: 'Existingbarcode', Value: 'Existingbarcode' },
                            { Key: '<:13:>', Type: 'Mapping', Source: 'ReceivedDate', Value: 'ReceivedDate' },
                            { Key: '<:14:>', Type: 'Mapping', Source: 'ReceivedCondition', Value: 'ReceivedCondition' },
                            { Key: '<:15:>', Type: 'Mapping', Source: 'CollectionTime', Value: 'CollectionTime' },
                            { Key: '<:16:>', Type: 'Mapping', Source: 'ReceivedTime', Value: 'ReceivedTime' },
                            { Key: '<:17:>', Type: 'Mapping', Source: 'State', Value: 'State' },
                            { Key: '<:18:>', Type: 'Mapping', Source: 'SpecimenAppearance', Value: 'SpecimenAppearance' },
                            { Key: '<:19:>', Type: 'Mapping', Source: 'FirstName', Value: 'FirstName' },
                            { Key: '<:20:>', Type: 'Mapping', Source: 'BloodAndBottleWeight', Value: 'BloodAndBottleWeight' },
                            { Key: '<:21:>', Type: 'Mapping', Source: 'AntibioticsInLast24hrs', Value: 'AntibioticsInLast24hrs' },
                            { Key: '<:22:>', Type: 'Mapping', Source: 'TempInLast24hrs', Value: 'TempInLast24hrs' },
                            { Key: '<:23:>', Type: 'Mapping', Source: 'AdditionalClinicalInformation', Value: 'AdditionalClinicalInformation' },
                            { Key: '<:24:>', Type: 'Mapping', Source: 'FurtherInformation', Value: 'FurtherInformation' },
                            { Key: '<:25:>', Type: 'Mapping', Source: 'RequestCultureTests', Value: 'RequestCultureTests' },
                            { Key: '<:26:>', Type: 'Mapping', Source: 'MelioidosisCultureTests', Value: 'MelioidosisCultureTests' },
                            { Key: '<:27:>', Type: 'Mapping', Source: 'BottleOnlyWeight', Value: 'BottleOnlyWeight' },
                            { Key: '<:28:>', Type: 'Mapping', Source: 'RejectionReason', Value: 'RejectionReason' },
                            { Key: '<:29:>', Type: 'Mapping', Source: 'ReasonOne', Value: 'ReasonOne' },
                            { Key: '<:30:>', Type: 'Mapping', Source: 'ReasonTwo', Value: 'ReasonTwo' },
                            { Key: '<:31:>', Type: 'Mapping', Source: 'ApprovalCommentL1', Value: 'ApprovalCommentL1' },
                            { Key: '<:32:>', Type: 'Mapping', Source: 'ApprovalCommentL2', Value: 'ApprovalCommentl2' },
                            { Key: '<:33:>', Type: 'Mapping', Source: 'CancellationReason', Value: 'CancellationReason' },
                            { Key: '<:34:>', Type: 'Mapping', Source: 'SelectReason', Value: 'SelectReason' },
                            { Key: '<:35:>', Type: 'Mapping', Source: 'TestCategory', Value: 'TestCategory' },
                            { Key: '<:36:>', Type: 'Mapping', Source: 'CultureTypeCategory', Value: 'CultureTypeCategory' },
                            { Key: '<:37:>', Type: 'Mapping', Source: 'AgeYears', Value: 'AgeYears' },
                            { Key: '<:38:>', Type: 'Mapping', Source: 'AgeMonths', Value: 'AgeMonths' },
                            { Key: '<:39:>', Type: 'Mapping', Source: 'AgeDays', Value: 'AgeDays' },
                            { Key: '<:40:>', Type: 'Mapping', Source: 'AgeHours', Value: 'AgeHours' },
                            { Key: '<:68:>', Type: 'Mapping', Source: 'DateOfBirth', Value: 'DateOfBirth' },
                            { Key: '<:41:>', Type: 'Mapping', Source: 'tags', Value: 'tags' },
                            { Key: '<:42:>', Type: 'Mapping', Source: 'DateOfAdmission', Value: 'DateOfAdmission' },
                            { Key: '<:43:>', Type: 'Mapping', Source: 'TimeOfAdmission', Value: 'TimeOfAdmission' },
                            { Key: '<:44:>', Type: 'Mapping', Source: 'RequestReference', Value: 'RequestReference' },
                            { Key: '<:45:>', Type: 'Mapping', Source: 'RequestDate', Value: 'RequestDate' },
                            { Key: '<:46:>', Type: 'Mapping', Source: 'RequestTime', Value: 'RequestTime' },
                            { Key: '<:47:>', Type: 'Mapping', Source: 'RequestingClinician', Value: 'RequestingClinician' },
                            { Key: '<:48:>', Type: 'Mapping', Source: 'Ward', Value: 'Ward' },
                            { Key: '<:49:>', Type: 'Mapping', Source: 'Cot', Value: 'Cot' },
                            { Key: '<:50:>', Type: 'Mapping', Source: 'Urgency', Value: 'Urgency' },
                            { Key: '<:51:>', Type: 'Mapping', Source: 'Indication', Value: 'Indication' },
                            { Key: '<:52:>', Type: 'Mapping', Source: 'CurrentWeight', Value: 'CurrentWeight' },
                            { Key: '<:53:>', Type: 'Mapping', Source: 'CurrentWeightDate', Value: 'CurrentWeightDate' },
                            { Key: '<:54:>', Type: 'Mapping', Source: 'AntibioticTiming', Value: 'AntibioticTiming' },
                            { Key: '<:55:>', Type: 'Mapping', Source: 'AntibioticAgents', Value: 'AntibioticAgents' },
                            { Key: '<:56:>', Type: 'Mapping', Source: 'AntibioticStartDate', Value: 'AntibioticStartDate' },
                            { Key: '<:57:>', Type: 'Mapping', Source: 'AntibioticStartTime', Value: 'AntibioticStartTime' },
                            { Key: '<:58:>', Type: 'Mapping', Source: 'RecentSurgery', Value: 'RecentSurgery' },
                            { Key: '<:59:>', Type: 'Mapping', Source: 'CentralLineType', Value: 'CentralLineType' },
                            { Key: '<:60:>', Type: 'Mapping', Source: 'RespiratorySupport', Value: 'RespiratorySupport' },
                            { Key: '<:61:>', Type: 'Mapping', Source: 'BodySide', Value: 'BodySide' },
                            { Key: '<:62:>', Type: 'Mapping', Source: 'CollectionMethod', Value: 'CollectionMethod' },
                            { Key: '<:63:>', Type: 'Mapping', Source: 'CollectedBy', Value: 'CollectedBy' },
                            { Key: '<:64:>', Type: 'Mapping', Source: 'SpecimenLabelled', Value: 'SpecimenLabelled' },
                            { Key: '<:65:>', Type: 'Mapping', Source: 'VolumeMethod', Value: 'VolumeMethod' },
                            { Key: '<:66:>', Type: 'Mapping', Source: 'BottleType', Value: 'BottleType' },
                            { Key: '<:67:>', Type: 'Mapping', Source: 'EstimatedBloodVolume', Value: 'EstimatedBloodVolume' }
                        ],
                        'Target': 
                            {
                                Sections: [
                                    {
                                        Id: 'specimendetails',
                                        Title: '@SpeDet@',
                                        Fields: [
                                            { Id: 'accessionnumber', Label: '@SpeAcc@', Highlight: true, Value: '<:1:>' },
                                            { Id: 'state', Label: '@GenSta@', Highlight: true, Value: '<:17:>' }
                                        ],
                                        SubSections: [
                                            {
                                                Id: 'collection', Title: '',
                                                Fields: [
                                                    { Id: 'patientname', Label: '@PatPatA@', Value: '<:19:> <:2:>' },
                                                    { Id: 'patientlocation', Label: '@GenLoc@', Value: '<:3:>' },
                                                    { Id: 'ward', Label: '@GenLocA@', Value: '<:4:>' },
                                                    { Id: 'admissiondate', Label: '@PatAdm@', Value: '<:5:>' },
                                                    { Id: 'clinicalcontact', Label: '@GenConA@', Value: '<:6:>' },
                                                    { Id: 'diagnosis', Label: '@GenDia@', Value: '<:7:>' },
                                                    { Id: 'tags', Label: '@GenTagA@', Value: '<:41:>' }
                                                ]
                                            },
                                            {
                                                Id: 'receipt', Title: '',
                                                Fields: [
                                                    { Id: 'specimentype', Label: '@SpeSpeB@', Value: '<:9:>' },
                                                    { Id: 'specimensite', Label: '@SpeSpeC@', Value: '<:10:>' },
                                                    { Id: 'bottleonlyweight', Label: '@SpeBot@', Value: '<:27:>' },
                                                    { Id: 'bloodandbottleweight', Label: '@SpeBlo@', Value: '<:20:>' },
                                                    { Id: 'existingbarcode', Label: '@SpeExi@', Value: '<:12:>' },
                                                    { Id: 'collectiondate', Label: '@SpeColC@', Value: '<:8:>' },
                                                    { Id: 'collectiontime', Label: '@SpeColD@', Value: '<:15:>' },
                                                    { Id: 'receiveddate', Label: '@SpeRecD@', Value: '<:13:>' },
                                                    { Id: 'receivedtime', Label: '@SpeRecE@', Value: '<:16:>' },
                                                    { Id: 'patientageatspecimen', Label: '@SpeAge@', Value: '<:68:>' },
                                                    { Id: 'bodyside', Label: '@NeoBodSid@', Value: '<:61:>' },
                                                    { Id: 'collectionmethod', Label: '@NeoColMet@', Value: '<:62:>' },
                                                    { Id: 'collectedby', Label: '@NeoColBy@', Value: '<:63:>' },
                                                    { Id: 'specimenlabelled', Label: '@NeoSpeLab@', Value: '<:64:>' },
                                                    { Id: 'volumemethod', Label: '@NeoVolMet@', Value: '<:65:>' },
                                                    { Id: 'bottletype', Label: '@NeoBotTyp@', Value: '<:66:>' },
                                                    { Id: 'estimatedbloodvolume', Label: '@NeoEstVol@', Value: '<:67:>' }
                                                ]
                                            },
                                            {
                                                Id: 'request', Title: '',
                                                Fields: [
                                                    { Id: 'AntibioticsInLast24hrs', Label: '@CusAnt@', Value: '<:21:>' },
                                                    { Id: 'TempInLast24hrs', Label: '@CusTem@', Value: '<:22:>' },
                                                    { Id: 'AdditionalClinicalInformation', Label: '@CusAdd@', Value: '<:23:>' },
                                                    { Id: 'FurtherInformation', Label: '@CusFur@', Value: '<:24:>' },
                                                    { Id: 'RequestCultureTests', Label: '@CusReq@', Value: '<:25:>' },
                                                    { Id: 'MelioidosisCultureTests', Label: '@CusMel@', Value: '<:26:>' },
                                                    { Id: 'TestCategory', Label: '@GenTesH@', Value: '<:35:>' },
                                                    { Id: 'CultureTypeCategory', Label: '@GenCulA@', Value: '<:36:>' }
                                                ]
                                            },
                                            {
                                                Id: 'initialassess', Title: '',
                                                Fields: [
                                                    { Id: 'condition', Label: '@SpeSpeE@', Value: '<:14:>' },
                                                    { Id: 'appearance', Label: '@SpeSpeF@', Value: '<:18:>' },
                                                    { Id: 'rejectionreason', Label: '@SpeReaH@', Value: '<:28:>' },
                                                    { Id: 'selectreason', Label: '@SpeReaG@', Value: '<:34:>' }
                                                ]
                                            },
                                            {
                                                Id: 'review', Title: '',
                                                Fields: [
                                                    { Id: 'ApprovalCommentL1', Label: '@SpeReaD@', Value: '<:31:>' },
                                                    { Id: 'ReasonOne', Label: '@SpeReaB@', Value: '<:29:>' },
                                                    { Id: 'ApprovalCommentL2', Label: '@SpeReaE@', Value: '<:32:>' },
                                                    { Id: 'ReasonTwo', Label: '@SpeReaC@', Value: '<:30:>' },
                                                    { Id: 'CancellationReason', Label: '@SpeCanC@', Value: '<:33:>' }
                                                ]
                                            }
                                        ]
                                    },
                                    {
                                        Id: 'admissiondetails',
                                        Title: '@NeoAdm@',
                                        Fields: [
                                            { Id: 'dateofadmission', Label: '@NeoAdmDat@', Value: '<:42:>' },
                                            { Id: 'timeofadmission', Label: '@NeoAdmTim@', Value: '<:43:>' }
                                        ]
                                    },
                                    {
                                        Id: 'requestdetails',
                                        Title: '@NeoReq@',
                                        Fields: [
                                            { Id: 'requestreference', Label: '@NeoReqRef@', Value: '<:44:>' },
                                            { Id: 'requestdate', Label: '@NeoReqDat@', Value: '<:45:>' },
                                            { Id: 'requesttime', Label: '@NeoReqTim@', Value: '<:46:>' },
                                            { Id: 'requestingclinician', Label: '@NeoCliNam@', Value: '<:47:>' },
                                            { Id: 'requestward', Label: '@NeoWar@', Value: '<:48:>' },
                                            { Id: 'cot', Label: '@NeoCot@', Value: '<:49:>' },
                                            { Id: 'urgency', Label: '@NeoUrg@', Value: '<:50:>' },
                                            { Id: 'indication', Label: '@NeoInd@', Value: '<:51:>' }
                                        ],
                                        SubSections: [
                                            {
                                                Id: 'clinicalstate', Title: '@NeoCliSta@',
                                                Fields: [
                                                    { Id: 'currentweight', Label: '@NeoCurWei@', Value: '<:52:>' },
                                                    { Id: 'currentweightdate', Label: '@NeoCurWeiDat@', Value: '<:53:>' },
                                                    { Id: 'antibiotictiming', Label: '@NeoAntTim@', Value: '<:54:>' },
                                                    { Id: 'antibioticagents', Label: '@NeoAntAge@', Value: '<:55:>' },
                                                    { Id: 'antibioticstartdate', Label: '@NeoAntStaDat@', Value: '<:56:>' },
                                                    { Id: 'antibioticstarttime', Label: '@NeoAntStaTim@', Value: '<:57:>' },
                                                    { Id: 'recentsurgery', Label: '@NeoRecSur@', Value: '<:58:>' },
                                                    { Id: 'centrallinetype', Label: '@NeoCenLinTyp@', Value: '<:59:>' },
                                                    { Id: 'respiratorysupport', Label: '@NeoResSup@', Value: '<:60:>' }
                                                ]
                                            }
                                        ]
                                    }
                                ]
                            }
                     }";
        }
    }
}


