const FormatDataIntoGraph = (threeDimensions, twoDimensions, colours, type) => {
    
    const data = type === "donut" || type === "pie" || type === "polar" ? twoDimensions : threeDimensions;
    const returnData = {
        labels: data.Labels.map(d => d.Value),
        datasets: []
    };

    if (data.Sections.length > 1) {
        let colourCount = 0;
        for (const section of data.Sections) {
            const sectionColours = section.Data.map(d => colours[colourCount])
            const newDataSet = {data: section.Data, backgroundColor: sectionColours, label: section.Value, fill: type === 'radar'};

            //newDataSet.stack = stack ? "stack" : undefined;

            returnData.datasets.push(newDataSet);
            colourCount++;
        }
    } else {
        const dataVal = data.Sections.length > 0 ? data.Sections[0].Data : [];
        const labelVal = data.Sections.length > 0 ? data.Sections[0].Value : [];        
        const newDataSet = {data: dataVal, backgroundColor: colours, label: labelVal};
        returnData.datasets.push(newDataSet);
    }

    if (type === 'radar') {
        for (const ds of returnData.datasets) {
            ds.borderColor = ds.backgroundColor;
            ds.fill = true;
        }
    }

    return returnData;
}

export default FormatDataIntoGraph;

