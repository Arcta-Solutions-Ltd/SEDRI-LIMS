import TransformDatesInJson from '../../../Utils/Local/TransformDatesInJson';

const formatReportData = (data) => {
    if (Array.isArray(data.Standard)) {
        TransformDatesInJson(data.Standard);
    }

    if (Array.isArray(data.Groups)) {
        data.Groups.forEach((group) => {
            if (Array.isArray(group.Multiple)) {
                group.Multiple.forEach((item) => {
                    if (item.Standard) {
                        TransformDatesInJson(item.Standard);
                    }
                });
            }
        });
    }
};

export { formatReportData as FormatReportData };
