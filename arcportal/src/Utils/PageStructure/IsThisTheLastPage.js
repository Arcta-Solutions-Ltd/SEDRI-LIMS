const IsThisTheLastPage = (currentPage, pageStructure) => {

    const current = pageStructure.findIndex((page) => { return page.Name === currentPage.Name})

    return current === pageStructure.length - 1;
}

export default IsThisTheLastPage;



