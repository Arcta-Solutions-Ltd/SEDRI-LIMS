const IsThisTheFirstPage = (currentPage, pageStructure) => {

    const current = pageStructure.findIndex((page) => { return page.Name === currentPage.Name})

    return current === 0;
}

export default IsThisTheFirstPage;