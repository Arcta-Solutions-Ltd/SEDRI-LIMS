using System;

namespace arc.data.Configuration
{
    public class ConfigDataModel
    {
        public short Id { get; set; }
        public string Language { get; set; }
        public string MultipleUserFlag { get; set; }
        public string MultipleLaboratoryFlag { get; set; }
        public DateTime LastModifiedDate { get; set; }
    }
}
