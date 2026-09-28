import React from 'react';
import { Breadcrumb } from '@fluentui/react/lib/Breadcrumb';

const ArcBreadcrumb = (props) => {

    // const pagesToDisplay = props.items.filter(p => p.Visible);
    // const items = pagesToDisplay.map(p =>  { return {text: p.PageTitle, key: p.Name }})

    const style = {
        item: {
            color: "rgb(16, 110, 190)", fontSize: "10pt"
        }
    };

    const items = props.items !== undefined && props.items.length > 1 ? props.items : [];

    return (
        <React.Fragment>
            <Breadcrumb
                items={items}
                maxDisplayedItems={10}
                ariaLabel="Breadcrumb with items rendered as buttons"
                overflowAriaLabel="More links"
                styles={style}
            />
        </React.Fragment>
    )
}

export default ArcBreadcrumb;