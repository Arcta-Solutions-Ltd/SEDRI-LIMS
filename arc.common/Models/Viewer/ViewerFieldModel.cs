namespace arc.common.Models.Viewer
{
    public class ViewerFieldModel
    {
        public string Id { get; set; }
        public string Label { get; set; }
        public string Value { get; set; }

        public ViewerFieldModel Copy()
        {
            ViewerFieldModel Replica = new ViewerFieldModel { Id = Id, Label = Label, Value = Value };
            return Replica;
        }
    }
}
