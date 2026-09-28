import AddListsIntoPage from './AddListsIntoPage';

const GetPagesFromPageList = (pagesList, pagesDef, lists, pages, formState, rules) => {

    for (const page of pagesList) {
        let newPage = pages.filter(p => p.Name === page)[0];
        if (newPage !== undefined) {
            newPage = AddListsIntoPage(newPage, lists);
            pagesDef.push({...newPage});
        }
    }

    return pagesDef;
}

export default GetPagesFromPageList;