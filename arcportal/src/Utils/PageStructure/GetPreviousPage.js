const GetPreviousPage = (currentPage, pageStructure) => {

    const pagesToConsider = pageStructure.filter(p => p.Visible);
    const currentIndex = pagesToConsider.findIndex((page) => { return page.Name === currentPage.Name})

    const newPage = currentIndex > 0 ? pagesToConsider[currentIndex-1] : currentPage;
    return newPage;
}

export default GetPreviousPage;