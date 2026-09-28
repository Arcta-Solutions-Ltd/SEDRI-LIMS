using arc.common;
using arc.common.Models.AST;
using arc.common.Models.Instruments;
using arc.common.Models.Laboratory;
using arc.common.Models.Specimen;
using arc.data.model.Culture;
using arc.domain.Configuration.EventsConfig;
using arc.domain.Configuration.QueryFiltersConfig;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace arc.app.Specimen;

public interface ICultureRepository
{
    /// <summary>
    /// Asynchronously retrieves AST culture data based on the specified identifier.
    /// </summary>
    /// <param name="id">The unique identifier for the AST culture data.</param>
    /// <returns>
    /// A task representing the asynchronous operation that returns an instance of <see cref="ASTCultureModel"/>.
    /// </returns>
    Task<ASTCultureModel> GetASTCultureDataAsync(string id);

    /// <summary>
    /// Asynchronously retrieves the specimen patient data for the specified specimen identifier.
    /// </summary>
    /// <param name="id">The unique identifier for the specimen patient.</param>
    /// <returns>
    /// A task representing the asynchronous operation that returns a <see cref="SpecimenPatientModel"/>.
    /// </returns>
    Task<SpecimenPatientModel> GetSingleAsync(int id);

    /// <summary>
    /// Asynchronously updates the culture result using the provided blood culture results model.
    /// </summary>
    /// <param name="result">The blood culture results model containing the updated data.</param>
    /// <returns>
    /// A task representing the asynchronous operation that returns the number of records updated.
    /// </returns>
    Task<int> UpdateCultureResultAsync(BloodCultureResultsModel result);

    /// <summary>
    /// Asynchronously retrieves a list of culture records for a given specimen using the provided query filters.
    /// </summary>
    /// <param name="queryFilters">The query filter configuration for filtering culture records.</param>
    /// <returns>
    /// A task representing the asynchronous operation that returns a list of <see cref="CultureListModel"/> instances.
    /// </returns>
    Task<List<CultureListModel>> GetCultureListBySpecimenIdAsync(QueryFilterConfig queryFilters);

    /// <summary>
    /// Asynchronously retrieves a detailed view model for a culture record by its identifier using the provided query filters.
    /// </summary>
    /// <param name="queryFilters">The query filter configuration for identifying the culture record.</param>
    /// <returns>
    /// A task representing the asynchronous operation that returns a <see cref="CultureViewModel"/> instance.
    /// </returns>
    Task<CultureViewModel> GetCultureViewByIdAsync(QueryFilterConfig queryFilters);

    /// <summary>
    /// Asynchronously retrieves a culture record as a <see cref="CultureModel"/> using the provided query filters.
    /// </summary>
    /// <param name="queryFilters">The query filter configuration for identifying the culture record.</param>
    /// <returns>
    /// A task representing the asynchronous operation that returns a <see cref="CultureModel"/> instance.
    /// </returns>
    Task<CultureModel> GetCultureByIdAsync(QueryFilterConfig queryFilters);

    /// <summary>
    /// Asynchronously adds a new culture record with the specified data, event information, test configuration, and user context.
    /// </summary>
    /// <param name="dataToSave">A string representation of the culture data to be saved.</param>
    /// <param name="command">The event model representing the command that triggered this operation.</param>
    /// <param name="eventData">The event configuration details associated with the new culture record.</param>
    /// <param name="cultureTestConfig">A list of culture test configuration models pertinent to the new record.</param>
    /// <param name="username">The username of the operator performing the add operation.</param>
    /// <returns>
    /// A task representing the asynchronous operation that returns the identifier of the newly added culture record.
    /// </returns>
    Task<int> AddCultureAsync(string dataToSave, EventModel command, EventConfig eventData, LaboratoryConfigurationListModel cultureTestConfig, string username);

    /// <summary>
    /// Executes the culture editing workflow by initializing an <see cref="EditCultureCommand"/> 
    /// and invoking its asynchronous execution logic.
    /// </summary>
    /// <param name="dataToSave">Serialized culture data to persist.</param>
    /// <param name="command">The event model containing context for the edit operation.</param>
    /// <param name="eventData">Configuration details specific to the current event.</param>
    /// <returns>
    /// A task that resolves to an integer status code indicating the result of the operation.
    /// </returns>
    Task<int> EditCultureAsync(string dataToSave, EventModel command, EventConfig eventData, string id);

    /// <summary>
    /// Asynchronously performs growth operations on all culture records associated with a specimen.
    /// </summary>
    /// <param name="cultureList">A list of culture records related to the specimen.</param>
    /// <param name="specimenQuantityId">The identifier representing the specimen quantity context.</param>
    /// <param name="newStateId">The identifier of the new state to transition the cultures into.</param>
    /// <param name="specimenId">The unique identifier for the specimen containing the cultures.</param>
    /// <returns>
    /// A task representing the asynchronous growth operation for all cultures in the specimen.
    /// </returns>
    Task GrowthForAllCulturesInSpecimenAsync(List<CultureListModel> cultureList, int specimenQuantityId, int newStateId, int specimenId);

    /// <summary>
    /// Asynchronously deletes the culture record identified by the specified identifier.
    /// </summary>
    /// <param name="id">The unique identifier of the culture record to delete.</param>
    /// <returns>
    /// A task representing the asynchronous delete operation.
    /// </returns>
    Task DeleteCultureAsync(string id);

    /// <summary>
    /// Asynchronously deletes the isolate record identified by the specified identifier.
    /// </summary>
    /// <param name="id">The unique identifier of the culture record to delete.</param>
    /// <returns>
    /// A task representing the asynchronous delete operation.
    /// </returns>
    Task DeleteIsolateAsync(string id);

    /// <summary>
    /// Asynchronously retrieves a culture record by its barcode using the provided query filters.
    /// </summary>
    /// <param name="queryFilters">The query filter configuration for identifying the culture record via barcode.</param>
    /// <returns>
    /// A task representing the asynchronous operation that returns a <see cref="CultureModel"/> instance.
    /// </returns>
    Task<CultureModel> GetCultureByBarcodeAsync(QueryFilterConfig queryFilters);

    /// <summary>
    /// Retrieves the culture by accession number and culture number asynchronously.
    /// </summary>
    /// <param name="queryFilters">The QueryFilterConfig object containing query filters.</param>
    /// <returns>A task that represents the asynchronous operation. 
    /// The task result contains the CultureModel object.</returns>
    Task<CultureDataModel> GetCultureByAccessionNumberAndCultureNumberAsync(QueryFilterConfig queryFilters);

    /// <summary>
    /// Replaces all culture file attachments with the given list of file attachment IDs.
    /// Deletes existing links for the culture, then inserts the new set.
    /// Returns the last inserted culturefileattachments id, or 0 if none.
    /// </summary>
    Task<int> SetCultureFileAttachmentsAsync(int cultureId, IEnumerable<int> fileAttachmentIds);

    /// <summary>
    /// Inserts a new culture row for the specimen and culture type (next culture number; does not enqueue instrument triggers).
    /// Multiple cultures of the same type per specimen are allowed.
    /// </summary>
    Task<int> CreateCultureForSpecimenAndTypeAsync(int specimenId, int typeId);
}

