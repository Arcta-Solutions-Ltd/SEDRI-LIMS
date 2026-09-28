import Get from '../../../../Data/Get';
import Post from '../../../../Data/Post';

const RefreshFilterList = (filters, searchText, orderBy, orderDescending, searchFields, queryname, dataReceivedHandler, errorHandler, startDate, endDate) => {
    var parameters = [];
    if (filters !== undefined && filters !== null) {
        for (var filter of filters) {
            if (filter.values !== undefined && filter.values !== null && filter.values.length > 0 && !filter.values.every(val => val == '')) {
                parameters.push({ Key: filter.FieldName, Value: filter.values.toString() });
            }
        }
    }

    if (searchText === undefined) {
        searchText = "";
    }

    if  (startDate !== undefined && startDate !== null) {
        var startDateValue = startDate.getFullYear() + "-" + (startDate.getMonth() + 1) + "-" + startDate.getDate();
        parameters.push({Key: "startdate", Value: startDateValue});
    } 
    if  (endDate !== undefined && endDate !== null) {
        var endDateValue = endDate.getFullYear() + "-" + (endDate.getMonth() + 1) + "-" + endDate.getDate();
        parameters.push({Key: "enddate", Value: endDateValue});
    } 
    
    if (searchFields !== "") {
        for (var field of searchFields) {
            parameters.push({Key: field, Value: searchText});
        }
    }
    if (parameters.length === 0 && orderBy === " ") {
        Get(queryname, dataReceivedHandler, errorHandler);
    } else {
        const criteria = { Name: queryname, Parameters: parameters, OrderBy: orderBy, OrderDescending: orderDescending };
        Post('query/filteredget', criteria, dataReceivedHandler, errorHandler);
    }
}

export default RefreshFilterList;