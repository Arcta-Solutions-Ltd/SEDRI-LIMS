const StringUtils = {
    containsString: function(list, target) {
        return list
            .split(',')
            .map(function(item) { return item.trim(); })
            .includes(target);
    },

    // Parses a JSON string into an object; if already an object, returns it.
    // Returns the original value if parsing fails.
    parseJsonIfString: function(value) {
        if (typeof value !== 'string') {
            return value;
        }
        try {
            return JSON.parse(value);
        } catch (e) {
            return value;
        }
    }
};

export default StringUtils;