using arc.app.Common;

namespace arc.app.Config.Queries.Specimen;

/// <summary>
/// Configuration for the "Specimen For Specimen View" query.
/// </summary>
internal class SpecimenForSpecimenViewQuery : IDefinition
{
    /// <summary>
    /// Retrieves the configuration for the "Specimen For Specimen View" query.
    /// </summary>
    /// <remarks>
    /// This configuration defines the query name, table name, type, translation settings, 
    /// result mapping, fields, joins, list items, and where conditions for retrieving specimen data.
    /// </remarks>
    /// <returns>
    /// A string representation of the query configuration, including metadata, fields, joins, and filtering rules.
    /// </returns>
    public string Get()
    {
        return @"{
                        'Query': 'SpecimenForSpecimenView',
                        'TableName': 'Specimen',
                        'Type': 'Single',
                        'Translate': true,
                        'ResultMapping': 'specimenviewmapper',
                        'Fields': [
                            { 'Name': 'AdmissionDate', 'Type': 'date' },
                            { 'Name': 'ClinicalContactNo', 'Type': 'string' },
                            { 'Name': 'AccessionNumber', 'Type': 'string'},
                            { 'Name': 'ExistingBarcode', 'Type': 'string'},
                            { 'Name': 'BottleOnlyWeight', 'Type': 'numeric'},
                            { 'Name': 'BloodAndBottleWeight', 'Type': 'numeric'},
                            { 'Name': 'CollectionDate', 'Type': 'date'},
                            { 'Name': 'ReceivedDate', 'Type': 'date'},
                            { 'Name': 'CollectionTime', 'Type': 'string'},
                            { 'Name': 'ReceivedTime', 'Type': 'string'},
                            { 'Name': 'FurtherInformation', 'Type': 'string'},
                            { 'Name': 'RequestCultureTests', 'Type': 'string'},
                            { 'Name': 'MelioidosisCultureTests', 'Type': 'string'},
                            { 'Name': 'AdditionalClinicalInformation', 'Type': 'string'},
                            { 'Name': 'RejectionReason', 'Type': 'string'},
                            { 'Name': 'ReasonOne', 'Type': 'string'},
                            { 'Name': 'ReasonTwo', 'Type': 'string'},
                            { 'Name': 'CancellationReason', 'Type': 'string'},
                            { 'Name': 'AgeYears', 'Type': 'int' },
                            { 'Name': 'AgeMonths', 'Type': 'int' },
                            { 'Name': 'AgeDays', 'Type': 'int' },
                            { 'Name': 'AgeHours', 'Type': 'int' },
                            { 'Name': 'EstimatedBloodVolume', 'Type': 'numeric' },
                            { 'Name': 'Tags', 'Type': 'specimentags', 'KnownAs': 'tags' }
                        ],
                        'Joins': [
                            { 'Table': 'Patient', 'Fields': [{'Name': 'FirstName'}, { 'Name': 'Surname'}, { 'Name': 'DateOfBirth'}] },
                            { 'Table': 'Laboratory', 'Fields': [{'Name': 'LaboratoryName'}] },
                            { 'Table': 'Organisation', 'Fields': [{'Name': 'FullyQualifiedName', 'KnownAs': 'fullorganisationname'}] },
                            { 'Table': 'Admission', 'Type': 'left',
                              'Fields': [
                                  { 'Name': 'DateOfAdmission', 'FromMoreData': true },
                                  { 'Name': 'TimeOfAdmission', 'FromMoreData': true }
                              ]
                            },
                            { 'Table': 'Request', 'Type': 'left',
                              'Fields': [
                                  { 'Name': 'RequestId', 'KnownAs': 'requestreference' },
                                  { 'Name': 'RequestDate', 'FromMoreData': true },
                                  { 'Name': 'RequestTime', 'FromMoreData': true },
                                  { 'Name': 'RequestingClinician', 'FromMoreData': true },
                                  { 'Name': 'CurrentWeight', 'FromMoreData': true },
                                  { 'Name': 'CurrentWeightDate', 'FromMoreData': true },
                                  { 'Name': 'AntibioticStartDate', 'FromMoreData': true },
                                  { 'Name': 'AntibioticStartTime', 'FromMoreData': true }
                              ],
                              'ListItems': 'Ward, Cot, Urgency, Indication, RecentSurgery, CentralLineType, RespiratorySupport, AntibioticTiming, AntibioticAgents'
                            }
                        ],
                        'ListItems': 'PatientLocation, SpecimenType, SpecimenSite, ReceivedCondition, SpecimenAppearance, Diagnosis, State, AntibioticsInLast24hrs, TempInLast24hrs, ApprovalCommentL1, ApprovalCommentL2, SelectReason, TestCategory, CultureTypeCategory, BodySide, CollectionMethod, CollectedBy, SpecimenLabelled, VolumeMethod, BottleType',
                        'MultiSelectItems': 'TestCategory, CultureTypeCategory, AntibioticAgents',
                        'Where': [
                            {'Field': 'Id', 'Comparison': '=' }
                        ],
                        'Tags': 'SP'
                    }";
    }
}


