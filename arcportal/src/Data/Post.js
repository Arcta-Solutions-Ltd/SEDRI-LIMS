import axios from 'axios';

const Post = (
    endpoint,
    dataToPost,
    successFunction,
    errorFunction,
    extraInfo
) => {
    const token = localStorage.getItem('arctoken');

    const config = {
        headers: {
            Authorization: `Bearer ${token}`,
            'Access-Control-Allow-Headers': '*',
        },
    };

    axios
        .post(endpoint, dataToPost, config)
        .then(function (response) {
            successFunction(response.data, extraInfo);
        })
        .catch((error) => {
            const onError = typeof errorFunction === 'function' ? errorFunction : () => {};
            if (error.response === undefined) {
                onError(error.message);
            } else {
                onError(error.response);
            }
        });
};

const PostList = (
    dataToPost,
    successFunction,
    errorFunction,
    extraInfo,
    callingId
) => {
    dataToPost[0].Id = callingId;
    Post('list/get', dataToPost, successFunction, errorFunction, extraInfo);
};

export default Post;
export { PostList };
