using System.Collections.Generic;

namespace arc.common.Models.Coding
{
    public class EditSynonymModel
    {
        public int Id { get; set; }
        public string PreferredName { get; set; }
        public List<SynonymModel> SynonymGrid { get; set;}

    }

    public class SynonymModel
    {
        public string Synonym { get; set; }
    }
}
