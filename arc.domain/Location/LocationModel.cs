namespace arc.domain.Location
{
    public class LocationModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Code { get; set; }
        public int ParentLocationId { get; set; }
        public float Latitude { get; set; }
        public float Longitude { get; set; }
        public string FullyQualifiedName { get; set; }
        public string LocationCodeHierarchy { get; set; }
        public string MoreData { get; set; }
        public string Enabled { get; set; }
    }
}
