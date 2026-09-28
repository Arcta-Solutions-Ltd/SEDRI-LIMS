const ArraySorter = (key) => {
    return function (a,b) {

        if (a[key] === undefined && b[key] === undefined) {
            return 0;
        } else if (a[key] === undefined) {
            return 1;
        } else if (b[key] === undefined) {
            return -1;
        } else if (a[key] > b[key]) {
            return 1;
        } else if (a[key] < b[key]) {
            return -1;
        } else {
            return 0;
        }
    }
}

const StringArraySorter = (list) => {
    list.sort(function (a, b) {
        return ('' + a.attr).localeCompare(b.attr);
    })
    return list;
}

export default ArraySorter;
export { StringArraySorter};