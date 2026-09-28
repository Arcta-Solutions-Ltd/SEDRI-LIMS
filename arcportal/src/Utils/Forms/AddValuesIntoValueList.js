const AddValuesIntoValueList = (valueList, valuesToChange) => {

    let returnList = {...valueList};
   
    for (const item of valuesToChange) {
        const asLowercase = item.key.toLowerCase();
        const property = Object.keys(returnList).find(key => key.toLowerCase() === asLowercase);
        if (property === undefined) {
            returnList[item.key] = newValue(item.value);
        } else {
            returnList[property] = newValue(item.value);
        }
    }

    return returnList;
}

const newValue = (value) => {
    if (value !== undefined && value !== null) {
        return value.Key === undefined ? value : value.value;
    } else {
        return value;
    }
}

export default AddValuesIntoValueList;