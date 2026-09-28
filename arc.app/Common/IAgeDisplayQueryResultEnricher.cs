namespace arc.app.Common
{
    /// <summary>
    /// Enriches mapped record view JSON with calculated patient age display values.
    /// </summary>
    public interface IAgeDisplayQueryResultEnricher
    {
        /// <summary>
        /// Sets the patient age display field on a mapped patient record view result.
        /// </summary>
        /// <param name="mappedJson">The mapped JSON from patientviewmapper.</param>
        /// <param name="rawQueryJson">The raw query result JSON containing DateOfBirth.</param>
        /// <returns>The enriched mapped JSON.</returns>
        string EnrichPatientViewAge(string mappedJson, string rawQueryJson);

        /// <summary>
        /// Sets the patient age at specimen field on a mapped specimen record view result.
        /// </summary>
        /// <param name="mappedJson">The mapped JSON from specimenviewmapper.</param>
        /// <param name="rawQueryJson">The raw query result JSON containing DateOfBirth and collection dates.</param>
        /// <returns>The enriched mapped JSON.</returns>
        string EnrichSpecimenViewAge(string mappedJson, string rawQueryJson);
    }
}
