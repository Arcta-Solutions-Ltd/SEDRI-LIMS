using System.Collections.Generic;

namespace arc.common.Models.Config
{
    public class RecordViewTargetModel
    {
        public List<RecordViewTargetSectionModel> Sections { get; set; } = new List<RecordViewTargetSectionModel>();

        public void Add(RecordViewTargetFieldModel target, int section, int subSection)
        {
            Sections[0].SubSections[2].Fields.Add(target);
        }

        public void Delete(string fieldName)
        {
            foreach(var section in Sections)
            {
                foreach(var subSection in section.SubSections)
                {
                    subSection.Fields.RemoveAll(f => f.Id.ToLower() == fieldName.ToLower());
                }
            }
        }
    }

    public class RecordViewTargetSectionModel
    {
        public string Id { get; set; }
        public string Title { get; set; }
        public List<RecordViewTargetFieldModel> Fields { get; set; } = new List<RecordViewTargetFieldModel>();
        public List<RecordViewTargetSubSectionModel> SubSections { get; set; } = new List<RecordViewTargetSubSectionModel>();
    }

    public class RecordViewTargetSubSectionModel
    {
        public string Id { get; set; }
        public string Title { get; set; }
        public List<RecordViewTargetFieldModel> Fields { get; set; } = new List<RecordViewTargetFieldModel>();
    }

    public class RecordViewTargetFieldModel {
        public string Id { get; set; }
        public string Label { get; set; }
        public string Value { get; set; }

    }
}
