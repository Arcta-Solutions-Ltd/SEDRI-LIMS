import { SingleDataValue } from './SingleDataValue';

const removeUnusedDoubleFieldData = (section, data) => {
    const columnOne = [];
    const columnTwo = [];

    for (const row of section.Column1?.Fields || []) {
        if (SingleDataValue(row.Value, data) !== '') {
            columnOne.push(row);
        }
    }

    for (const row of section.Column2?.Fields || []) {
        if (SingleDataValue(row.Value, data) !== '') {
            columnTwo.push(row);
        }
    }

    const newColumns = reorderDoubleColumnsWithData(columnOne, columnTwo);
    if (section.Column1) {
        section.Column1.Fields = newColumns.Column1;
    }
    if (section.Column2) {
        section.Column2.Fields = newColumns.Column2;
    }
};

const removeUnusedSingleFieldData = (section, data) => {
    const columnOne = [];

    for (const row of section.Column1.Fields) {
        if (SingleDataValue(row.Value, data) !== '') {
            columnOne.push(row);
        }
    }

    section.Column1.Fields = columnOne;
};

const reorderDoubleColumnsWithData = (columnOne, columnTwo) => {
    const newColumnOne = [];
    const newColumnTwo = [];
    const loopLength = Math.max(columnOne.length, columnTwo.length);
    let placeInFirstColumn = true;

    for (let x = 0; x < loopLength; x++) {
        if (columnOne.length > x) {
            if (placeInFirstColumn) {
                newColumnOne.push(columnOne[x]);
            } else {
                newColumnTwo.push(columnOne[x]);
            }
            placeInFirstColumn = !placeInFirstColumn;
        }
        if (columnTwo.length > x) {
            if (placeInFirstColumn) {
                newColumnOne.push(columnTwo[x]);
            } else {
                newColumnTwo.push(columnTwo[x]);
            }
            placeInFirstColumn = !placeInFirstColumn;
        }
    }

    return { Column1: newColumnOne, Column2: newColumnTwo };
};

export { removeUnusedDoubleFieldData as RemoveUnusedDoubleFieldData, removeUnusedSingleFieldData as RemoveUnusedSingleFieldData };
