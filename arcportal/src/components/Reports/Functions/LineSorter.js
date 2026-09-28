function sortLines(config, invertY = false, invertX = false) {
    if (!config || !Array.isArray(config)) {
        return [];
    }

    return config.sort((a, b) => {
        if (a.Line !== b.Line) {
            return invertY ? b.Line - a.Line : a.Line - b.Line;
        }

        if (invertX) {
            return b.Left - a.Left;
        } else {
            return a.Left - b.Left;
        }
    });
}

export { sortLines as SortLines };
