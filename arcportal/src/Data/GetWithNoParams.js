import axios from 'axios'

const GetWithNoParams = (endpoint, successFunction, errorFunction = () => {}, token = "") => {

    if (token === "") {
        token = localStorage.getItem("arctoken");
    }
    
    const config = {
        params: { },
        headers: { Authorization: `Bearer ${token}`, 'Access-Control-Allow-Headers': '*' }
    };

    axios.get(endpoint,config).then(function (response) {
        successFunction(response.data);
    }).catch(error => {
        errorFunction(error.response);
    })
}

export default GetWithNoParams;

