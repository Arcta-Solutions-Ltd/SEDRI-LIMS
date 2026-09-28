const initialState = { 
  display: {
    showFullScreen: false,
    expandedMenuItem: '',
    username: ''
  }
}

const displayReducer = (state = initialState, action) => {

    let newState = state;

    if (action.type === 'TOGGLEFULLSCREEN') {
      newState = {
            ...state,
            showFullScreen: ! state.showFullScreen
      }
    }
    if (action.type === 'SETEXPANDEDMENUITEM') {
      newState = { 
        ...state,
        expandedMenuItem: action.value
      }
    }
    if (action.type === 'HEADINGTEXT') {
      newState = { 
        ...state,
        headingText: action.value
      }
    }

    return newState;
}

export default displayReducer;