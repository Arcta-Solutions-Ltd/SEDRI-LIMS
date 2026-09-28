import React from 'react';
import './Userface.css';
import { PersonaSize, Facepile } from '@fluentui/react';

const personProps = () => {
    return {
        hidePersonaDetails: true,
    };
};

const userFace = (props) => {
    const setInitials = (username, givenName, surname) => {
        const getFirstChar = (str) => str?.trim().charAt(0).toUpperCase() || '';
        
        const givenInitial = getFirstChar(givenName);
        const surnameInitial = getFirstChar(surname);
        
        if (givenInitial || surnameInitial) {
            return givenInitial + surnameInitial;
        }
        
        const cleanUsername = (username || '').trim();
        return cleanUsername.slice(0, 2).toUpperCase();
    };

    const setName = (username, givenName, surname) => {
        const trim = (str) => str?.trim() || '';
        
        const cleanGivenName = trim(givenName);
        const cleanSurname = trim(surname);
        
        if (cleanGivenName || cleanSurname) {
            return [cleanGivenName, cleanSurname]
                .filter(Boolean)  // Remove empty strings
                .join(' ');
        }
        
        return trim(username) || 'User';
    };

    const personaSize = PersonaSize.size24;
    const persona = {
        imageInitials: setInitials(props.username, props.givenName, props.surname),
        personaName: setName(props.username, props.givenName, props.surname),
        initialsColor: 0,
    };

    return (
        <div className="userface-content">
            <Facepile
                personaSize={personaSize}
                personas={[persona]}
                getPersonaProps={personProps}
            />
        </div>
    );
};

export default userFace;
