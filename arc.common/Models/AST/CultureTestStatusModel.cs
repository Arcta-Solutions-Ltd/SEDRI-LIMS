namespace arc.common.Models.AST
{
    /// <summary>
    /// Represents the status of a culture test for display on the AST screen.
    /// Completion is derived from Status === "Complete" in the culturetests table.
    /// </summary>
    public class CultureTestStatusModel
    {
        /// <summary>
        /// The culturetest record ID. Used when opening the isolate test form from AST so the form loads and saves correctly.
        /// </summary>
        public int? Id { get; set; }
        public string TestName { get; set; }
        public string Status { get; set; }
    }
}
