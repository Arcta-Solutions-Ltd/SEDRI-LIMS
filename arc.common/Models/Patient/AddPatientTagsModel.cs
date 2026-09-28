using System.Collections.Generic;

namespace arc.common.Models.Patient
{
    /// <summary>
    /// Model for adding tags to a patient. Used by AddPatientTagsCommand.
    /// </summary>
    public class AddPatientTagsModel
    {
        public int PatientId { get; set; }
        public IEnumerable<int> ListItemIds { get; set; }
    }
}
