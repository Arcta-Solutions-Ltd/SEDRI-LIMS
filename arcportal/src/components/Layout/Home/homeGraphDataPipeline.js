/**
 * Pure helpers mirroring Graph.js data shaping for dashboard tiles (no full-page filter UI).
 */

export const sectionContentsSort = (a, b) => {
    if (a.Value < b.Value) return -1;
    if (a.Value > b.Value) return 1;
    return 0;
};

export const AddNewSection = (sections, newData, labelCount) => {
    newData.sort(sectionContentsSort);
    for (const section of sections) {
        const newDataLine = newData.filter((d) => d.Value === section.Value);
        if (newDataLine.length === 0) {
            section.Data.push(0);
        } else {
            section.Data.push(newDataLine[0].Number);
        }
    }
    for (const newDataLine of newData) {
        const foundSection = sections.filter((d) => d.Value === newDataLine.Value);
        if (foundSection.length === 0) {
            const data = [];
            for (let i = 1; i < labelCount; i++) {
                data.push(0);
            }
            data.push(newDataLine.Number);
            const newSection = { Key: sections.length + 1, Value: newDataLine.Value, Data: data };
            sections.push(newSection);
        }
    }
    return sections;
};

/**
 * Sums all `Number` values in graph rows after {@link ReorganiseData}, matching the aggregate used
 * before chart series are built for home dashboard graphs. Use the same `filterState` as {@link GenerateThreeDimensions}.
 * @param {unknown} data Raw rows from `graph/getdata` (array of objects with `Number`).
 * @param {object} filterState Filter state with `filters` array (must include `dateinterval` like the graph pipeline).
 * @returns {number}
 */
export function sumGraphDashboardCounts(data, filterState) {
    if (!data || !Array.isArray(data) || !filterState?.filters) {
        return 0;
    }
    const reorganised = ReorganiseData(data, filterState);
    return reorganised.reduce((sum, row) => sum + (Number(row.Number) || 0), 0);
}

export const ReorganiseData = (data, filterState) => {
    const returnedData = [];
    let filterResults = [];
    const intervalFilter = filterState.filters.filter((f) => f.Key === 'dateinterval')[0].values[0];
    for (const item of data) {
        if (intervalFilter === '479') {
            filterResults = returnedData.filter((d) => d.Value === item.Value && d.Day === item.Day);
        }
        if (intervalFilter === '480') {
            filterResults = returnedData.filter((d) => d.Value === item.Value && d.Month === item.Month);
        }
        if (intervalFilter === '481') {
            filterResults = returnedData.filter((d) => d.Value === item.Value && d.Year === item.Year);
        }
        if (filterResults.length === 0) {
            returnedData.push(item);
        } else {
            filterResults[0].Number += item.Number;
        }
    }
    return returnedData;
};

export const GenerateThreeDimensions = (data, filterState) => {
    let currentLabel = '';
    let labelKeyCount = 0;
    let sections = [];
    const labels = [];
    let sectionDataForThisColumn = [];
    const newData = ReorganiseData(data, filterState);
    const dateInterval = filterState.filters.filter((t) => t.Key === 'dateinterval');
    for (const line of newData) {
        const newLabel =
            dateInterval[0].values[0] === '480'
                ? line.MonthName.substring(0, 3)
                : dateInterval[0].values[0] === '481'
                  ? line.Year
                  : line.Day;
        if (newLabel !== currentLabel) {
            if (sectionDataForThisColumn.length > 0) {
                sections = AddNewSection(sections, sectionDataForThisColumn, labelKeyCount);
            }
            labelKeyCount++;
            labels.push({ Key: labelKeyCount, Value: newLabel });
            currentLabel = newLabel;
            sectionDataForThisColumn = [];
        }
        sectionDataForThisColumn.push({ Value: line.Value, Number: line.Number });
    }
    if (sectionDataForThisColumn.length > 0) {
        sections = AddNewSection(sections, sectionDataForThisColumn, labelKeyCount);
    }
    return { Labels: labels, Sections: sections };
};

export const CreateTwoDimensionsFromThreeDimensions = (three) => {
    const labels = [];
    const sections = [{ Value: '', Data: [] }];
    for (const section of three.Sections) {
        labels.push({ Key: section.Key, Value: section.Value });
        const total = section.Data.reduce((p, a) => p + a, 0);
        sections[0].Data.push(total);
    }
    return { Labels: labels, Sections: sections };
};
