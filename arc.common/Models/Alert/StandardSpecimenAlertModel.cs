namespace arc.common.Models.Alert
{
    public class StandardSpecimenAlertModel
    {
        public string Type { get; set; }
        public string SpecimenTypes { get; set; }
        public string TestFormNames { get; set; }
        public bool NoAdditionalTest { get; set; }
        public string CultureQuantity { get; set; }
        public string CultureTypes { get; set; }
        public bool NoAdditionalCulture { get; set; }
        public bool NoCultures { get; set; }
        public bool NoTests { get; set; }
    }
}
