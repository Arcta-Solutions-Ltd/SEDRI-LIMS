// Progress-enabled multipart POST using axios, mirroring Post.js conventions
// Signature: postMultipart(endpoint, formData, successFunction, errorFunction, extraInfo, onProgress)
import axios from 'axios';

const postMultipart = (
  endpoint,
  formData,
  successFunction,
  errorFunction,
  extraInfo,
  onProgress
) => {
  const token = localStorage.getItem('arctoken');

  const config = {
    headers: {
      Authorization: `Bearer ${token}`,
      'Access-Control-Allow-Headers': '*',
      'Content-Type': 'multipart/form-data',
    },
    onUploadProgress: (evt) => {
      if (typeof onProgress === 'function' && evt && evt.total) {
        const percent = Math.round((evt.loaded / evt.total) * 100);
        onProgress(evt.loaded, evt.total, percent);
      }
    },
  };

  axios
    .post(endpoint, formData, config)
    .then(function (response) {
      if (typeof successFunction === 'function') successFunction(response.data, extraInfo);
    })
    .catch((error) => {
      if (typeof errorFunction !== 'function') return;
      if (error.response === undefined) {
        errorFunction(error.message);
      } else {
        errorFunction(error.response);
      }
    });
};

export default postMultipart;

