import AddValuesIntoPageStructure from './AddValuesIntoPageStructure';

const AddJsonValuesIntoPageStructure = (pageStructure, jsonData, wipeIfNotFound) => {

    const valuesList = [];
    for(var i in jsonData){
        if (jsonData[i] !== undefined && jsonData[i] !== null) {
            valuesList.push({ key: i, value: jsonData[i]})
        }
    }

    return AddValuesIntoPageStructure(pageStructure, valuesList, wipeIfNotFound);
}

export default AddJsonValuesIntoPageStructure;