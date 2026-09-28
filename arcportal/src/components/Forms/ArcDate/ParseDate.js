import parseLocalDate from '../../../Utils/General/ParseLocalDate';

const ParseDate = (newValue) => {
    if (newValue.includes("/")) {
        const values = newValue.trim().split("/");
        const day = parseInt(values[0], 10);
        const month = parseInt(values[1], 10) - 1;
        const year = parseInt(values[2], 10);
        return new Date(year, month, day);
    }
    if (newValue.includes("-")) {
        return parseLocalDate(newValue.trim());
    }
};

export default ParseDate;
