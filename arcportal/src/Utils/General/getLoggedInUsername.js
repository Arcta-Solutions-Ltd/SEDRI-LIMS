import { jwtDecode } from 'jwt-decode';

const USERNAME_CLAIM = 'http://schemas.microsoft.com/ws/2008/06/identity/claims/userdata';

/**
 * Returns the logged-in username from the JWT in localStorage, or an empty string.
 */
const getLoggedInUsername = () => {
    try {
        const token = localStorage.getItem('arctoken');
        if (!token) {
            return '';
        }
        const decodedToken = jwtDecode(token);
        return decodedToken[USERNAME_CLAIM] ?? '';
    } catch {
        return '';
    }
};

export default getLoggedInUsername;
