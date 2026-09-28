const SubstituteListValues = (dataToChange, lists) => {

    const returndata = {};

    for (const key of Object.keys(dataToChange)) {
        let found = false;
        const tempKey = key.toLowerCase();
        const idPos = tempKey.lastIndexOf("id");
        if (idPos > 0 && idPos + 2 === tempKey.length) {
            const listName = tempKey.substring(0,tempKey.length - 2)
            const list = lists.filter((list) => list.Name === listName);
            if (list !== undefined) {
                const newValue = list[0].Options.filter((opt) => opt.Key === dataToChange[key].toString());
                if (newValue.length > 0) {
                    returndata[listName] = newValue[0].Text
                    found = true;
                }
            }
        }

        if (! found) {
            returndata[key] = dataToChange[key];
        }
    }

    return returndata;
}

export default SubstituteListValues;


