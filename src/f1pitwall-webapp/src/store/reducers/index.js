import { combineReducers } from 'redux';
import { connectRouter } from 'connected-react-router';

import session from './Session';

const reducer = (history) =>
combineReducers({
  router: connectRouter(history),
  session
});

export default reducer;