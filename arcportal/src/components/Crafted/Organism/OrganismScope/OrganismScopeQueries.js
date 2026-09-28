import Post from "../../../../Data/Post";

const runOrderListQuery = (orderRetrieved, errorWhenRetrievingData) => {
    const criteria = { Name: "OrderList" };
    Post('query/filteredget', criteria, orderRetrieved, errorWhenRetrievingData);
}

const runFamilyListQuery = (orderid, familyRetrieved, errorWhenRetrievingData) => {
    const parameters = [{ Key: 'orderid', Value: orderid }];
    const criteria = { Name: "FamilyList", Parameters: parameters };
    Post('query/filteredget', criteria, familyRetrieved, errorWhenRetrievingData);
}

const runGenusListQuery = (familyid, genusRetrieved, errorWhenRetrievingData) => {
    const parameters = familyid === undefined ? [] : [{ Key: 'familyid', Value: familyid }];
    //const parameters = [{ Key: 'familyid', Value: familyid }];
    const criteria = { Name: "GenusList", Parameters: parameters };
    Post('query/filteredget', criteria, genusRetrieved, errorWhenRetrievingData);
}

const runSpeciesListQuery = (genusid, speciesRetrieved, errorWhenRetrievingData) => {
    const parameters = [{ Key: 'genusid', Value: genusid }];
    const criteria = { Name: "SpeciesList", Parameters: parameters };
    Post('query/filteredget', criteria, speciesRetrieved, errorWhenRetrievingData);
}

const runSubSpeciesListQuery = (speciesid, subspeciesRetrieved, errorWhenRetrievingData) => {
    const parameters = [{ Key: 'speciesid', Value: speciesid }];
    const criteria = { Name: "SubSpeciesList", Parameters: parameters};
    Post('query/filteredget', criteria, subspeciesRetrieved, errorWhenRetrievingData);
}

const runSerotypeListQuery = (speciesid, serotypeRetrieved, errorWhenRetrievingData) => {
    const parameters = [{ Key: 'speciesid', Value: speciesid }];
    const criteria = { Name: "SerotypeList", Parameters: parameters};
    Post('query/filteredget', criteria, serotypeRetrieved, errorWhenRetrievingData);
}

const runOrderAndFamilyFromGenusIdQuery = (genusid, orderAndFamilyRetrieved, errorWhenRetrievingData, currentFilter) => {
    const parameters = [{ Key: 'genusid', Value: genusid }];
    const criteria = { Name: "OrderAndFamilyFromGenusId", Parameters: parameters};
    Post('query/filteredget', criteria, orderAndFamilyRetrieved, errorWhenRetrievingData, currentFilter);
}

export {runSerotypeListQuery, runSubSpeciesListQuery, runSpeciesListQuery, runGenusListQuery, runFamilyListQuery, runOrderListQuery, runOrderAndFamilyFromGenusIdQuery };
