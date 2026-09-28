class TokenInfo {
    
    constructor(tokenName) {
        const token = localStorage.getItem(tokenName);

        this.tokenInfo = this.#GetTokenDetails(token);
    }

    get LaboratoryId() {
        return this.tokenInfo.LaboratoryId;
    }

    get LanguageId() {
        return this.tokenInfo.LanguageId;
    }

    get OrganisationId() {
        return this.tokenInfo.OrganisationId;
    }

    get AuthMethod() {
        return this.tokenInfo.AuthMethod;
    }

    get AllowedLaboratories() {
        return this.tokenInfo.AllowedLaboratories;
    }

    get AllowedOrganisations() {
        return this.tokenInfo.AllowedOrganisations;
    }

    #GetTokenDetails = (token) => {
        const base64Url = token.split('.')[1];
        const base64 = base64Url.replace(/-/g, '+').replace(/_/g, '/');
        const jsonPayload = decodeURIComponent(
            atob(base64)
            .split('')
            .map(c => '%' + ('00' + c.charCodeAt(0).toString(16)).slice(-2))
            .join('')
        );

        return JSON.parse(jsonPayload);
    }
}

export default TokenInfo