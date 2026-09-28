const AddBlankCraftedStructureForPagesWithoutData = (pages, data) => {

    const datacopy = Array.isArray(data) ? [...data] : data === undefined || data === null ? [] : {...data};
//   const datacopy = Array.isArray(data) ? data : [];

    // The copy seeds the first crafted page with the record's values. It must not carry the crafted list,
    // because that same list is what the new page is pushed into, which would make the form data circular
    // and unserialisable on save.
    if (!Array.isArray(datacopy)) {
        delete datacopy.Crafted;
    }

    let pageCount = 1;
    for (const page of pages) {
        if (page.Crafted) {
            data.Crafted = data.Crafted === undefined ? [] : data.Crafted;
            const craftedItem = data.Crafted.findIndex((item) => item.Name === page.Name);
            if (craftedItem === -1) {
                const newContents = pageCount === 1 ? datacopy : [];
                const newItem = { Name: page.Name, Contents: newContents};
                data.Crafted.push(newItem);
            }
        }
        pageCount++;
    };
}

export { AddBlankCraftedStructureForPagesWithoutData }