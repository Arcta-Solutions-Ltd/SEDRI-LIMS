const FlattenData = (data, properties = [], arrayName) => {

    let result = [];
  
    const recurse = (current) => {
      const entry = {};
      properties.forEach(property => {
        entry[property] = current[property];
      });
      result.push(entry);
  
      if (Array.isArray(current[arrayName])) {
        current[arrayName].forEach(child => recurse(child));
      }
    };
  
    recurse(data);
    return result;
  };
  
  export default FlattenData;
  