import Post from'./Post';


const PostEvent = (eventToPost, successFunction, errorFunction, extraInfo) => {
    Post("/event/post", eventToPost, successFunction, errorFunction, extraInfo);
}

export default PostEvent;