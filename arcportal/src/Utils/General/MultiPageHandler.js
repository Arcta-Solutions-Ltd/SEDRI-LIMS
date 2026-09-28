const AdjustPageToHandleMultiPageView = (page, groups, fullScreen) => {

    if (groups === undefined || groups === "") {
        page.MultiPageView = false;
    } else {
        page.MultiPageView = fullScreen;
    }

    return page;
}

export default AdjustPageToHandleMultiPageView;