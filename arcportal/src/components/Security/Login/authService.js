// authService.js
import { PublicClientApplication } from '@azure/msal-browser';
import axios from 'axios';
import { msalConfig, loginRequest } from './authConfig';

class AuthService {
    constructor() {
        this.msalInstance = null;
    }

    async initializeMsal() {
        if (!this.msalInstance) {
            this.msalInstance = new PublicClientApplication(msalConfig);
            await this.msalInstance.initialize();
        }
        return this.msalInstance;
    }

    async loginToAzure() {
        const msalInstance = await this.initializeMsal();
        return await msalInstance.loginPopup(loginRequest);
    }

    async loginWithAzureAccessToken(azureAccessToken) {
        const loginResponse = await axios.post(
            axios.defaults.baseURL + '/auth/callback',
            null,
            {
                headers: {
                    Authorization: `Bearer ${azureAccessToken}`
                },
                withCredentials: true
            }
        )
        localStorage.setItem('arctoken', loginResponse.data.token);
        localStorage.setItem('auth_type', 'azure');
        return loginResponse.data.token;
    }

    async getCsrfToken() {
        const response = await axios.get(
            axios.defaults.baseURL + '/auth/csrf-token',
            { withCredentials: true }
        );
        return response.data.token;
    }

    async loginWithCredentials(username, password) {
        // WAPT-007: Fetch CSRF token before login
        const csrfToken = await this.getCsrfToken();
        
        const loginResponse = await axios.post(
            axios.defaults.baseURL + '/auth/login',
            {
                username,
                password,
            },
            {
                headers: {
                    'X-CSRF-TOKEN': csrfToken
                },
                withCredentials: true
            }
        );

        localStorage.setItem('arctoken', loginResponse.data.token);
        localStorage.setItem('auth_type', 'local');

        return loginResponse.data.token;
    }
    
    async logout() {
        const authType = localStorage.getItem('auth_type');

        localStorage.removeItem('arctoken');
        localStorage.removeItem('auth_type');

        if (authType === 'azure') {
            await this.msalInstance.logoutPopup();
        }
    }
}

export const authService = new AuthService();
