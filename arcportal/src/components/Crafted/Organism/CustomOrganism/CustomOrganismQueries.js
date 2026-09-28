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
    const parameters = [{ Key: 'familyid', Value: familyid }];
    const criteria = { Name: "GenusList", Parameters: parameters };
    Post('query/filteredget', criteria, genusRetrieved, errorWhenRetrievingData);
}

const runSpeciesListQuery = (genusid, speciesRetrieved, errorWhenRetrievingData) => {
    const parameters = [{ Key: 'genusid', Value: genusid }];
    const criteria = { Name: "SpeciesList", Parameters: parameters };
    Post('query/filteredget', criteria, speciesRetrieved, errorWhenRetrievingData);
}

export {runSpeciesListQuery, runGenusListQuery, runFamilyListQuery, runOrderListQuery };