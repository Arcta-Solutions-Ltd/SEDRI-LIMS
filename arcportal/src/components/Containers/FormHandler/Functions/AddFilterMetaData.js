const AddFilterMetaData = (filters, data) => {

    if (filters !== undefined) {
        for (const filter of filters) {
            let value = filter.values;
            if (Array.isArray(value)) {
                value = value.toString();
            }
            data["metaf" + filter.FieldName] = value;
        }
    }

    return data;
}

const AddMetaDataToParameters = (filters, parameters) => {
    if (filters !== undefined) {
        for (const filter of filters) {
            let value = filter.values;
            if (Array.isArray(value)) {
                value = value.toString();
            }
            parameters.push({ Key: "metaf" + filter.FieldName, Value: value});
        }
    }

    return parameters;
}

export default AddFilterMetaData;
export {AddMetaDataToParameters}