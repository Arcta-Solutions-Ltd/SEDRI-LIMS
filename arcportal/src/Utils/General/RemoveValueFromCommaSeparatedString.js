const RemoveValueFromCommaSeparatedString = (list, value, separator) => {
    separator = separator || ",";
    var values = list.split(separator);
    for(var i = 0 ; i < values.length ; i++) {
      if(values[i] === value) {
        values.splice(i, 1);
        return values.join(separator);
      }
    }
    return list;
}

export default RemoveValueFromCommaSeparatedString;