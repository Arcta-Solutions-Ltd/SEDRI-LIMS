using arc.app.Common;
using arc.app.Files;
using arc.app.Instruments;
using arc.app.Specimen;
using arc.app.Tests;
using arc.common.Models;
using arc.common.Models.Instruments;
using arc.common.Utils;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Text;
using System.Threading.Tasks;

namespace arc.app.Instruments.Processors;

/// <summary>
/// Loads instrument JSON from a linked file attachment and saves into a direct test using the same event/mapping path as the portal (e.g. <c>mtbpcrform</c>).
/// The <c>Tests.TestName</c> row is ensured using the instrument profile’s <c>DirectTestId</c> (resolved to <c>configs.configname</c>), not the file’s <c>TestType</c> string.
/// </summary>
internal class DirectTestInstrumentProcessor(IServiceProvider serviceProvider) : IRespond
{
    /// <summary>
    /// TestType value in the file that maps to the MTBPCR form pipeline.
    /// </summary>
    internal const string MtbPcrTestType = "MTBPCR";

    /// <inheritdoc />
    public async Task<bool> Run(ResponseModel response, TokenInfoModel token)
    {
        var logger = serviceProvider.GetRequiredService<ILoggerFactory>().CreateLogger(typeof(DirectTestInstrumentProcessor));
        if (token == null)
        {
            logger.LogError("DirectTestInstrumentProcessor requires TokenInfoModel for direct test save.");
            return false;
        }

        if (response.SourceFileAttachmentIds == null || response.SourceFileAttachmentIds.Count == 0)
        {
            logger.LogError("Direct test instrument load requires at least one SourceFileAttachmentIds entry.");
            return false;
        }

        try
        {
            var fileHandler = serviceProvider.GetRequiredService<IFileHandler>();
            var specimenRepository = serviceProvider.GetRequiredService<ISpecimenRepository>();
            var testRepository = serviceProvider.GetRequiredService<ITestRepository>();
            var listRepository = serviceProvider.GetRequiredService<IListRepository>();
            var runEventHandler = serviceProvider.GetRequiredService<IRunEventHandler>();

            var attachmentId = response.SourceFileAttachmentIds[0];
            logger.LogInformation(
                "DirectTestInstrumentProcessor: profile={ProfileName}, fileAttachmentId={AttachmentId}",
                response.ProfileName,
                attachmentId);

            var read = await fileHandler.ReadByIdAsync(attachmentId);
            var text = Encoding.UTF8.GetString(read.Bytes);
            var file = ArcJson.Deserialize<DirectTestInstrumentFileModel>(text);
            if (file == null || string.IsNullOrWhiteSpace(file.AccessionNumber))
            {
                logger.LogError("Instrument file JSON is missing or has no AccessionNumber.");
                return false;
            }

            if (string.IsNullOrWhiteSpace(file.TestType))
            {
                logger.LogError("Instrument file JSON has no TestType.");
                return false;
            }

            if (!string.Equals(file.TestType.Trim(), MtbPcrTestType, StringComparison.OrdinalIgnoreCase))
            {
                logger.LogWarning(
                    "DirectTestInstrumentProcessor: TestType {TestType} is not supported (only {Supported} is implemented).",
                    file.TestType,
                    MtbPcrTestType);
                return false;
            }

            if (string.IsNullOrWhiteSpace(response.ProfileName))
            {
                logger.LogError("DirectTestInstrumentProcessor requires ResponseModel.ProfileName to resolve the direct test name from the instrument profile.");
                return false;
            }

            var singleInstrumentProfile = serviceProvider.GetRequiredService<ISingleInstrumentProfile>();
            var instrumentRepository = serviceProvider.GetRequiredService<IInstrumentRepository>();
            var profile = await singleInstrumentProfile.GetAsync(response.ProfileName.Trim());
            if (profile == null)
            {
                logger.LogError("No instrument profile found for ProfileName {ProfileName}.", response.ProfileName);
                return false;
            }

            if (string.IsNullOrWhiteSpace(profile.DirectTestId))
            {
                logger.LogError(
                    "DirectTestInstrumentProcessor: profile {ProfileName} has no DirectTestId; set it to the direct test form config id(s) (same as manual instrument request).",
                    response.ProfileName);
                return false;
            }

            var testName = await instrumentRepository.GetFirstConfigNameFromConfigIdListAsync(profile.DirectTestId);
            if (string.IsNullOrWhiteSpace(testName))
            {
                logger.LogError(
                    "DirectTestInstrumentProcessor: could not resolve direct test name from DirectTestId for profile {ProfileName} (DirectTestId='{DirectTestId}'). Ensure the first id in the list exists in configs.",
                    response.ProfileName,
                    profile.DirectTestId);
                return false;
            }

            var specimenIdStr = await specimenRepository.GetSpecimenIdByAccessionNumberAsync(file.AccessionNumber.Trim());
            if (string.IsNullOrEmpty(specimenIdStr) || !int.TryParse(specimenIdStr, out var specimenId) || specimenId <= 0)
            {
                logger.LogError("No specimen found for AccessionNumber {AccessionNumber}.", file.AccessionNumber);
                return false;
            }

            logger.LogInformation("DirectTestInstrumentProcessor: resolved specimenId={SpecimenId}", specimenId);

            var testId = await testRepository.EnsureDirectTestExistsAsync(specimenId, testName);
            logger.LogInformation("DirectTestInstrumentProcessor: Tests.Id={TestId} for TestName={TestName}", testId, testName);

            var payload = await MtbPcrDirectTestInstrumentMapper.BuildMtbPcrSavePayloadAsync(
                file,
                testId,
                listRepository,
                logger);

            var message = payload.ToString(Formatting.None);
            logger.LogDebug("DirectTestInstrumentProcessor: mtbpcrform payload: {Payload}", message);

            var validationMessage = await runEventHandler.RunAsync(message, token);
            if (!string.IsNullOrEmpty(validationMessage))
            {
                logger.LogError("Direct test save failed validation: {Message}", validationMessage);
                return false;
            }

            logger.LogInformation("DirectTestInstrumentProcessor: mtbpcrform completed for Tests.Id={TestId}", testId);
            return true;
        }
        catch (Exception ex)
        {
            var log = serviceProvider.GetRequiredService<ILoggerFactory>().CreateLogger(typeof(DirectTestInstrumentProcessor));
            log.LogError(ex, "DirectTestInstrumentProcessor failed.");
            return false;
        }
    }
}
