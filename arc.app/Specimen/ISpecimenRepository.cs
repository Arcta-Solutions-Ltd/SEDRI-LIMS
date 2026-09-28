using arc.common;
using arc.common.Models;
using arc.common.Models.Config;
using arc.common.Models.Home;
using arc.common.Models.Laboratory;
using arc.common.Models.Specimen;
using arc.domain.Configuration.QueryFiltersConfig;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace arc.app.Specimen;

/// <summary>
/// Defines methods for creating, retrieving, updating, and managing specimen data and related operations.
/// </summary>
public interface ISpecimenRepository
{
    /// <summary>
    /// Asynchronously creates a new specimen record.
    /// </summary>
    /// <param name="data">The event model containing the specimen creation data.</param>
    /// <param name="specimenAlreadyReceived">Indicates whether the specimen has already been received.</param>
    /// <param name="command">The event command associated with the specimen creation.</param>
    /// <param name="token">The token containing authentication and context information.</param>
    /// <param name="tableExceptions">A list of table exceptions for processing specific configurations.</param>
    /// <param name="json">A JSON string representing additional specimen data.</param>
    /// <param name="accessionNumber">The accession number associated with the specimen.</param>
    /// <param name="patientref">The patient reference associated with the specimen.</param>
    /// <param name="laboratoryConfiguration">The laboratory configuration containing culture test settings.</param>
    /// <returns>
    /// A task representing the asynchronous operation that returns an integer, typically indicating the specimen's identifier or status.
    /// </returns>
    Task<int> CreateSpecimenAsync(
        CreateSpecimenEventModel data,
        bool specimenAlreadyReceived,
        EventModel command,
        TokenInfoModel token,
        List<TableExceptionModel> tableExceptions,
        string json,
        string accessionNumber,
        string patientref,
        LaboratoryConfigurationListModel laboratoryConfiguration);

    /// <summary>
    /// Asynchronously retrieves a single specimen patient model by specimen identifier.
    /// </summary>
    /// <param name="id">The unique identifier of the specimen.</param>
    /// <returns>
    /// A task representing the asynchronous operation that returns a <see cref="SpecimenPatientModel"/>.
    /// </returns>
    Task<SpecimenPatientModel> GetSingleAsync(int id);

    /// <summary>
    /// Asynchronously retrieves a single specimen patient model including associated comments by specimen identifier.
    /// </summary>
    /// <param name="id">The unique identifier of the specimen.</param>
    /// <returns>
    /// A task representing the asynchronous operation that returns a <see cref="SpecimenPatientModel"/>.
    /// </returns>
    Task<SpecimenPatientModel> GetSingleCommentAsync(int id);
    Task<SpecimenPatientModel> GetSingleSpecimenTagAsync(int id);

    /// <summary>
    /// Replaces all specimen tags with the given list of ListItemIds.
    /// Deletes existing tags for the specimen, then inserts the new set.
    /// Returns the last inserted SpecimenTag id, or 0 if none.
    /// </summary>
    Task<int> SetSpecimenTagsAsync(int specimenId, IEnumerable<int> listItemIds);

    /// <summary>
    /// Adds the given tag IDs to the specimen, merging with existing tags.
    /// Does not add duplicates if a tag already exists for the specimen.
    /// Returns the last inserted SpecimenTag id, or 0 if none were added.
    /// </summary>
    Task<int> AddSpecimenTagsAsync(int specimenId, IEnumerable<int> listItemIds);

    /// <summary>
    /// Replaces all specimen file attachments with the given list of file attachment IDs.
    /// Deletes existing links for the specimen, then inserts the new set.
    /// Returns the last inserted specimenfileattachments id, or 0 if none.
    /// </summary>
    Task<int> SetSpecimenFileAttachmentsAsync(int specimenId, IEnumerable<int> fileAttachmentIds);

    /// <summary>
    /// Asynchronously retrieves a single specimen patient model from direct test data using the specimen identifier.
    /// </summary>
    /// <param name="id">The unique identifier of the specimen.</param>
    /// <returns>
    /// A task representing the asynchronous operation that returns a <see cref="SpecimenPatientModel"/>.
    /// </returns>
    Task<SpecimenPatientModel> GetSingleFromDirectTestsAsync(int id);

    /// <summary>
    /// Asynchronously retrieves the specimen type identifier for a given specimen.
    /// </summary>
    /// <param name="id">The unique identifier of the specimen.</param>
    /// <returns>
    /// A task representing the asynchronous operation that returns an integer representing the specimen type.
    /// </returns>
    Task<int> GetSpecimenTypeAsync(int id);

    /// <summary>
    /// Asynchronously retrieves the specimen type identifier from culture test data for a given specimen.
    /// </summary>
    /// <param name="id">The unique identifier of the specimen.</param>
    /// <returns>
    /// A task representing the asynchronous operation that returns an integer representing the specimen type from culture data.
    /// </returns>
    Task<int> GetSpecimenTypeFromCultureAsync(int id);

    /// <summary>
    /// Asynchronously retrieves the collection date of the specimen.
    /// </summary>
    /// <param name="id">The unique identifier of the specimen.</param>
    /// <returns>
    /// A task representing the asynchronous operation that returns the <see cref="DateTime"/> when the specimen was collected.
    /// </returns>
    Task<DateTime> GetCollectionDateAsync(int id);

    /// <summary>
    /// Asynchronously retrieves a batch list of specimens based on the specified query filters and token information.
    /// </summary>
    /// <param name="queryFilters">The configuration that defines the query filters for the specimen batch list.</param>
    /// <param name="token">The token containing authentication and context details.</param>
    /// <returns>
    /// A task representing the asynchronous operation that returns an enumerable of <see cref="SpecimenBatchListModel"/>.
    /// </returns>
    Task<IEnumerable<SpecimenBatchListModel>> SpecimenBatchListAsync(QueryFilterConfig queryFilters, TokenInfoModel token);

    /// <summary>
    /// Asynchronously retrieves a single specimen patient model from culture test data using the specimen identifier.
    /// </summary>
    /// <param name="id">The unique identifier of the specimen.</param>
    /// <returns>
    /// A task representing the asynchronous operation that returns a <see cref="SpecimenPatientModel"/>.
    /// </returns>
    Task<SpecimenPatientModel> GetSingleFromCultureTestsAsync(int id);

    /// <summary>
    /// Asynchronously retrieves a single culture specimen patient model using the specimen identifier.
    /// </summary>
    /// <param name="id">The unique identifier of the culture specimen.</param>
    /// <returns>
    /// A task representing the asynchronous operation that returns a <see cref="SpecimenPatientModel"/>.
    /// </returns>
    Task<SpecimenPatientModel> GetCultureSingleAsync(int id);

    /// <summary>
    /// Asynchronously retrieves the count of specimens grouped by type based on the specified query filters.
    /// </summary>
    /// <param name="queryFilters">The query filters used to group and count specimen types.</param>
    /// <returns>
    /// A task representing the asynchronous operation that returns a list of <see cref="SpecimenCountModel"/>.
    /// </returns>
    Task<List<SpecimenCountModel>> SpecimenTypeCountAsync(QueryFilterConfig queryFilters);

    /// <summary>
    /// Asynchronously retrieves the count of specimens grouped by state based on the specified query filters.
    /// </summary>
    /// <param name="queryFilters">The query filters used to group and count specimen states.</param>
    /// <returns>
    /// A task representing the asynchronous operation that returns a list of <see cref="SpecimenCountModel"/>.
    /// </returns>
    Task<List<SpecimenCountModel>> SpecimenStateCountAsync(QueryFilterConfig queryFilters);

    /// <summary>
    /// Asynchronously retrieves the count of specimens grouped by tag based on the specified query filters.
    /// </summary>
    /// <param name="queryFilters">The query filters used to group and count specimen tags.</param>
    /// <returns>
    /// A task representing the asynchronous operation that returns a list of <see cref="SpecimenCountModel"/>.
    /// </returns>
    Task<List<SpecimenCountModel>> SpecimenTagCountAsync(QueryFilterConfig queryFilters);

    /// <summary>
    /// Asynchronously retrieves the approval details for a specimen based on the specified query filters.
    /// </summary>
    /// <param name="queryFilters">The query filters used to identify and retrieve the specimen approval information.</param>
    /// <returns>
    /// A task representing the asynchronous operation that returns a <see cref="SpecimenApprovalModel"/>.
    /// </returns>
    Task<SpecimenApprovalModel> SpecimenApprovalAsync(QueryFilterConfig queryFilters);

    /// <summary>
    /// Asynchronously retrieves a specimen model based on the specified query filters.
    /// </summary>
    /// <param name="queryFilters">The query filters used to identify the specimen.</param>
    /// <returns>
    /// A task representing the asynchronous operation that returns a <see cref="SpecimenModel"/>.
    /// </returns>
    Task<SpecimenModel> SpecimenByIdAsync(QueryFilterConfig queryFilters);

    /// <summary>
    /// Asynchronously acknowledges the receipt of a specimen by processing the associated event data and culture test configurations.
    /// </summary>
    /// <param name="dataToSave">The event model containing data to be saved for the receipt acknowledgment.</param>
    /// <param name="cultureTestConfig">A list of culture test configuration models associated with the specimen receipt.</param>
    /// <param name="json">A JSON string representing additional specimen data.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    Task AcknowledgeReceiptCommandAsync(ACKReceiptEventModel dataToSave, List<CultureTestConfigModel> cultureTestConfig, string json);

    /// <summary>
    /// Asynchronously edits an existing specimen record with the provided update data, configuration name, and new state identifier.
    /// </summary>
    /// <param name="dataToSave">A string representing the data needed to update the specimen.</param>
    /// <param name="configName">The name of the configuration to be updated.</param>
    /// <param name="newStateId">The new state identifier for the specimen.</param>
    /// <returns>A task representing the asynchronous update operation.</returns>
    Task EditSpecimenAsync(string dataToSave, string configName, string newStateId);

    /// <summary>
    /// Updates a specimen and merges MoreData on related patient, admission and request entities
    /// according to page TableName routing.
    /// </summary>
    /// <param name="dataToSave">Serialised edit payload.</param>
    /// <param name="tableExceptions">Field routing from form page configuration.</param>
    /// <param name="newStateId">Target workflow state id.</param>
    Task EditSpecimenWithRelatedEntitiesAsync(string dataToSave, List<TableExceptionModel> tableExceptions, string newStateId);

    /// <summary>
    /// Asynchronously retrieves the specimen identifier corresponding to the specified accession number.
    /// </summary>
    /// <param name="accessionNumber">The accession number associated with the specimen.</param>
    /// <returns>
    /// A task representing the asynchronous operation that returns the specimen identifier as a string.
    /// </returns>
    Task<string> GetSpecimenIdByAccessionNumberAsync(string accessionNumber);

    /// <summary>
    /// Asynchronously moves a specimen based on the provided data.
    /// </summary>
    /// <param name="dataToSave">A string representing the data that defines the move operation for the specimen.</param>
    /// <returns>
    /// A task representing the asynchronous operation that returns an integer, typically indicating a status or identifier.
    /// </returns>
    Task<int> MoveSpecimenAsync(string dataToSave);

    /// <summary>
    /// Gets the finalised timestamp (LastModifiedDate) for each specimen from SpecimenStateHistory
    /// where StateId=534 (Specimen Finalised). Returns the most recent row per specimen.
    /// </summary>
    Task<Dictionary<int, DateTime?>> GetSpecimenFinalisedDatesAsync(IEnumerable<int> specimenIds);

    /// <summary>
    /// For specimens whose current state is terminal (finalised, rejected, cancelled), returns the
    /// LastModifiedDate of the most recent SpecimenStateHistory row for that state.
    /// Used for archive list TAT end time.
    /// </summary>
    Task<Dictionary<int, DateTime?>> GetSpecimenTerminalStateEndTimesAsync(IEnumerable<int> specimenIds);

    /// <summary>
    /// Returns recent queue-derived activity for the home dashboard Recently Used tile (filtered by token scope).
    /// </summary>
    Task<List<HomeDashboardRecentItemModel>> HomeDashboardRecentlyUsedAsync(QueryFilterConfig queryFilters);

    /// <summary>
    /// Returns TAT compliance KPI aggregates for the home dashboard (primary window from dashboard timerange; optional rolling average).
    /// </summary>
    Task<TatComplianceKpiModel> HomeDashboardTatComplianceAsync(QueryFilterConfig queryFilters);
}
