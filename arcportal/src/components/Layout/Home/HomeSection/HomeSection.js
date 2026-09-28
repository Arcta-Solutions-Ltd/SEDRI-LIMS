import React from 'react';
import './HomeSection.css';
import HomeCardItem from '../HomeCardItem/HomeCardItem';

const HomeSection = (props) => {

    return (
        <React.Fragment>
            <div className='home-sectiontitle'>
                {props.data.length > 0 && props.title}
            </div>
            <div className='homesection-fieldcontent'>
                {props.data.length > 0 && props.data.map((field, index) => (
                    <HomeCardItem  key={index} type={props.type} value={field.Value} id={field.Id} number={field.Number} onClick={props.onClick}></HomeCardItem>
                ))}
            </div>  
        </React.Fragment>
    )
};

export default HomeSection;