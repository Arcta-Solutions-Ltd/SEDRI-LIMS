const singleDataValue = (name, data) => {
    if (!name) { return };
    return data.Standard.find(f => f.Key.toLowerCase() === name.toLowerCase())?.Value ?? "";
  }
  
  const singleLabelText = (name, config) => {
    return config.find(f => f.Key === name)?.Label ?? "";
  }
  
  export { singleDataValue as SingleDataValue, singleLabelText as SingleLabelText };