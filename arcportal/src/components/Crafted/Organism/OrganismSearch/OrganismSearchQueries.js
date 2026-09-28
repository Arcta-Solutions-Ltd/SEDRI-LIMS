import Post from "../../../../Data/Post";

const runGenusListQuery = (genusRetrieved, errorWhenRetrievingData) => {
    const criteria = { Name: "GenusList" };
    Post('query/filteredget', criteria, genusRetrieved, errorWhenRetrievingData);
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

const runSpeciesListQuery = (genusid, speciesRetrieved, errorWhenRetrievingData) => {
    const parameters = [{ Key: 'genusid', Value: genusid }];
    const criteria = { Name: "SpeciesList", Parameters: parameters};
    Post('query/filteredget', criteria, speciesRetrieved, errorWhenRetrievingData);
}

export {runSubSpeciesListQuery, runSerotypeListQuery, runSpeciesListQuery, runGenusListQuery};