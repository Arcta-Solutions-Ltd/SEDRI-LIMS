import { addDays, addMonths, addYears } from "@fluentui/react";

const ParseLimit = (dateString) => {

    if (dateString === undefined || dateString === null) { return undefined; }

    let dateLimit = null;
    let expression = dateString.replace(/\s/g, "");
    if (expression.slice(0, 3).toLowerCase() === 'now') {
        dateLimit = new Date(Date.now());

        if (expression.length > 5) {
            let field = expression[3];
            let delta = parseInt(expression.slice(4,20))

            switch (field.toLowerCase()) {
                case 'y':
                    dateLimit = addYears(dateLimit, delta);
                    break;
                case 'm':
                    dateLimit = addMonths(dateLimit, delta);
                    break;
                case 'd':
                    dateLimit = addDays(dateLimit, delta);
                    break;
                default:
                    break;
            }
        }
    }
    return dateLimit;
}

export default ParseLimit;
