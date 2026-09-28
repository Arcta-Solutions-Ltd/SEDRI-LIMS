const AddEmptyButtonArrays = (pageStructure) => {

    let returnStructure = [...pageStructure];

    for (const page of returnStructure) {
        page.buttons = [];
    }

    return returnStructure;
}

export default AddEmptyButtonArrays;