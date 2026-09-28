using arc.app.Common;
using arc.app.Specimen;
using arc.common;
using arc.common.ExtensionMethods;
using arc.common.Models;
using arc.common.Models.Instruments;
using arc.common.Utils;
using arc.data.model.Culture;
using arc.data.model.Instruments;
using arc.data.model.Specimen;
using arc.domain.Configuration.QueryFiltersConfig;
using System;
using System.Threading.Tasks;

namespace arc.app.Instruments
{
    /// <summary>
    /// Handles all results that come through the Instrument Controller and are therefore sent from an external system.
    /// Author: Arcta Solutions Limited
    /// </summary>
    public class InstrumentResultHandler(IInstrumentRepository instrumentRepository, IGeneralRepository generalRepository, ICultureRepository cultureRepository, ISingleInstrumentProfile singleInstrumentProfile,
        IMapType<ResponseModel, InstrumentResultsDataModel> responseModelToInstrumentResultsDataModelMapper, IRunEventHandler runEventHandler) : IInstrumentResultHandler
    {
        private readonly IMapType<ResponseModel, InstrumentResultsDataModel> _responseModelToInstrumentResultsDataModelMapper = responseModelToInstrumentResultsDataModelMapper;
        private readonly IInstrumentRepository _instrumentRepository = instrumentRepository;
        private readonly ISingleInstrumentProfile _singleInstrumentProfile = singleInstrumentProfile;
        private readonly ICultureRepository _cultureRepository = cultureRepository;
        private readonly IGeneralRepository _generalRepository = generalRepository;
        private readonly IRunEventHandler _runEventHandler = runEventHandler;
        public async Task<bool> HandleAsync(ResponseModel responseModel, TokenInfoModel token)
        {
            bool runEvent = false;

            responseModel.Event = "InstrumentResults";
            var instrumentProfile = await _singleInstrumentProfile.GetAsync(responseModel.ProfileName);

            if (instrumentProfile == null)
            {
                await RaiseInstrumentErrorAsync(responseModel.ProfileName, "@InsUpdA@", "No profile matching " + responseModel.ProfileName + " found");
                return false;
            }

            // Custom interface (InterfaceTypeId 10) loads an individual uploaded file by reverse-mapping it against the
            // linked export profile. It creates its own patient/specimen/culture hierarchy, so it bypasses the
            // instrumentresults culture-matching used by the AST/direct-test paths and dispatches straight to the
            // InstrumentResults event (which routes to CustomInstrumentProcessor). The processor records its own
            // instrument errors, so a successful dispatch returns Ok even when a record could not be loaded.
            if (string.Equals(
                    InstrumentProcessorTypeResolver.ResolveFactoryKey(instrumentProfile.InterfaceTypeId),
                    InstrumentProcessorTypeResolver.FactoryKeyCustomInstrument,
                    StringComparison.Ordinal))
            {
                var customContents = ArcJson.Serialize(responseModel);
                var customMessage = await _runEventHandler.RunAsync(customContents, token);
                if (!string.IsNullOrEmpty(customMessage))
                {
                    await RaiseInstrumentErrorAsync(responseModel.ProfileName, "@InsCusSave@", customMessage);
                    return false;
                }
                return true;
            }

            responseModel.OrganismList = instrumentProfile.OrganismGroupId.ToInt();
            responseModel.AntibioticList = instrumentProfile.AntibioticGroupId.ToInt();
            responseModel.AllowAstOverwrite = instrumentProfile.AllowAstOverwrite == "Yes";
            responseModel.AllowIdOverwrite = instrumentProfile.AllowIdOverwrite == "Yes";
            responseModel.IgnoreUnrecognisedAntibiotics = instrumentProfile.IgnoreUnrecognisedAntibiotics == "Yes";
            responseModel.DefaultGrowth = instrumentProfile.DefaultGrowth;

            var resultContents = ArcJson.Serialize(responseModel);

            string resolvedDirectTestName = null;
            if (!string.IsNullOrWhiteSpace(instrumentProfile.DirectTestId))
                resolvedDirectTestName = await _instrumentRepository.GetFirstConfigNameFromConfigIdListAsync(instrumentProfile.DirectTestId);

            int? machineId = responseModel.InstrumentMachineId;
            if (!machineId.HasValue && int.TryParse(instrumentProfile.InstrumentMachineId?.Trim(), out var profileMachineId) && profileMachineId > 0)
                machineId = profileMachineId;

            var isDirectProfile = !string.IsNullOrWhiteSpace(instrumentProfile.DirectTestId);

            var match = await _instrumentRepository.ResolveInboundInstrumentResultAsync(
                responseModel.InstrumentResultId,
                responseModel.ProfileName,
                responseModel.AccessionNumber?.Trim() ?? "",
                responseModel.CultureNumber?.Trim() ?? "",
                responseModel.CultureId,
                machineId,
                isDirectProfile,
                resolvedDirectTestName ?? "");

            if (match.IsAmbiguous)
            {
                await RaiseInstrumentErrorAsync(responseModel.ProfileName, "@InsUpdA@", "Multiple instrument results matched the inbound message.");
                return false;
            }

            var instrumentResultRecord = match.Row;

            if (responseModel.InstrumentResultId > 0 && instrumentResultRecord == null)
            {
                await RaiseInstrumentErrorAsync(responseModel.ProfileName, "@InsNoA@", resultContents);
                return false;
            }

            if (instrumentResultRecord != null && responseModel.InstrumentResultId > 0)
            {
                if (!string.Equals(instrumentResultRecord.InstrumentProfile, responseModel.ProfileName, StringComparison.OrdinalIgnoreCase))
                {
                    await RaiseInstrumentErrorAsync(responseModel.ProfileName, "@InsUpdA@", "InstrumentResultId does not match profile.");
                    return false;
                }

                if (machineId.HasValue && instrumentResultRecord.InstrumentMachineId.HasValue && instrumentResultRecord.InstrumentMachineId.Value != machineId.Value)
                {
                    await RaiseInstrumentErrorAsync(responseModel.ProfileName, "@InsUpdA@", "InstrumentResultId does not match instrument machine.");
                    return false;
                }
            }

            var instrumentResultData = _responseModelToInstrumentResultsDataModelMapper.Map(responseModel);

            int? resolvedInstrumentResultId = null;

            // If no matching result create a new one

            if (instrumentResultRecord == null)
            {
                var cultureRecord = new CultureDataModel();
                if (!string.IsNullOrEmpty(responseModel.AccessionNumber) && !string.IsNullOrEmpty(responseModel.CultureNumber))
                {
                    //Check culture exists

                    var queryFilters = new QueryFilterConfig();
                    queryFilters.AddString("AccessionNumber", responseModel.AccessionNumber);
                    queryFilters.AddString("CultureNumber", responseModel?.CultureNumber);

                    cultureRecord = await _cultureRepository.GetCultureByAccessionNumberAndCultureNumberAsync(queryFilters);

                    if (cultureRecord == null)
                    {
                        var specimenRecord = await _generalRepository.GetSingleRecordAsync(new SpecimenDataModel(), "culture", "ManufacturersBarcode", responseModel.Identifier);

                        var errorText = specimenRecord == null ? "@InsNoA@" : "@InsAccC@";
                        await RaiseInstrumentErrorAsync(responseModel.ProfileName, errorText, resultContents);
                    }

                }
                else
                {
                    if (!string.IsNullOrEmpty(responseModel.Identifier))
                    {
                        //Check whether a matching record for the barcode exists on the system
                        cultureRecord = await _generalRepository.GetSingleRecordAsync(new CultureDataModel(), "culture", "ManufacturersBarcode", responseModel.Identifier);
                    }
                }

                //If it does create a new one
                if (cultureRecord == null || cultureRecord.Id == 0)
                {
                    var errorRecord = new InstrumentErrorsDataModel
                    {
                        ProfileName = responseModel.ProfileName,
                        InstrumentDirectionId = (int)InstrumentDirectionEnum.Inbound,
                        ErrorText = "@InsNo@",
                        ErrorStatusId = 11,
                        Message = resultContents
                    };
                    await _generalRepository.AddAsync(errorRecord, "instrumenterrors");
                }
                else
                {
                    instrumentResultData.CultureId = cultureRecord.Id;
                    instrumentResultData.SpecimenId = cultureRecord.SpecimenId;
                    if (!string.IsNullOrWhiteSpace(responseModel.AccessionNumber))
                        instrumentResultData.AccessionNumber = responseModel.AccessionNumber.Trim();
                    else
                        instrumentResultData.AccessionNumber = await _instrumentRepository.GetAccessionNumberForSpecimenIdAsync(cultureRecord.SpecimenId);
                    var cultureNum = !string.IsNullOrWhiteSpace(responseModel.CultureNumber)
                        ? responseModel.CultureNumber.Trim()
                        : (cultureRecord.CultureNumber.HasValue ? cultureRecord.CultureNumber.Value.ToString() : null);
                    instrumentResultData.CultureNumber = string.IsNullOrWhiteSpace(cultureNum) ? null : cultureNum;
                    instrumentResultData.RawResult = resultContents;
                    instrumentResultData.StatusId = instrumentProfile.NeedsApproval == "Yes" ? 885 : 884;
                    var receivedUtcNew = DateTime.UtcNow;
                    instrumentResultData.ResultReceived = receivedUtcNew;
                    instrumentResultData.LastModifiedDate = receivedUtcNew;
                    var newId = await _generalRepository.AddAsync(instrumentResultData, "instrumentresults");
                    instrumentResultData.Id = newId;
                    resolvedInstrumentResultId = newId;
                    runEvent = true;
                }
            }
            else
            {
                var receivedUtc = DateTime.UtcNow;
                instrumentResultRecord.ResultReceived = receivedUtc;
                instrumentResultRecord.LastModifiedDate = receivedUtc;
                instrumentResultRecord.RawResult = resultContents;
                await _generalRepository.UpdateAsync(instrumentResultRecord, "instrumentresults", "id");
                instrumentResultData.Id = instrumentResultRecord.Id;
                resolvedInstrumentResultId = instrumentResultRecord.Id;
                runEvent |= true;
            }

            if (runEvent && instrumentProfile.NeedsApproval != "Yes")
            {
                var resultMessage = await _runEventHandler.RunAsync(resultContents, token);

                if (resultMessage == "" && resolvedInstrumentResultId.HasValue && responseModel.SourceFileAttachmentIds != null && responseModel.SourceFileAttachmentIds.Count > 0)
                {
                    await _instrumentRepository.AddInstrumentResultFileAttachmentsAsync(
                        resolvedInstrumentResultId.Value,
                        responseModel.SourceFileAttachmentIds);
                }

                if (string.IsNullOrEmpty(resultMessage))
                {
                    var updateModel = new { instrumentResultData.Id, StatusId = (int)InstrumentStatusEnum.Received };
                    await _generalRepository.UpdateAsync(updateModel, "instrumentresults", "id");
                }
                else
                {
                    await RaiseInstrumentErrorAsync(responseModel.ProfileName, "@InsUpdA@", resultMessage);
                }
            }

            return true;
        }

        /// <summary>
        /// Raises an instrument error asynchronously.
        /// </summary>
        /// <param name="profile">The profile name associated with the error.</param>
        /// <param name="errorText">The text describing the error.</param>
        /// <param name="message">Additional message related to the error.</param>
        /// <returns>A task that represents the asynchronous operation.</returns>
        private async Task RaiseInstrumentErrorAsync(string profile, string errorText, string message)
        {
            var errorRecord = new InstrumentErrorsDataModel
            {
                ProfileName = profile,
                InstrumentDirectionId = (int)InstrumentDirectionEnum.Inbound,
                ErrorText = errorText,
                ErrorStatusId = 11,
                Message = message
            };
            await _generalRepository.AddAsync(errorRecord, "instrumenterrors");
        }
    }
}
