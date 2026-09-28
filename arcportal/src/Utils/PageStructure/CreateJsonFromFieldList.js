const CreatejsonFromFieldList = (fieldList) => {

    let json = "{"
    for (const field of fieldList) {
        const newField = field.key + ":'" + field.value + "'";
        json += json.length === 1 ? newField : "," + newField;
    }
    json += "}";
    return json;
}

export default CreatejsonFromFieldList;