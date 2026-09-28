import { array } from "prop-types";

const AddValuesIntoArrayList = (valueList, valuesToChange) => {

    let returnList = Array.isArray(valueList) ? [...valueList] : [];

    if (Array.isArray(valuesToChange)) {
        for (const item of valuesToChange) {
            //const itemToChange = returnList.findIndex((x) => x.key === item.key);
            const itemToChange = returnList.findIndex((x) => x.Key === item.key);
            if (itemToChange === -1) {
                returnList.push(item.value);
            } else {
                returnList[itemToChange] = item.value;
            }
        }
    }

    return returnList;
}

export default AddValuesIntoArrayList;