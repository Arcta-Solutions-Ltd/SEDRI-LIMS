namespace arc.data.model.Configuration
{
    /// <summary>
    /// Represents the fields in the language table in the database.
    /// </summary>
    public class LanguageDataModel : IdAndDateBase
    {
        public int TranslationId { get; set; }
        public int SourceId { get; set; }
        public string? Pack { get; set; }
    }
}
