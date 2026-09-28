using System.Collections.Generic;

namespace arc.common.Models.Specimen
{
    /// <summary>
    /// Model for setting or adding specimen tags. Used by SetSpecimenTagsCommand and AddSpecimenTagsCommand.
    /// </summary>
    public class SetSpecimenTagsModel
    {
        /// <summary>Specimen the tags belong to.</summary>
        public int SpecimenId { get; set; }

        /// <summary>List item ids of the tags to apply.</summary>
        public IEnumerable<int> ListItemIds { get; set; }
    }
}
