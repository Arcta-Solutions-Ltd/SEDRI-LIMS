import axios from 'axios'

const Get = (queryName, successFunction, errorFunction = () => {}) => {

    RawGet("/Query/get",{ queryName: queryName }, successFunction, errorFunction);
}

// const GetList = (listModel, successFunction, errorFunction = () => {}, extraInfo) => {
//     RawGet("/list/get",listModel, successFunction, errorFunction, extraInfo);
// }

const RawGet = (endpoint, params, successFunction, errorFunction = () => {}, extraInfo) => {

    const token = localStorage.getItem("arctoken");

    const config = {
        params: params,
        headers: { Authorization: `Bearer ${token}`, 'Access-Control-Allow-Headers': '*' }
    };

    axios.get(endpoint, config).then(function (response) {
        successFunction(response.data, extraInfo);
    }).catch(error => {
        if (error.response === undefined) {
            errorFunction(error.toJSON().message);
        } else {
            errorFunction(error.response);
        }
    })
}

export default Get;
// export {GetList};
