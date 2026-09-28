const setGridLinesUtil = (number, gridLines) => {
    const newLines = gridLines.filter(f => f.number !== number);

    const newLinesToSave = [];
    let numberCount = 1;
    for (const line of newLines) {
        const newLine = {...line, number: numberCount};
        newLinesToSave.push(newLine);
        numberCount++;
    }

    return newLinesToSave;
}

const setGridDataUtil = (number, gridData) => {
    const newData = gridData.filter(f => f.lineNumber !== number);
    let numberCount = 1;
    for (const line of newData) {
        line.lineNumber = numberCount;
        numberCount++;
    }
    return newData;
}

const resetLineNumberUtil = (data) => {
    const changedData = [];
    for (const line of data) {
        const changedLine = {...line};
        delete changedLine.lineNumber;
        changedData.push(changedLine);
    }        
    return changedData;
}

export {setGridLinesUtil, setGridDataUtil, resetLineNumberUtil }