import Post from "../../../Data/Post";

const runTestPatternQuery = (cultureId, testPatternId, testPatternRetrieved, errorWhenRetrievingData) => {

    const parameters = [{ Key: 'cultureid', Value: cultureId },
                        { Key: 'testpatternid', Value: testPatternId }];

    const criteria = { Name: "TestPatternWithBreakpoints", Parameters: parameters };
    Post('query/filteredget', criteria, testPatternRetrieved, errorWhenRetrievingData);
}

const runBreakpointsQuery = (cultureId, antibioticId, testMethodId, dosage, guidelinesId, handle, breakpointsRetrieved, errorWhenRetrievingData) => {

    const parameters = [{ Key: 'cultureid', Value: cultureId },
                        { Key: 'antibioticid', Value: antibioticId },
                        { Key: 'testmethodid', Value: testMethodId },
                        { Key: 'guidelinesid', Value: guidelinesId },
                        { Key: 'dosage', Value: dosage },
                        { Key: 'handle', Value: handle }];

    const criteria = { Name: "BreakpointsForASTRow", Parameters: parameters };
    Post('query/filteredget', criteria, breakpointsRetrieved, errorWhenRetrievingData);
}

export { runTestPatternQuery, runBreakpointsQuery };
