import TransformDatesInJson from "../../../Utils/Local/TransformDatesInJson";

const ManageRecordDates = (data) => {
    if (Array.isArray(data.Sections)) {
        for (let x = 0; x < data.Sections.length; x++) {
            TransformDatesInJson(data.Sections[x].Fields);
            if (Array.isArray(data.Sections[x].SubSections)) {
                for (let y = 0; y < data.Sections[x].SubSections.length; y++) {
                    TransformDatesInJson(data.Sections[x].SubSections[y].Fields);
                }
            }
        }
    }
}

export default ManageRecordDates;