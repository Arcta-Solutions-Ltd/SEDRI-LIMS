function csvToArray(csvString) {
    if (!csvString) {
      return [];
    }
    return csvString.split(',').map(item => item.trim());
}
  
export default csvToArray;