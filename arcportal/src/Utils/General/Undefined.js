const Undefined = (field) => {
    return field === undefined || field === null;
}

const NotNullOrUndefined = (field) => {
    return field !== undefined && field !== null;
}

export default Undefined;

export {NotNullOrUndefined};